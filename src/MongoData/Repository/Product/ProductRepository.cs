using MongoData.Domain;

namespace MongoData.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}