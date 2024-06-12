using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Uttambsolutionsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketingController : ControllerBase
    {
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
            // Implement video creation logic using the images in the directory
            // This could involve using a third-party library like FFmpeg or System.Drawing.Common to encode images into a video format
            // Due to the complexity of video creation, this part may require additional configuration and setup
            // For simplicity, I'm omitting this part in this example.
            return true;
        }
    }
}
