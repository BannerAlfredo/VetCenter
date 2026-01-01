using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetCenter.Data;
using VetCenter.Filters;
using VetCenter.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace VetCenter.Controllers
{
    [Autenticado]
    public class ClienteController : Controller
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        // 1. LISTAR (Index) con paginación
        public IActionResult Index(int pageNumber = 1)
        {
            var clientes = _context.Clientes
                .OrderBy(c => c.Id); // Ordenar por ID

            return View(clientes.ToPagedList(pageNumber, 5)); // ✅ 10 registros por página
        }

        // 2. FORMULARIO NUEVO
        public IActionResult Nuevo()
        {
            // Enviamos un objeto vacío para evitar errores de nulos en la vista
            return View("ClienteForm", new Cliente { Nombre = "", Apellido = "" });
        }

        // 3. FORMULARIO EDITAR
        public async Task<IActionResult> Editar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return View("ClienteForm", cliente);
        }

        // 4. GUARDAR (Create/Update)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (cliente.Id == 0)
                    _context.Clientes.Add(cliente);
                else
                    _context.Clientes.Update(cliente);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("ClienteForm", cliente);
        }

        // 5. DETALLES
        public async Task<IActionResult> Detalles(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Mascotas) // Incluimos mascotas para verlas en el detalle
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // 6. ELIMINAR
        public async Task<IActionResult> Eliminar(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Mascotas)
                .ThenInclude(m => m.Citas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente != null)
            {
                // Borramos primero las citas de cada mascota
                foreach (var mascota in cliente.Mascotas)
                {
                    if (mascota.Citas != null && mascota.Citas.Any())
                        _context.Citas.RemoveRange(mascota.Citas);
                }

                // Luego las mascotas
                if (cliente.Mascotas != null && cliente.Mascotas.Any())
                    _context.Mascotas.RemoveRange(cliente.Mascotas);

                // Finalmente el cliente
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
