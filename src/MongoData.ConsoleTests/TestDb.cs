using MongoData.Domain;
using MongoData.Repository;

namespace MongoData.ConsoleTests
{
    public class TestDb
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public TestDb(IUnitOfWork unitOfWork,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public void Run()
        {
            Setup();
            Test();
            Cleanup();
        }

        private void Setup()
        {
            DropDb();
        }

        private void Cleanup()
        {
            DropDb();
        }

        private void DropDb()
        {
            _unitOfWork.Client.DropDatabase(_unitOfWork.Database.DatabaseNamespace.DatabaseName);
        }

        private void Test()
        {
            var product = new Product
            {
                Name = "test",
                Type = ProductTypes.Product
            };
            product = _productRepository.Add(product);

            var count = _productRepository.Count();
            var product1 = _productRepository.GetById(product.Id);

            product.Name = "test_1";
            product = _productRepository.Update(product);

            var product2 = _productRepository.GetById(product.Id);
        }
    }
}
