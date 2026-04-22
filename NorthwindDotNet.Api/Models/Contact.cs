using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("Contacts")]
public class Contact
{
    [Key]
    [Column("ContactID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContactId { get; set; }

    [Column("CompanyID")]
    public int? CompanyId { get; set; }

    [Column("LastName")]
    [MaxLength(30)]
    public string? LastName { get; set; }

    [Column("FirstName")]
    [MaxLength(20)]
    public string? FirstName { get; set; }

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

    [Column("Notes")]
    public string? Notes { get; set; }

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

    [ForeignKey("CompanyId")]
    public Company? Company { get; set; }
}
