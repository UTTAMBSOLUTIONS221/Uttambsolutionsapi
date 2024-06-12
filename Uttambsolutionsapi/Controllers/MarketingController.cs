using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
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
                var speechFileName = "speech.wav";
                string directory = Path.Combine("wwwroot", "Content", speechFileName);
                // Ensure the directory exists
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var synth = new SpeechSynthesizer())
                {
                    synth.SelectVoiceByHints(ParseVoiceGender(VoiceGender), VoiceAge.Adult);
                    synth.SetOutputToWaveFile(speechFileName);
                    synth.Rate = Rate;
                    synth.Volume = 100;
                    synth.Speak(ParsedText);
                }

                // Check if the file exists before attempting to read
                if (System.IO.File.Exists(speechFileName))
                {
                    var fileBytes = await System.IO.File.ReadAllBytesAsync(speechFileName);
                    return File(fileBytes, "audio/wav", "speech.wav");
                }
                else
                {
                    return NotFound(); // File not found
                }
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
    }
}
