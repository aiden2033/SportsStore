using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using System.Linq;
using SportsStore.Models;
using SportsStore.Controllers;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void CanPaginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>(); //мок у нас пародирует объект, реализующий IProductRepository
            mock.Setup(m => m.Products) //указываем какой тип мок будет возвращать
                .Returns( //указываем, конкретный массив объектов для возврата
                (new Product[]
                {
                new Product(){ProductID=1,Name="P1"},
                new Product(){ProductID=2,Name="P2"},
                new Product(){ProductID=3,Name="P3"},
                new Product(){ProductID=4,Name="P4"},
                new Product(){ProductID=5,Name="P5"}
                })
                .AsQueryable<Product>); //IQuerable лучше подходит для работы с БД, оптимизирует запросы, а так же может двигаться по коллекции в двух направлениях, лучше использовать, когда требуется НЕ весь набор данных
            ProductController productController = new ProductController(mock.Object); //передаем экземпляр нашего мока в контроллер

            productController.PageSize = 3;

            //IEnumerable<Product> result = productController.View(2).ViewData.Model as IEnumerable<Product>; // имитируем вызов второй страницы

            ProductsListViewModel result = productController.View(2).ViewData.Model as ProductsListViewModel;


            Product[] array = result.Products.ToArray();

            Assert.True(array.Length == 2); //нужное ли колличество выдал

            Assert.Equal("P4", array[0].Name); //нужные ли записи выдал

            Assert.Equal("P5", array[1].Name); //нужные ли записи выдал
        }

        [Fact]
        public void CanSendPaginationViewModel()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1, Name="P1"},
                new Product {ProductID=2, Name="P2"},
                new Product {ProductID=3, Name="P3"},
                new Product {ProductID=4, Name="P4"},
                new Product {ProductID=5, Name="P5"}
            }.AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = controller.View(2).ViewData.Model as ProductsListViewModel;

            PagingInfo pageInfo = result.PagingInfo;

            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);


        }

    }
}
