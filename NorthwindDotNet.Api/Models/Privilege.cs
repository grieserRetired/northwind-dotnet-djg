using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("Privileges")]
public class Privilege
{
    [Key]
    [Column("PrivilegeID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PrivilegeId { get; set; }

    [Column("PrivilegeName")]
    [MaxLength(50)]
    public string? PrivilegeName { get; set; }

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

    public ICollection<EmployeePrivilege> EmployeePrivileges { get; set; } = [];
}
