using System.ComponentModel.DataAnnotations;

namespace CompanyProfileMVC.Areas.Admin.Models
{
    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage = "Nama kategori wajib diisi")]
        public string Name { get; set; } = "";
    }
}
