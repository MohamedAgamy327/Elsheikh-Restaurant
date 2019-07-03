using System;
using System.Collections.Generic;
using System.Linq;
using BLL.RepositoryService;
using DAL;
using DAL.ConstString;
using DAL.Entities;
using DTO.BillItemDataModel;

namespace BLL.BillItemService
{
    public class BillItemRepository : GenericRepository<BillItem>, IBillItemRepository
    {
        public BillItemRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public List<BillsCategoriesDataModel> GetBillsCategories(int categoryID,DateTime dt)
        {
            return GeneralDBContext.BillsItems.Where(w => w.Item.CategoryID == categoryID && dt <= w.Bill.RegistrationDate).GroupBy(l => l.ItemID).
                  Select(s => new BillsCategoriesDataModel
                  {
                      Item = s.FirstOrDefault().Item,
                      Qty = s.Sum(j => j.Qty),
                      Total = s.Sum(p => p.Total)
                  }).ToList();
        }

        public List<BillsCategoriesDataModel> GetBillsCategories(int categoryID, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.BillsItems.Where(w => w.Item.CategoryID == categoryID && w.Bill.RegistrationDate >= dtFrom && w.Bill.RegistrationDate <= dtTo).GroupBy(l => l.ItemID).
                  Select(s => new BillsCategoriesDataModel
                  {
                      Item = s.FirstOrDefault().Item,
                      Qty = s.Sum(j => j.Qty),
                      Total = s.Sum(p => p.Total)
                  }).ToList();
        }
    }
}
