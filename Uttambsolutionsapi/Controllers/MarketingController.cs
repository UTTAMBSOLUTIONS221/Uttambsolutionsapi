using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Speech.Synthesis;

namespace Uttambsolutionsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketingController : ControllerBase
    {

        [HttpGet("videomarketer")]
        public async Task<IActionResult> VideoMarketer(string ParsedText, int Rate, string VoiceGender)
        {
            try
            {
                //var speechFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "speech.wav");

                string speechFileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos", "speech.wav");
                if (!Directory.Exists(speechFileName))
                {
                    Directory.CreateDirectory(speechFileName);
                }

                using (var synth = new SpeechSynthesizer())
                {
                    synth.SelectVoiceByHints(ParseVoiceGender(VoiceGender), VoiceAge.Adult);
                    synth.SetOutputToWaveFile(speechFileName);
                    synth.Rate = Rate;
                    synth.Speak(ParsedText);
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(speechFileName);
                return File(fileBytes, "audio/wav", "speech.wav");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private VoiceGender ParseVoiceGender(string gender)
        {
            if (string.Equals(gender, "Male", StringComparison.OrdinalIgnoreCase))
            {
                return VoiceGender.Male;
            }
            else
            {
                return VoiceGender.Female;
            }
        }

        [HttpGet("generate")]
        public IActionResult GenerateVideo()
        {
            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string videoFileName = $"video_{DateTime.Now.Ticks}.mp4";
            string videoFilePath = Path.Combine(outputDirectory, videoFileName);

            GenerateImages(outputDirectory);

            if (!CreateVideoFromImages(outputDirectory, videoFilePath))
            {
                return BadRequest("Failed to create video.");
            }

            var fileStream = new FileStream(videoFilePath, FileMode.Open);
            return File(fileStream, "video/mp4", videoFileName);
        }

        private void GenerateImages(string outputDirectory)
        {
            // Generate images from text here
            for (int i = 0; i < 10; i++)
            {
                string text = $"Frame {i + 1}";
                string imagePath = Path.Combine(outputDirectory, $"frame_{i + 1}.png");
                CreateImageFromText(text, imagePath);
            }
        }

        private void CreateImageFromText(string text, string imagePath)
        {
            int width = 800;
            int height = 600;

            using (var bitmap = new Bitmap(width, height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);
                    Font font = new Font("Arial", 40, FontStyle.Bold, GraphicsUnit.Pixel);
                    PointF point = new PointF(50, 200);
                    SolidBrush brush = new SolidBrush(Color.Black);

                    graphics.DrawString(text, font, brush, point);
                }

                bitmap.Save(imagePath);
            }
        }

        private bool CreateVideoFromImages(string imageDirectory, string videoFilePath)
        {
            try
            {
                // Path to ffmpeg executable
                string ffmpegPath = @"C:\path\to\ffmpeg.exe"; // Update with the actual path to ffmpeg

                // Command to create video from images
                string arguments = $"-framerate 24 -i \"{Path.Combine(imageDirectory, "frame_%d.png")}\" -c:v libx264 -pix_fmt yuv420p \"{videoFilePath}\"";

                // Start the ffmpeg process
                ProcessStartInfo psi = new ProcessStartInfo(ffmpegPath, arguments)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                };

                using (Process ffmpegProcess = new Process { StartInfo = psi })
                {
                    ffmpegProcess.Start();
                    ffmpegProcess.WaitForExit();

                    // Check if the video file was created successfully
                    return System.IO.File.Exists(videoFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating video: {ex.Message}");
                return false;
            }
        }

    }
}
