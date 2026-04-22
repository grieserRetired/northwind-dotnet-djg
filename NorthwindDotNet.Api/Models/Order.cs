using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

// CustomerID and ShipperID both reference Companies.CompanyID (two separate
// FK relationships to the same table).  InverseProperty on Company.CustomerOrders
// and Company.ShipperOrders disambiguates the two ends.
[Table("Orders")]
public class Order
{
    [Key]
    [Column("OrderID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }

    [Column("EmployeeID")]
    public int? EmployeeId { get; set; }

    [Column("CustomerID")]
    public int? CustomerId { get; set; }

    [Column("OrderDate")]
    public DateTime? OrderDate { get; set; }

    [Column("InvoiceDate")]
    public DateTime? InvoiceDate { get; set; }

    [Column("ShippedDate")]
    public DateTime? ShippedDate { get; set; }

    [Column("ShipperID")]
    public int? ShipperId { get; set; }

    [Column("ShippingFee")]
    public decimal? ShippingFee { get; set; }

    // REAL in PostgreSQL maps to float (single-precision) in C#.
    [Column("TaxRate")]
    public float? TaxRate { get; set; }

    [Column("TaxStatusID")]
    public short? TaxStatusId { get; set; }

    [Column("PaymentMethod")]
    [MaxLength(50)]
    public string? PaymentMethod { get; set; }

    [Column("PaidDate")]
    public DateTime? PaidDate { get; set; }

    [Column("Notes")]
    public string? Notes { get; set; }

    [Column("OrderStatusID")]
    public int? OrderStatusId { get; set; }

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

    [ForeignKey("EmployeeId")]
    public Employee? Employee { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("CustomerOrders")]
    public Company? Customer { get; set; }

    [ForeignKey("ShipperId")]
    [InverseProperty("ShipperOrders")]
    public Company? Shipper { get; set; }

    [ForeignKey("TaxStatusId")]
    public TaxStatus? TaxStatus { get; set; }

    [ForeignKey("OrderStatusId")]
    public OrderStatus? OrderStatus { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = [];
}
