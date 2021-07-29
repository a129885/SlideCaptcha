using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SlideCaptcha.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SlideCaptcha.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Captcha()
        {
            var imageTool = new Tools.ImageTool();
            //var base64Str = imageTool.ImageToBase64("C:\\Test\\Test.jpg");  png才带透明通道！
            var res = imageTool.GetNewValidateImageBase("C:\\Test\\Test.png");
            ViewBag.ImageBase64 = res.OriginalImageBase64;
            ViewBag.ImageBase64Snip = res.SnipImageBase64;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
