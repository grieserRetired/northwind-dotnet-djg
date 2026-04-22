using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("SystemSettings")]
public class SystemSetting
{
    [Key]
    [Column("SettingID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SettingId { get; set; }

    [Column("SettingName")]
    [MaxLength(50)]
    public string? SettingName { get; set; }

    [Column("SettingValue")]
    [MaxLength(255)]
    public string? SettingValue { get; set; }

    [Column("Notes")]
    [MaxLength(255)]
    public string? Notes { get; set; }
}
