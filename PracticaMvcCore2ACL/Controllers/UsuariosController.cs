using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using PracticaMvcCore2ACL.Extensions;
using PracticaMvcCore2ACL.Filters;
using PracticaMvcCore2ACL.Models;
using PracticaMvcCore2ACL.Repositories;
using System.Security.Claims;

namespace PracticaMvcCore2ACL.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositoryLibros repo;
        public UsuariosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        [AuthorizeUser]
        public IActionResult Perfil()
        {            
            return View();
        }
        [AuthorizeUser]
        public async Task<IActionResult> PedidosUser()
        {
            int idUser = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<VistaPedidos> pedidos = await this.repo.GetPedidosUserAsync(idUser);
            return View(pedidos);
        }

        public async Task<IActionResult> Carrito(int? id)
        {
            List<Libros> libros = HttpContext.Session.GetObject<List<Libros>>("CARRITO");
            

            if (id != null)
            {
                Libros libro = await this.repo.FindLibroAsync(id.Value);
                if (libros != null)
                {
                    if (!libros.Contains(libro))
                    {
                        libros.Add(libro);
                    }
                    HttpContext.Session.SetObject("CARRITO", libros);
                }
                else
                {
                    libros = new List<Libros>();
                    libros.Add(libro);
                    HttpContext.Session.SetObject("CARRITO", libros);
                }
            }
            return View(libros);
        }

        public async Task<IActionResult> QuitarDelCarro(int id)
        {
            Libros libro = await this.repo.FindLibroAsync(id);
            List<Libros> libros =
                HttpContext.Session.GetObject<List<Libros>>("CARRITO");
            libros.RemoveAll(p => p.IdLibro == libro.IdLibro);
            if (libros.Count() == 0)
            {
                HttpContext.Session.SetObject("CARRITO", null);

            }
            else
            {
                HttpContext.Session.SetObject("CARRITO", libros);

            }
            return RedirectToAction("Carrito", "Usuarios");
        }
        [AuthorizeUser]
        public async Task<IActionResult> ComprarCarrito()
        {
            List<Libros> carrito = HttpContext.Session.GetObject<List<Libros>>("CARRITO");
            int idUsuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await this.repo.ComprarCarroAsync(carrito, idUsuario);
            HttpContext.Session.Clear();
            return RedirectToAction("PedidosUser", "Usuarios");
        }
    }
}
