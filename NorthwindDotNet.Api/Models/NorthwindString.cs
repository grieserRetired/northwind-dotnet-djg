using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

// Class renamed NorthwindString to avoid conflict with the System.String type name.
[Table("Strings")]
public class NorthwindString
{
    [Key]
    [Column("StringID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StringId { get; set; }

    [Column("StringData")]
    public string? StringData { get; set; }

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
}
