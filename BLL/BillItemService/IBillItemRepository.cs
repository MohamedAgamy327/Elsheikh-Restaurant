using BLL.RepositoryService;
using DAL.Entities;
using DTO.BillItemDataModel;
using System;
using System.Collections.Generic;

namespace BLL.BillItemService
{
    public interface IBillItemRepository : IGenericRepository<BillItem>
    {
        List<BillsCategoriesDataModel> GetBillsCategories(int categoryID,DateTime dt);
        List<BillsCategoriesDataModel> GetBillsCategories(int categoryID, DateTime dtFrom,DateTime dtTo);

    }
}
