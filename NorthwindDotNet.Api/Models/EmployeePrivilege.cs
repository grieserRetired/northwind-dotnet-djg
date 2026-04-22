using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("EmployeePrivileges")]
public class EmployeePrivilege
{
    [Key]
    [Column("EmployeePrivilegeID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeePrivilegeId { get; set; }

    [Column("EmployeeID")]
    public int? EmployeeId { get; set; }

    [Column("PrivilegeID")]
    public int? PrivilegeId { get; set; }

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

    [ForeignKey("EmployeeId")]
    public Employee? Employee { get; set; }

    [ForeignKey("PrivilegeId")]
    public Privilege? Privilege { get; set; }
}
