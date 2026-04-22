using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("OrderDetailStatus")]
public class OrderDetailStatus
{
    [Key]
    [Column("OrderDetailStatusID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderDetailStatusId { get; set; }

    [Column("OrderDetailStatusName")]
    [MaxLength(50)]
    public string? OrderDetailStatusName { get; set; }

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

    public ICollection<OrderDetail> OrderDetails { get; set; } = [];
}
