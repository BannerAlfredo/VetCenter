using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetCenter.Models
{
    [Table("clientes")] // Mismo nombre que en tu SQL
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public required string Apellido { get; set; }

        [StringLength(100)]
        public string? Direccion { get; set; }

        [StringLength(100)]
        public string? Correo { get; set; }

        [StringLength(15)]
        public string? Telefono { get; set; }

        // Relación: Un cliente tiene muchas mascotas
        public virtual ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
    }
}
