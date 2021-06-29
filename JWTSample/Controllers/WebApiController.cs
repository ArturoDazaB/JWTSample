﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JWTSample.Models;
using JWTSample.JWT;
using Microsoft.AspNetCore.Authorization;

namespace JWTSample.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class WebApiController : ControllerBase
    {
        private readonly ICustomAuthenticationManager customauthenticationManager;

        public WebApiController(ICustomAuthenticationManager customauthenticationManager)
        {
            this.customauthenticationManager = customauthenticationManager;
        }

        [HttpGet]
        public List<string> main()
        {
            return new List<string> {
                "Arturo", "Alberto", "Angelica", "Santiago",   
            };
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LogInRequest credentials) 
        {

            var token = customauthenticationManager.Authenticate(credentials.Username, credentials.Password);

            if (token != null)
                return Ok(token);
            else
                return Unauthorized();
        }
    }
}
