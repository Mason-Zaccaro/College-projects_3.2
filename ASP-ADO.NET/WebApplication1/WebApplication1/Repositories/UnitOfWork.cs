using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly Repository<Product> _productRepository = new();
        private static readonly Repository<Category> _categoryRepository = new();

        public IRepository<Product> Products => _productRepository;
        public IRepository<Category> Categories => _categoryRepository;
    }
}