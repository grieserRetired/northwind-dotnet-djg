using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("Learn")]
public class Learn
{
    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("SectionNo")]
    public short? SectionNo { get; set; }

    [Column("SectionText")]
    public string? SectionText { get; set; }
}
