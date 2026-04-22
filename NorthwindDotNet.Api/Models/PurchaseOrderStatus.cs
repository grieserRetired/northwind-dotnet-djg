using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("PurchaseOrderStatus")]
public class PurchaseOrderStatus
{
    [Key]
    [Column("StatusID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StatusId { get; set; }

    [Column("StatusName")]
    [MaxLength(50)]
    public string? StatusName { get; set; }

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

    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = [];
}
