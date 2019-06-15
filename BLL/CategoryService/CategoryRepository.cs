using System.Collections.Generic;
using System.Linq;
using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using System.Data.Entity;
using DTO.CategoryDataModel;

namespace BLL.ItemService
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key)
        {
            return GeneralDBContext.Categories.Where(s => s.Name.Contains(key)).Count();
        }

        public List<CategoryDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Categories.Where(w => (w.Name).Contains(key)).OrderBy(t => t.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new CategoryDisplayDataModel
            {
                Category = s,
                Count=s.Items.Count,
                CanDelete = s.Items.Count > 0 ? false : true
            }).ToList(); ;
        }
    }
}
