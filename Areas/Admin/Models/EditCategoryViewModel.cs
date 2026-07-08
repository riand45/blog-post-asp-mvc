using System.ComponentModel.DataAnnotations;

namespace CompanyProfileMVC.Areas.Admin.Models
{
    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nama kategori wajib diisi")]
        public string Name { get; set; } = "";
    }
}
