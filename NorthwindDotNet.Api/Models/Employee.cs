using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("Employees")]
public class Employee
{
    [Key]
    [Column("EmployeeID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeId { get; set; }

    [Column("FirstName")]
    [MaxLength(20)]
    public string? FirstName { get; set; }

    [Column("LastName")]
    [MaxLength(30)]
    public string? LastName { get; set; }

    [Column("EmailAddress")]
    [MaxLength(255)]
    public string? EmailAddress { get; set; }

    [Column("JobTitle")]
    [MaxLength(50)]
    public string? JobTitle { get; set; }

    [Column("PrimaryPhone")]
    [MaxLength(20)]
    public string? PrimaryPhone { get; set; }

    [Column("SecondaryPhone")]
    [MaxLength(20)]
    public string? SecondaryPhone { get; set; }

    // FK to Titles.Title (string FK); navigation named TitleNavigation to
    // avoid ambiguity between the scalar column property and the nav property.
    [Column("Title")]
    [MaxLength(20)]
    public string? Title { get; set; }

    [Column("Notes")]
    public string? Notes { get; set; }

    [Column("Attachments")]
    public string? Attachments { get; set; }

    [Column("SupervisorID")]
    public int? SupervisorId { get; set; }

    [Column("WindowsUserName")]
    [MaxLength(50)]
    public string? WindowsUserName { get; set; }

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

    [ForeignKey("Title")]
    public Title? TitleNavigation { get; set; }

    [ForeignKey("SupervisorId")]
    public Employee? Supervisor { get; set; }

    [InverseProperty("Supervisor")]
    public ICollection<Employee> Subordinates { get; set; } = [];

    public ICollection<EmployeePrivilege> EmployeePrivileges { get; set; } = [];

    public ICollection<Mru> MruEntries { get; set; } = [];

    public ICollection<Order> Orders { get; set; } = [];

    [InverseProperty("SubmittedBy")]
    public ICollection<PurchaseOrder> SubmittedPurchaseOrders { get; set; } = [];

    [InverseProperty("ApprovedBy")]
    public ICollection<PurchaseOrder> ApprovedPurchaseOrders { get; set; } = [];
}
