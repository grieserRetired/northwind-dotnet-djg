using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("Titles")]
public class Title
{
    // Property renamed to avoid CS0542 (member name same as enclosing type).
    [Key]
    [Column("Title")]
    [Required]
    [MaxLength(20)]
    public string TitleValue { get; set; } = string.Empty;

    [Column("AddedBy")]
    [MaxLength(255)]
    public string? AddedBy { get; set; }

    [Column("AddedOn")]
    public DateTime? AddedOn { get; set; }

    [Column("ModifiedBy")]
    [MaxLength(255)]
    public string? ModifiedBy { get; set; }

    [Column("ModifiedOn")]
    public DateTime? ModifiedOn { get; set; }

    [InverseProperty("TitleNavigation")]
    public ICollection<Employee> Employees { get; set; } = [];
}
