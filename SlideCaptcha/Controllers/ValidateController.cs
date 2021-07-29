using Microsoft.AspNetCore.Mvc;
using SlideCaptcha.Models.Vaidate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace SlideCaptcha.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ValidateController : ControllerBase
    {
        ValidateResponseModel _Response;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ValidateController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _Response = new ValidateResponseModel();
        }

        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {
            var e = HttpContext.Session.GetInt32("MoveX");
            var e2 = HttpContext.Session.GetInt32("MoveY");
            var imageTool = new Tools.ImageTool();
            //var base64Str = imageTool.ImageToBase64("C:\\Test\\Test.jpg");
            var url = "\\upload\\images\\TestZ.png";
            var random = new Random().Next(1, 8);
            url = url.Replace("Z", random.ToString());

            string webRootPath = _webHostEnvironment.WebRootPath;

            var base64Obj = imageTool.GetNewValidateImageBase(webRootPath + url);

            if (string.IsNullOrEmpty(base64Obj.OriginalImageBase64))
            {
                _Response.isOk = false;
                _Response.msg = "出现了异常";
            }
            else
            {
                HttpContext.Session.SetInt32("MoveX", base64Obj.X1);
                HttpContext.Session.SetInt32("MoveY", base64Obj.Y1);
                _Response.isOk = true;
                _Response.data = base64Obj;
            }
            return Ok(_Response);
        }

        [HttpGet("Check")]
        public async Task<IActionResult> Check(int X, int Y)
        {
            var needMoveX = HttpContext.Session.GetInt32("MoveX");
            var needMoveY = HttpContext.Session.GetInt32("MoveY");
            if (!needMoveX.HasValue)
            {
                _Response.isOk = false;
                _Response.msg = "验证失败";
                return Ok(_Response);
            }
            if (System.Math.Abs(X - needMoveX.Value) <= 5 && System.Math.Abs(Y - needMoveY.Value) <= 5)
            {
                _Response.isOk = true;
            }
            else
            {
                _Response.isOk = false;
                _Response.msg = "验证失败";
            }
            return Ok(_Response);
        }
    }
}
