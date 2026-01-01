using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetCenter.Models
{
    [Table("mascotas")]
    public class Mascota
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Raza { get; set; }
        public int Edad { get; set; }
        public bool Estado { get; set; }

        // Clave foránea (Buenas prácticas en EF Core)
        [Column("cliente_id")] // <--- AGREGA ESTA LÍNEA (Mira tu script SQL, se llama cliente_id)
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
