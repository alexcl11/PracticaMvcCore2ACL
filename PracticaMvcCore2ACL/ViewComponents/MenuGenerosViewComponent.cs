using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2ACL.Models;
using PracticaMvcCore2ACL.Repositories;

namespace PracticaMvcCore2ACL.ViewComponents
{
    public class MenuGenerosViewComponent: ViewComponent
    {
        private RepositoryLibros repo;
        public MenuGenerosViewComponent(RepositoryLibros repo) 
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Generos> generos = await this.repo.GetGenerosAsync();
            return View(generos);
        }
    }
}
