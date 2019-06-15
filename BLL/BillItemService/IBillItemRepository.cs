using BLL.RepositoryService;
using DAL.Entities;
using DTO.BillItemDataModel;
using System.Collections.Generic;

namespace BLL.BillItemService
{
    public interface IBillItemRepository : IGenericRepository<BillItem>
    {
        List<BillItemDisplayDataModel> GetShiftItems();
        List<BillItemDisplayDataModel> GetBillItems(int billID);
    }
}
