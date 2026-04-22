using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("Products")]
public class Product
{
    [Key]
    [Column("ProductID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductId { get; set; }

    [Column("ProductCode")]
    [MaxLength(20)]
    public string? ProductCode { get; set; }

    [Column("ProductName")]
    [MaxLength(50)]
    public string? ProductName { get; set; }

    [Column("ProductDescription")]
    public string? ProductDescription { get; set; }

    [Column("StandardUnitCost")]
    public decimal? StandardUnitCost { get; set; }

    [Column("UnitPrice")]
    public decimal? UnitPrice { get; set; }

    [Column("ReorderLevel")]
    public short? ReorderLevel { get; set; }

    [Column("TargetLevel")]
    public short? TargetLevel { get; set; }

    [Column("QuantityPerUnit")]
    [MaxLength(50)]
    public string? QuantityPerUnit { get; set; }

    [Column("Discontinued")]
    public bool? Discontinued { get; set; }

    [Column("MinimumReorderQuantity")]
    public short? MinimumReorderQuantity { get; set; }

    [Column("ProductCategoryID")]
    public int? ProductCategoryId { get; set; }

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

    [ForeignKey("ProductCategoryId")]
    public ProductCategory? ProductCategory { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = [];

    public ICollection<ProductVendor> ProductVendors { get; set; } = [];

    public ICollection<StockTake> StockTakes { get; set; } = [];

    public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = [];
}
