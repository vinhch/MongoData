using MongoData.Domain;

namespace MongoData.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}