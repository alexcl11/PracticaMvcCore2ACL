using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2ACL.Models;
using PracticaMvcCore2ACL.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PracticaMvcCore2ACL.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryLibros repo;
        public ManagedController(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Usuarios user = await this.repo.LoginUsuarioAsync(email, password);
            if(user != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role
                    );
                Claim claimNombre = new Claim(ClaimTypes.Name, user.Nombre);
                identity.AddClaim(claimNombre);
                Claim claimEmail = new Claim("Email", user.Email);
                identity.AddClaim(claimEmail);
                Claim claimIdUser = new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString());
                identity.AddClaim(claimIdUser);
                Claim claimApellido = new Claim("Apellido", user.Apellido);
                identity.AddClaim(claimApellido);
                Claim claimPass = new Claim("Pass", user.Pass);
                identity.AddClaim(claimPass);
                Claim claimFoto = new Claim("Foto", user.Foto);
                identity.AddClaim(claimFoto);

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal
                    );

                string controller = TempData["CONTROLLER"].ToString();
                string action = TempData["ACTION"].ToString();

                if (TempData["id"] != null)
                {
                    string id = TempData["id"].ToString();

                    return RedirectToAction(action, controller, new { id = id });
                }
                else
                {
                    return RedirectToAction(action, controller);
                }
            }
            else
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
            (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
