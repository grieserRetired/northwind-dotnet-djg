using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("NorthwindFeatures")]
public class NorthwindFeature
{
    [Key]
    [Column("NorthwindFeaturesID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NorthwindFeaturesId { get; set; }

    [Column("ItemName")]
    [MaxLength(255)]
    public string? ItemName { get; set; }

    [Column("Description")]
    [MaxLength(255)]
    public string? Description { get; set; }

    [Column("Navigation")]
    [MaxLength(255)]
    public string? Navigation { get; set; }

    [Column("LearnMore")]
    public string? LearnMore { get; set; }

    [Column("HelpKeywords")]
    [MaxLength(255)]
    public string? HelpKeywords { get; set; }

    [Column("OpenMethod")]
    public int? OpenMethod { get; set; }
}
