using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.ContactInfo {
  [Table("Contact")]
  public class Contact
  {
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString(); // UID

    [Column(TypeName = "nvarchar")]
    [StringLength(50)]
    [Required(ErrorMessage = "Need to input {0}")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } //

    [StringLength(100)]
    [Required(ErrorMessage = "Need to input {0}")]
    [EmailAddress(ErrorMessage = "It must be {0}")]
    public string Email { get; set; } //

    public DateTime DateSent { get; set; } //
    
    [Display(Name = "Message to send")]
    public string Message { get; set; }
    
    [StringLength(50)]
    [Phone(ErrorMessage = "It must be {0}")]
    public string Phone { get; set; } 

  }
}