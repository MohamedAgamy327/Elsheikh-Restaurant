using DAL.BindableBaseService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Item : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int? _categoryId;
        public int? CategoryID
        {
            get { return _categoryId; }
            set { SetProperty(ref _categoryId, value); }
        }

        private int? _order;
        public int? Order
        {
            get { return _order; }
            set { SetProperty(ref _order, value); }
        }

        private bool _isAvailable;
        public bool IsAvailable
        {
            get { return _isAvailable; }
            set { SetProperty(ref _isAvailable, value); }
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

        public virtual Category Category { get; set; }

        public virtual ICollection<BillItem> BillsItems { get; set; }
    }
}
