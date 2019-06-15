using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.CategoryDataModel
{
    public class CategoryUpdateDataModel : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
    }
}
