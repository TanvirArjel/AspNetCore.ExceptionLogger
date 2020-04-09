using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AspNetCore.ExceptionLogger.Demo.Models;
using AspNetCore.ExceptionLogger.Services;
using AspNetCore.ExceptionLogger;

namespace AspNetCore.ExceptionLogger.Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IExceptionService _exceptionService;

        public HomeController(IExceptionService exceptionService)
        {
            _exceptionService = exceptionService;
        }

        public async Task<IActionResult> Index()
        {
            ExceptionModel exceptionAsync = await _exceptionService.GetExceptionAsync(1);
            return View();
        }

        public IActionResult Privacy()
        {
            Convert.ToInt32("Hello");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
