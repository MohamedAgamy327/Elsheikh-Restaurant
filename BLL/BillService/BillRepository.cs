using BLL.RepositoryService;
using DAL;
using DAL.ConstString;
using DAL.Entities;
using DTO.BillDataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.BillService
{
    public class BillRepository : GenericRepository<Bill>, IBillRepository
    {
        public BillRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.Bills.Where(w =>  (w.ID.ToString() + w.Type + w.Details + w.User.Name).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
        }

        public List<BillDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize)
        {

            return GeneralDBContext.Bills.Where(w => (w.ID.ToString() + w.Type + w.Details + w.User.Name).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderByDescending(o => o.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new BillDisplayDataModel
            {
                Bill = s,
                User = s.User
            }).ToList();

        }
    }
}
