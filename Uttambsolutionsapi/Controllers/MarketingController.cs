using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace Uttambsolutionsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketingController : ControllerBase
    {
        [HttpGet("generatevideo")]
        public async Task<IActionResult> GenerateVideo(string ParsedText, int Rate, string VoiceGender)
        {
            try
            {
                string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                string videoFileName = $"video_{DateTime.Now.Ticks}.mp4";
                string videoFilePath = Path.Combine(outputDirectory, videoFileName);

                // Generate images
                GenerateImages(outputDirectory);

                // Generate speech and get the path
                string speechFilePath = await GenerateSpeech(ParsedText, Rate, VoiceGender);

                // Create video from images and include audio
                if (!CreateVideoFromImages(outputDirectory, videoFilePath, speechFilePath))
                {
                    return BadRequest("Failed to create video.");
                }

                // Return the video file
                var fileStream = new FileStream(videoFilePath, FileMode.Open);
                return File(fileStream, "video/mp4", videoFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<string> GenerateSpeech(string ParsedText, int Rate, string VoiceGender)
        {
            try
            {
                
                var speechFileName = "speech.wav";
                string directory = Path.Combine("wwwroot", "Content");
                string speechFilePath = Path.Combine(directory, speechFileName);

                // Ensure the directory exists
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var synth = new SpeechSynthesizer())
                {
                    synth.SelectVoiceByHints(ParseVoiceGender(VoiceGender), VoiceAge.Adult);
                    synth.SetOutputToWaveFile(speechFilePath);
                    synth.Rate = Rate;
                    synth.Volume = 100;
                    synth.Speak(ParsedText);
                }

                // Check if the file exists before attempting to read
                if (!System.IO.File.Exists(speechFilePath))
                {
                    throw new FileNotFoundException("Speech file not found after generation.");
                }

                return speechFilePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating speech: {ex.Message}");
                throw;
            }
        }

        private VoiceGender ParseVoiceGender(string gender)
        {
            return string.Equals(gender, "Male", StringComparison.OrdinalIgnoreCase) ? VoiceGender.Male : VoiceGender.Female;
        }

        private void GenerateImages(string outputDirectory)
        {
            // Generate images from text here
            for (int i = 0; i < 10; i++)
            {
                string text = $"Frame {i + 1}";
                string imagePath = Path.Combine(outputDirectory, $"frame_{i + 1}.png");

                // Apply cartoon effect to the image
                Mat image = CreateImageFromText(text);
                image.SaveImage(imagePath);
            }
        }

        private Mat CreateImageFromText(string text)
        {
            int width = 800;
            int height = 600;

            // Create a white background image
            Mat image = Mat.Zeros(height, width, MatType.CV_8UC3);
            image.SetTo(Scalar.White);

            // Add text to the image
            Point textPosition = new Point(width / 4, height / 2);
            Cv2.PutText(image, text, textPosition, HersheyFonts.HersheyComplex, 2.0, Scalar.Black, 2);

            return image;
        }

        private bool CreateVideoFromImages(string imageDirectory, string videoFilePath, string speechFilePath)
        {
            try
            {
                // Path to ffmpeg executable
                string ffmpegPath = @"C:\path\to\ffmpeg.exe"; // Update with the actual path to ffmpeg

                // Command to create video from images and audio
                string arguments = $"-framerate 24 -i \"{Path.Combine(imageDirectory, "frame_%d.png")}\" -i \"{speechFilePath}\" -c:v libx264 -c:a aac -strict experimental -pix_fmt yuv420p \"{videoFilePath}\"";

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
