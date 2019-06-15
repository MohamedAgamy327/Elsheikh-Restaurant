using BLL.BillItemService;
using BLL.BillService;
using BLL.ItemService;
using BLL.RoleService;
using BLL.SafeService;
using BLL.ShiftService;
using BLL.SpendingService;
using BLL.UserService;
using DAL;

namespace BLL.UnitOfWorkService
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GeneralDBContext _context;

        private IUserRepository users;
        private IBillRepository bills;
        private IBillItemRepository billsItems;
        private IItemRepository items;
        private IRoleRepository roles;
        private ISafeRepository safes;
        private IShiftRepository shifts;
        private ISpendingRepository spendings;
        private ICategoryRepository categories;

        public UnitOfWork(GeneralDBContext context)
        {
            _context = context;
        }

        public IUserRepository Users
        {
            get
            {
                if (users == null)
                {
                    users = new UserRepository(_context);
                }
                return users;
            }
        }

        public IBillRepository Bills
        {
            get
            {
                if (bills == null)
                {
                    bills = new BillRepository(_context);
                }
                return bills;
            }
        }

        public IBillItemRepository BillsItems
        {
            get
            {
                if (billsItems == null)
                {
                    billsItems = new BillItemRepository(_context);
                }
                return billsItems;
            }
        }

        public IItemRepository Items
        {
            get
            {
                if (items == null)
                {
                    items = new ItemRepository(_context);
                }
                return items;
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                if (roles == null)
                {
                    roles = new RoleRepository(_context);
                }
                return roles;
            }
        }

        public ISafeRepository Safes
        {
            get
            {
                if (safes == null)
                {
                    safes = new SafeRepository(_context);
                }
                return safes;
            }
        }

        public IShiftRepository Shifts
        {
            get
            {
                if (shifts == null)
                {
                    shifts = new ShiftRepository(_context);
                }
                return shifts;
            }
        }

        public ISpendingRepository Spendings
        {
            get
            {
                if (spendings == null)
                {
                    spendings = new SpendingRepository(_context);
                }
                return spendings;
            }
        }

        public ICategoryRepository Categories
        {
            get
            {
                if (categories == null)
                {
                    categories = new CategoryRepository(_context);
                }
                return categories;
            }
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
