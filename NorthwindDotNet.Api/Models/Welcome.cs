using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("Welcome")]
public class Welcome
{
    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Properties renamed to avoid CS0542 (member name same as enclosing type).
    [Column("Welcome")]
    public string? WelcomeText { get; set; }

    [Column("Learn")]
    public string? LearnText { get; set; }

    [Column("DataMacro")]
    public string? DataMacro { get; set; }
}
