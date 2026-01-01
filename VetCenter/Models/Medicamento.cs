using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VetCenter.Models
{
    [Table("medicamento")]
    public class Medicamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string NombreDelMedicamento { get; set; }

        [Required]
        [StringLength(100)]
        public required string Descripcion { get; set; }

        [Required]
        [StringLength(100)]
        public string? DosisRecomendada { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaDeVencimiento { get; set; }

        [Required]
        [StringLength(10)]
        public string? Stock { get; set; }
    }
}
