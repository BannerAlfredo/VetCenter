using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetCenter.Data;
using VetCenter.Models;

namespace VetCenter.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Usuario/Index
        // GET: Usuario/Index
        public async Task<IActionResult> Index(string buscar, int? pageNumber)
        {
            // 1. Configuración de paginación
            int pageSize = 5; // <--- AQUÍ CAMBIAS CUÁNTOS USUARIOS VER POR PÁGINA
            int pageIndex = pageNumber ?? 1;

            // 2. Empezamos con la consulta base
            var query = _context.Usuarios.AsQueryable();

            // 3. Filtro de búsqueda (igual que antes)
            if (!string.IsNullOrEmpty(buscar))
            {
                query = query.Where(u =>
                    u.Nombre.Contains(buscar) ||
                    u.Apellido.Contains(buscar) ||
                    u.Email.Contains(buscar) ||
                    u.Rol.Contains(buscar)
                );
            }

            // 4. Calcular el total de elementos (para saber cuántas páginas dibujar)
            int count = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            // 5. Aplicar el recorte (Paginación)
            // Skip: Salta los registros de las páginas anteriores
            // Take: Toma solo la cantidad definida (pageSize)
            var items = await query.OrderBy(u => u.Id)
                                   .Skip((pageIndex - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            // 6. Pasar datos a la Vista (ViewBag es lo más rápido)
            ViewData["BusquedaActual"] = buscar;
            ViewBag.CurrentPage = pageIndex;
            ViewBag.TotalPages = totalPages;

            return View(items);
        }

        // GET: Usuario/Nuevo
        // Esta acción es la que llama el botón "Agregar"
        public IActionResult Nuevo()
        {
            // Retornamos la vista del formulario enviando un usuario vacío
            return View("UsuarioForm", new Usuario());
        }

        // GET: Usuario/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            if (id == 0) return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            // Reutilizamos la misma vista "UsuarioForm" para editar
            return View("UsuarioForm", usuario);
        }

        // POST: Usuario/Guardar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(Usuario usuario)
        {
            // LÓGICA ESPECIAL PARA CONTRASEÑA EN EDICIÓN:
            // Si estamos editando (Id > 0) y el campo Password viene vacío,
            // eliminamos el error de validación para mantener la contraseña actual.
            if (usuario.Id > 0 && string.IsNullOrWhiteSpace(usuario.Password))
            {
                ModelState.Remove("Password");
            }

            if (ModelState.IsValid)
            {
                if (usuario.Id == 0) // --- CREANDO NUEVO ---
                {
                    _context.Usuarios.Add(usuario);
                    TempData["Mensaje"] = "Usuario creado correctamente.";
                }
                else // --- EDITANDO EXISTENTE ---
                {
                    var usuarioDB = await _context.Usuarios.FindAsync(usuario.Id);
                    if (usuarioDB == null) return NotFound();

                    // Actualizamos los datos
                    usuarioDB.Nombre = usuario.Nombre;
                    usuarioDB.Apellido = usuario.Apellido;
                    usuarioDB.Email = usuario.Email;
                    usuarioDB.Rol = usuario.Rol;

                    // Solo cambiamos la contraseña si el usuario escribió una nueva
                    if (!string.IsNullOrWhiteSpace(usuario.Password))
                    {
                        usuarioDB.Password = usuario.Password;
                    }

                    // EF Core detecta los cambios automáticamente al guardar
                    TempData["Mensaje"] = "Usuario actualizado correctamente.";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores de validación, volvemos a mostrar el formulario
            return View("UsuarioForm", usuario);
        }

        // GET: Usuario/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Usuario eliminado correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Usuario/Perfil
        public async Task<IActionResult> Perfil()
        {
            // 1. Obtenemos el email del usuario logueado desde la sesión
            var emailUsuario = HttpContext.Session.GetString("usuario");

            if (string.IsNullOrEmpty(emailUsuario))
            {
                // Si no hay sesión, mandar al login (ajusta "Cuenta" y "Login" si tus nombres son otros)
                return RedirectToAction("Login", "Cuenta");
            }

            // 2. Buscamos sus datos en la base de datos
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == emailUsuario);

            if (usuario == null) return NotFound();

            return View(usuario);
        }

        // POST: Usuario/ActualizarPerfil
        [HttpPost]
        public async Task<IActionResult> ActualizarPerfil(int id, string Nombre, string Apellido, string Email, string? NuevaPassword, string? ConfirmarPassword)
        {
            var usuarioDB = await _context.Usuarios.FindAsync(id);
            if (usuarioDB == null) return NotFound();

            // 1. Actualizamos datos personales
            usuarioDB.Nombre = Nombre;
            usuarioDB.Apellido = Apellido;
            usuarioDB.Email = Email; // Ojo: Si cambia el email, la próxima vez debe loguearse con el nuevo

            // 2. Lógica de cambio de contraseña (solo si escribió algo)
            if (!string.IsNullOrWhiteSpace(NuevaPassword))
            {
                if (NuevaPassword != ConfirmarPassword)
                {
                    TempData["Error"] = "Las contraseñas no coinciden.";
                    return RedirectToAction(nameof(Perfil));
                }
                usuarioDB.Password = NuevaPassword;
            }

            await _context.SaveChangesAsync();

            // Actualizamos la sesión por si cambió el correo
            HttpContext.Session.SetString("usuario", usuarioDB.Email);

            TempData["Mensaje"] = "Perfil actualizado correctamente.";
            return RedirectToAction(nameof(Perfil));
        }
    }
}