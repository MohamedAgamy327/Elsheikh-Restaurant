using BLL.RepositoryService;
using DAL.Entities;
using DTO.BillDataModel;
using System;
using System.Collections.Generic;

namespace BLL.BillService
{
    public interface IBillRepository : IGenericRepository<Bill>
    {
        int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo);
        List<BillDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize);
    }
}
