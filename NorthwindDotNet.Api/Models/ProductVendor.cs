using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("ProductVendors")]
public class ProductVendor
{
    [Key]
    [Column("ProductVendorID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductVendorId { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }

    // VendorID references Companies.CompanyID per relation New_New_CompaniesProductVendors.
    [Column("VendorID")]
    public int? VendorId { get; set; }

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

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    [ForeignKey("VendorId")]
    public Company? Vendor { get; set; }
}
