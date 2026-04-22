using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

// Note: the composite unique constraint UQ_Companies_CustomerName (CompanyName,
// CompanyTypeID) cannot be expressed via data annotations and must be configured
// in OnModelCreating with HasAlternateKey / HasIndex(...).IsUnique().
[Table("Companies")]
public class Company
{
    [Key]
    [Column("CompanyID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CompanyId { get; set; }

    [Column("CompanyName")]
    [MaxLength(50)]
    public string? CompanyName { get; set; }

    [Column("CompanyTypeID")]
    public int? CompanyTypeId { get; set; }

    [Column("BusinessPhone")]
    [MaxLength(20)]
    public string? BusinessPhone { get; set; }

    [Column("Address")]
    [MaxLength(255)]
    public string? Address { get; set; }

    [Column("City")]
    [MaxLength(255)]
    public string? City { get; set; }

    [Column("StateAbbrev")]
    [MaxLength(2)]
    public string? StateAbbrev { get; set; }

    [Column("Zip")]
    [MaxLength(10)]
    public string? Zip { get; set; }

    [Column("Website")]
    public string? Website { get; set; }

    [Column("Notes")]
    public string? Notes { get; set; }

    [Column("StandardTaxStatusID")]
    public short? StandardTaxStatusId { get; set; }

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

    [ForeignKey("CompanyTypeId")]
    public CompanyType? CompanyType { get; set; }

    [ForeignKey("StateAbbrev")]
    public State? State { get; set; }

    [ForeignKey("StandardTaxStatusId")]
    public TaxStatus? StandardTaxStatus { get; set; }

    public ICollection<Contact> Contacts { get; set; } = [];

    [InverseProperty("Customer")]
    public ICollection<Order> CustomerOrders { get; set; } = [];

    [InverseProperty("Shipper")]
    public ICollection<Order> ShipperOrders { get; set; } = [];

    [InverseProperty("Vendor")]
    public ICollection<ProductVendor> ProductVendors { get; set; } = [];

    [InverseProperty("Vendor")]
    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = [];
}
