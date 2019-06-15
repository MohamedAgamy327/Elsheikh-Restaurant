using DAL.BindableBaseService;
using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace DTO.BillItemDataModel
{
    public class BillItemDisplayDataModel : ValidatableBindableBase
    {
        private int _itemID;
        [Required]
        public int ItemID
        {
            get { return _itemID; }
            set { SetProperty(ref _itemID, value); }
        }

        private decimal? _qty;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? Qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }

        private decimal? _total;
        [Required]
        [Range(0.1, double.MaxValue)]
        public decimal? Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }

        private Item _item;
        public Item Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

    }
}
