using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext DBcontext;
        public EFProductRepository(ApplicationDbContext context)
        {
            DBcontext = context;
        }
        public IQueryable<Product> Products => DBcontext.Products;
    }
}
