using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("USysRibbons")]
public class USysRibbon
{
    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("RibbonName")]
    [MaxLength(255)]
    public string? RibbonName { get; set; }

    [Column("RibbonXML")]
    public string? RibbonXml { get; set; }
}
