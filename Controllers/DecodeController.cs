﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MareSueEncoder.Models;
using MareSueEncoder.Lib;
using System.Text;

namespace MareSueEncoder.Controllers
{
    [Route("api/[controller]")]
    public class DecodeController : Controller
    {
        private ILogger _logger;

        public DecodeController(ILogger<DecodeController> logger)
        {
            _logger = logger;
        }

        [HttpPost()]
        public IActionResult Post([FromBody]DecodeParam param)
        {
            if (param == null || string.IsNullOrWhiteSpace(param.Code))
            {
                _logger.LogError("No param in decoding.");
                return BadRequest("no param");
            }

            var code = param.Code.Trim();
            try
            {
                var sourceAes = EncodeTool.StringToByteArray(code);
                var source = AESTool.Decrypt(sourceAes);
                var sourceStr = Encoding.UTF8.GetString(source);

                _logger.LogInformation("Decoding successfully.\nCode: {0} \nSource:  {1}", code, sourceStr);
                return new JsonResult(new DecodeResult() { Source = sourceStr });
            }
            catch(Exception ex)
            {
                _logger.LogError("Decoding error for string:\n {0} \nError: {1}", code, ex.Message);
                return BadRequest("decode error");
            }
        }
    }
}
