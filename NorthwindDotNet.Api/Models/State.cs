using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("States")]
public class State
{
    [Key]
    [Column("StateAbbrev")]
    [Required]
    [MaxLength(2)]
    public string StateAbbrev { get; set; } = string.Empty;

    [Column("StateName")]
    [MaxLength(50)]
    public string? StateName { get; set; }

    public ICollection<Company> Companies { get; set; } = [];
}
