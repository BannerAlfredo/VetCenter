using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VetCenter.Models;

namespace VetCenter.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }


    }
}
