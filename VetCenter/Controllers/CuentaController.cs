using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Necesario para FirstOrDefaultAsync
using VetCenter.Data;
using VetCenter.Models;

namespace VetCenter.Controllers
{
    public class CuentaController : Controller
    {
        private readonly AppDbContext _context;

        public CuentaController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password) // Convertido a async Task
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Por favor ingrese correo y contraseña.";
                return View();
            }

            // Busqueda con comparación simple (tal como la solicitaste)
            var usuario = await _context.Usuarios
                                        .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (usuario != null)
            {
                // Mantenemos tu sesión original y guardamos los campos
                HttpContext.Session.SetInt32("id", usuario.Id);
                HttpContext.Session.SetString("usuario", usuario.Email);
                HttpContext.Session.SetString("nombre", usuario.Nombre);

                // --- CAMBIO APLICADO: 'Roles' corregido a 'Rol' (singular) ---
                HttpContext.Session.SetString("rol", usuario.Rol);
                // -----------------------------------------------------------

                return RedirectToAction("Index", "Panel");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}