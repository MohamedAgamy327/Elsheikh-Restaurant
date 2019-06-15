using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.ItemDataModel
{
    public class ItemAddDataModel : ValidatableBindableBase
    {
        private int _categoryID;
        [Required]
        public int CategoryID
        {
            get { return _categoryID; }
            set { SetProperty(ref _categoryID, value); }
        }

        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private decimal? _price;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }
    }
}
