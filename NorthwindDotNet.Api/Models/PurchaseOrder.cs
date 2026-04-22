using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

// VendorID references Companies.CompanyID (relation New_New_CompaniesPurchaseOrders).
// SubmittedByID and ApprovedByID are two separate FKs to Employees; InverseProperty
// on Employee.SubmittedPurchaseOrders / ApprovedPurchaseOrders disambiguates them.
[Table("PurchaseOrders")]
public class PurchaseOrder
{
    [Key]
    [Column("PurchaseOrderID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PurchaseOrderId { get; set; }

    [Column("VendorID")]
    public int? VendorId { get; set; }

    [Column("SubmittedByID")]
    public int? SubmittedById { get; set; }

    [Column("SubmittedDate")]
    public DateTime? SubmittedDate { get; set; }

    [Column("ApprovedByID")]
    public int? ApprovedById { get; set; }

    [Column("ApprovedDate")]
    public DateTime? ApprovedDate { get; set; }

    [Column("StatusID")]
    public int? StatusId { get; set; }

    [Column("ReceivedDate")]
    public DateTime? ReceivedDate { get; set; }

    [Column("ShippingFee")]
    public decimal? ShippingFee { get; set; }

    [Column("TaxAmount")]
    public decimal? TaxAmount { get; set; }

    [Column("PaymentDate")]
    public DateTime? PaymentDate { get; set; }

    [Column("PaymentAmount")]
    public decimal? PaymentAmount { get; set; }

    [Column("PaymentMethod")]
    [MaxLength(50)]
    public string? PaymentMethod { get; set; }

    [Column("Notes")]
    public string? Notes { get; set; }

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

    [ForeignKey("VendorId")]
    [InverseProperty("PurchaseOrders")]
    public Company? Vendor { get; set; }

    [ForeignKey("SubmittedById")]
    [InverseProperty("SubmittedPurchaseOrders")]
    public Employee? SubmittedBy { get; set; }

    [ForeignKey("ApprovedById")]
    [InverseProperty("ApprovedPurchaseOrders")]
    public Employee? ApprovedBy { get; set; }

    [ForeignKey("StatusId")]
    public PurchaseOrderStatus? Status { get; set; }

    public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = [];
}
