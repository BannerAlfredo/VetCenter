using Microsoft.EntityFrameworkCore;
using VetCenter.Data;
using VetCenter.Models;

namespace VetCenter.Services
{
    public class MascotaService
    {
        private readonly AppDbContext _context;

        // Inyección por constructor (No se usa @Autowired)
        public MascotaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Mascota>> ListarAsync()
        {
            // .Include es como el EAGER loading de JPA para traer al Cliente
            return await _context.Mascotas.Include(m => m.Cliente).ToListAsync();
        }

        public async Task<Mascota?> ObtenerAsync(int id)
        {
            return await _context.Mascotas
                                 .Include(m => m.Cliente)
                                 .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task GuardarAsync(Mascota mascota)
        {
            if (mascota.Id == 0)
                _context.Mascotas.Add(mascota);
            else
                _context.Mascotas.Update(mascota);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota != null)
            {
                _context.Mascotas.Remove(mascota);
                await _context.SaveChangesAsync();
            }
        }
    }
}
