using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2ACL.Data;
using PracticaMvcCore2ACL.Models;

namespace PracticaMvcCore2ACL.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;
        public RepositoryLibros(LibrosContext context)
        {
            this.context = context;
        }
        public async Task<List<Libros>> GetLibrosAsync()
        {
            return await this.context.Libros.ToListAsync();
        }
        public async Task<List<Generos>> GetGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }
        public async Task<List<Libros>> GetLibrosPorGeneroAsync(int idGenero)
        {
            return await this.context.Libros.Where(l => l.IdGenero == idGenero).ToListAsync();
        }
        public async Task<Libros> FindLibroAsync(int idLibro)
        {
            return await this.context.Libros.Where(l => l.IdLibro == idLibro).FirstOrDefaultAsync();
        }
        public async Task<string> GetNombreGenero(int id)
        {
            Generos genero = await this.context.Generos.Where(g => g.IdGenero == id).FirstOrDefaultAsync();
            return genero.Nombre;
        }
        public async Task<Usuarios> LoginUsuarioAsync(string email, string password)
        {
            return await 
                this.context.Usuarios.Where(u => u.Email == email && u.Pass == password).FirstOrDefaultAsync();
        }
        public async Task<List<VistaPedidos>> GetPedidosUserAsync(int idUser)
        {
            return await this.context.VistaPedidos.Where(p => p.IdUsuario == idUser).ToListAsync();
        }

        public async Task ComprarCarroAsync(List<Libros> carrito, int idUser)
        {
            
            int maxIdFactura = await this.context.Pedidos.MaxAsync(p => p.IdFactura);
            int cont = 0;
            foreach (Libros libro in carrito)
            {
                int maxIdPedido = await this.context.Pedidos.MaxAsync(p => p.IdPedido);
                cont++;
                Pedido pedido = new Pedido
                {
                    IdPedido = maxIdPedido + cont,
                    IdFactura = maxIdFactura + 1,
                    Fecha = DateTime.Now,
                    IdLibro = libro.IdLibro,
                    IdUsuario = idUser,
                    Cantidad = 1
                };
                await this.context.Pedidos.AddAsync(pedido);
            }
            await this.context.SaveChangesAsync();
        }
    }
}
