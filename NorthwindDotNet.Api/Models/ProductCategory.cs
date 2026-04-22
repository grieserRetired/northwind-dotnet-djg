using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindDotNet.Api.Models;

[Table("ProductCategories")]
public class ProductCategory
{
    [Key]
    [Column("ProductCategoryID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductCategoryId { get; set; }

    [Column("ProductCategoryName")]
    [MaxLength(255)]
    public string? ProductCategoryName { get; set; }

    [Column("ProductCategoryCode")]
    [MaxLength(3)]
    public string? ProductCategoryCode { get; set; }

    [Column("ProductCategoryDesc")]
    [MaxLength(255)]
    public string? ProductCategoryDesc { get; set; }

    [Column("ProductCategoryImage")]
    public string? ProductCategoryImage { get; set; }

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

    public ICollection<Product> Products { get; set; } = [];
}
