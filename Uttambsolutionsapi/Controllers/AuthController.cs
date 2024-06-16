﻿using DBL;
using DBL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace Uttambsolutionsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BL bl;
        IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
            _config = config;
        }

        [AllowAnonymous]
        [Route("Authenticate"), HttpPost]
        public async Task<ActionResult> AuthenticateAsync([FromBody] Usercred userdata)
        {
            var _userData = await bl.ValidateSystemStaff(userdata.username, userdata.password);
            if (_userData.RespStatus == 1)
                return Unauthorized(new UsermodelResponce
                {
                    RespStatus = 401,
                    RespMessage = _userData.RespMessage,
                    Token = "",
                    Usermodel = new UsermodeldataResponce()
                });
            if (_userData.RespStatus == 2)
                return StatusCode(StatusCodes.Status500InternalServerError, _userData.RespMessage);
            var claims = new[] {
                     new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                     new Claim("UserId", _userData.Usermodel.Userid.ToString()),
                 };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: signIn);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new UsermodelResponce
            {
                RespStatus = 200,
                RespMessage = "Ok",
                Token = tokenString,
                Usermodel = _userData.Usermodel
            });
        }
    }
}
