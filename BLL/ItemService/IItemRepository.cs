using BLL.RepositoryService;
using DAL.Entities;
using DTO.ItemDataModel;
using System.Collections.Generic;

namespace BLL.ItemService
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        int GetRecordsNumber(string key);
        int GetRecordsNumber(int categoryID);
        List<ItemDisplayDataModel> Search(string key, int pageNumber, int pageSize);
        List<ItemOrderDataModel> Search(int categoryID);
    }
}
