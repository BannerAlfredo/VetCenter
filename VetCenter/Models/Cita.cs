using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetCenter.Models
{
    [Table("cita")]
    public class Cita
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan Hora { get; set; }

        [StringLength(100)]
        public string? Motivo { get; set; }

        public bool Estado { get; set; } = true;

        // Relación con Mascota
        [Column("mascota_id")]
        public int MascotaId { get; set; }

        [ForeignKey("MascotaId")]
        public virtual Mascota? Mascota { get; set; }

        // Cliente se obtiene desde Mascota.Cliente
    }
}
