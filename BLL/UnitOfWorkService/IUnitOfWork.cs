using BLL.BillItemService;
using BLL.BillService;
using BLL.ItemService;
using BLL.RoleService;
using BLL.SafeService;
using BLL.ShiftService;
using BLL.SpendingService;
using BLL.UserService;
using System;

namespace BLL.UnitOfWorkService
{
    public interface IUnitOfWork : IDisposable
    {
        IBillRepository Bills { get;  }
        IBillItemRepository BillsItems { get; }
        IItemRepository Items { get; }
        IRoleRepository Roles { get; }
        ISafeRepository Safes { get; }
        IShiftRepository Shifts { get; }
        ISpendingRepository Spendings { get; }
        IUserRepository Users { get; }
        ICategoryRepository Categories { get; }
        int Complete();
    }
}
