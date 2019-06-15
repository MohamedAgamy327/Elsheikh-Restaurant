using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.CategoryDataModel
{
    public class CategoryAddDataModel : ValidatableBindableBase
    {
        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

    }
}
