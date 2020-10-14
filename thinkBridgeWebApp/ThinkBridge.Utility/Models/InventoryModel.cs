using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ThinkBridge.UtilityAndModels.Models
{
    public class InventoryModel
    {
        public long InventoryID { get; set; }
        [DisplayName("Product Name")]
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string ProductName { get; set; }


        [DisplayName("Product Description")]
        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string ProductDescription { get; set; }

        [DisplayName("Product Price")]
        [Required]
        public long ProductPrice { get; set; }

        [DisplayName("Product Photo")]
        public byte[] ProductPhoto { get; set; }

        public bool IsActive { get; set; }
        public string  FileExtension { get; set; }
        public string  FileBase64 { get; set; }
    }
}
