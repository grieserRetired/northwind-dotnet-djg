using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

// TaxStatusID uses SMALLINT (not SERIAL) because IDs 0 and 1 are fixed enum-like
// values (0=Tax Exempt, 1=Taxable) keyed to enumTaxStatus in the Access VBA code.
[Table("TaxStatus")]
public class TaxStatus
{
    [Key]
    [Column("TaxStatusID")]
    public short TaxStatusId { get; set; }

    // Property renamed to avoid CS0542 (member name same as enclosing type).
    [Column("TaxStatus")]
    [MaxLength(50)]
    public string? TaxStatusName { get; set; }

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

    [InverseProperty("StandardTaxStatus")]
    public ICollection<Company> Companies { get; set; } = [];

    [InverseProperty("TaxStatus")]
    public ICollection<Order> Orders { get; set; } = [];
}
