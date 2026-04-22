using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("OrderStatus")]
public class OrderStatus
{
    [Key]
    [Column("OrderStatusID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderStatusId { get; set; }

    [Column("OrderStatusCode")]
    [MaxLength(5)]
    public string? OrderStatusCode { get; set; }

    [Column("OrderStatusName")]
    [MaxLength(50)]
    public string? OrderStatusName { get; set; }

    [Column("SortOrder")]
    public short? SortOrder { get; set; }

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

    public ICollection<Order> Orders { get; set; } = [];
}
