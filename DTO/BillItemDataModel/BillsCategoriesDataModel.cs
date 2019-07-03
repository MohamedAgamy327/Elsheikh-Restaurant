using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.BillItemDataModel
{
    public class BillsCategoriesDataModel :  ValidatableBindableBase
    {
        private Item _item;
        public Item Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set { SetProperty(ref _checked, value); }
        }

        private decimal? _qty;
        public decimal? Qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }

        private decimal? _total;
        public decimal? Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }

    }
}
