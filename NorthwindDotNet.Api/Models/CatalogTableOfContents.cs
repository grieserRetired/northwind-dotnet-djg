using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("Catalog_TableOfContents")]
public class CatalogTableOfContents
{
    [Key]
    [Column("TocTitle")]
    [Required]
    [MaxLength(255)]
    public string TocTitle { get; set; } = string.Empty;

    [Column("TocPage")]
    public short? TocPage { get; set; }
}
