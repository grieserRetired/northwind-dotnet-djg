using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("CompanyTypes")]
public class CompanyType
{
    [Key]
    [Column("CompanyTypeID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CompanyTypeId { get; set; }

    // Property renamed to avoid CS0542 (member name same as enclosing type).
    [Column("CompanyType")]
    [MaxLength(50)]
    public string? CompanyTypeName { get; set; }

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

    public ICollection<Company> Companies { get; set; } = [];
}
