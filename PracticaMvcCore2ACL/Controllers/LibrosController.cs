using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2ACL.Extensions;
using PracticaMvcCore2ACL.Filters;
using PracticaMvcCore2ACL.Models;
using PracticaMvcCore2ACL.Repositories;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PracticaMvcCore2ACL.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;
        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Libros> libros = await this.repo.GetLibrosAsync();
            return View(libros);
        }
        public async Task<IActionResult> Details(int id)
        {

            ViewData["CARRITO"] = HttpContext.Session.GetObject<List<Libros>>("CARRITO");
            Libros libro = await this.repo.FindLibroAsync(id);
            return View(libro);
        }
        public async Task<IActionResult> LibrosPorGenero(int id)
        {
            string nombreGenero = await this.repo.GetNombreGenero(id);
            ViewData["GENERO"] = nombreGenero;
            List<Libros> libros = await this.repo.GetLibrosPorGeneroAsync(id);
            return View(libros);
        }
        
    }
}
