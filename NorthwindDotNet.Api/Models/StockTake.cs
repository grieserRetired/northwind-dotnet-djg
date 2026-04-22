using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("StockTake")]
public class StockTake
{
    [Key]
    [Column("StockTakeID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StockTakeId { get; set; }

    [Column("StockTakeDate")]
    public DateTime? StockTakeDate { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }

    [Column("QuantityOnHand")]
    public short? QuantityOnHand { get; set; }

    [Column("ExpectedQuantity")]
    public int? ExpectedQuantity { get; set; }

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
}
