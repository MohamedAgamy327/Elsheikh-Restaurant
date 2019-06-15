using BLL.RepositoryService;
using DAL.Entities;
using DTO.CategoryDataModel;
using System.Collections.Generic;

namespace BLL.ItemService
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        int GetRecordsNumber(string key);

        List<CategoryDisplayDataModel> Search(string key, int pageNumber, int pageSize);
    }
}
