using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WingtipToys.Models
{

  [Table("Products")]
  public class Product
  {
    [ScaffoldColumn(false)]
    public int ProductID { get; set; }

    [Required, StringLength(100), Display(Name = "Name")]
    public string ProductName { get; set; }

    [Required, StringLength(10000), Display(Name = "Product Description"), DataType(DataType.MultilineText)]
    public string Description { get; set; }

    [Display(Name = "Image")]
    public string ImagePath { get; set; }

    [Display(Name = "Price"), DisplayFormat(DataFormatString ="{0:C}")]
    public double? UnitPrice { get; set; }

    public int? CategoryID { get; set; }

    public virtual Category Category { get; set; }
  }
}