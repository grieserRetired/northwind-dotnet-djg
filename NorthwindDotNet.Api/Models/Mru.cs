using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("MRU")]
public class Mru
{
    [Key]
    [Column("MRU_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MruId { get; set; }

    [Column("EmployeeID")]
    public int? EmployeeId { get; set; }

    [Column("TableName")]
    [MaxLength(50)]
    public string? TableName { get; set; }

    [Column("PKValue")]
    public int? PkValue { get; set; }

    [Column("DateAdded")]
    public DateTime? DateAdded { get; set; }

    [ForeignKey("EmployeeId")]
    public Employee? Employee { get; set; }
}
