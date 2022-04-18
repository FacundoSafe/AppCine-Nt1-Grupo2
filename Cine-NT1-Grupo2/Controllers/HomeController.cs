using Cine_NT1_Grupo2.Context;
using Cine_NT1_Grupo2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Cine_NT1_Grupo2.Controllers
{
    /* Por lo visto la clase home no tiene context creado por lo que se lo creamos nosotros para poder mostrar las peliculas en el index home*/

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CineContext _context;
        public HomeController(ILogger<HomeController> logger, CineContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View( _context.Pelicula.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
