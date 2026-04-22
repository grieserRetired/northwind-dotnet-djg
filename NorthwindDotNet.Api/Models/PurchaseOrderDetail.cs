using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("PurchaseOrderDetails")]
public class PurchaseOrderDetail
{
    [Key]
    [Column("PurchaseOrderDetailID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PurchaseOrderDetailId { get; set; }

    [Column("PurchaseOrderID")]
    public int? PurchaseOrderId { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }

    [Column("Quantity")]
    public short? Quantity { get; set; }

    [Column("UnitCost")]
    public decimal? UnitCost { get; set; }

    [Column("ReceivedDate")]
    public DateTime? ReceivedDate { get; set; }

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

    [ForeignKey("PurchaseOrderId")]
    public PurchaseOrder? PurchaseOrder { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
}
