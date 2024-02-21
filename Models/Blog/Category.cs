using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.Blog {
  [Table("Category")]
  public class Category
  {

      [Key]
      public string Id { get; set; }  // UID

      // Category cha (FKey)
      [Display(Name = "Danh mục cha")]
      public string? ParentCategoryId { get; set; }

      // Tiều đề Category
      [Required(ErrorMessage = "Phải có tên danh mục")]
      [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
      [Display(Name = "Tên danh mục")]
      public required string Title { get; set; }

      // Nội dung, thông tin chi tiết về Category
      [DataType(DataType.Text)]
      [Display(Name = "Nội dung danh mục")]
      public required string Description { set; get; }

      //chuỗi Url
      [Required(ErrorMessage = "Phải tạo url")]
      [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
      [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
      [Display(Name = "Url hiện thị")]
      public required string Slug { set; get; }

      // Các Category con
      public ICollection<Category>? CategoryChildren { get; set; }

      [ForeignKey("ParentCategoryId")]
      [Display(Name = "Danh mục cha")]
      public Category? ParentCategory { set; get; }
  }
}