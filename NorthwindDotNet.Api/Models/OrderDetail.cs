using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("OrderDetails")]
public class OrderDetail
{
    [Key]
    [Column("OrderDetailID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderDetailId { get; set; }

    [Column("OrderID")]
    public int? OrderId { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }

    [Column("Quantity")]
    public short? Quantity { get; set; }

    [Column("UnitPrice")]
    public decimal? UnitPrice { get; set; }

    // REAL in PostgreSQL maps to float (single-precision) in C#.
    [Column("Discount")]
    public float? Discount { get; set; }

    [Column("OrderDetailStatusID")]
    public int? OrderDetailStatusId { get; set; }

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

    [ForeignKey("OrderId")]
    public Order? Order { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    [ForeignKey("OrderDetailStatusId")]
    public OrderDetailStatus? OrderDetailStatus { get; set; }
}
