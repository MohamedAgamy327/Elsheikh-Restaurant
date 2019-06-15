using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.ItemDataModel
{
    public class ItemOrderDataModel : ValidatableBindableBase
    {
        private bool _isLast;
        public bool IsLast
        {
            get { return _isLast; }
            set { SetProperty(ref _isLast, value); }
        }

        private Category _category;
        public Category Category
        {
            get { return _category; }
            set { SetProperty(ref _category, value); }
        }

        private Item _item;
        public Item Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }
    }
}
