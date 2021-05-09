using Moq;
using System.Collections.Generic;
using UnitTest.Web.Controllers;
using UnitTest.Web.Models;
using UnitTest.Web.Repositories;

namespace UnitTest.Web.Test.Controllers
{
    public class ProductControllerTest
    {
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly ProductsController _productController;

        private List<Product> Products { get; set; }

        public ProductControllerTest()
        {
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _productController = new ProductsController(_productRepositoryMock.Object);

            Products = new List<Product>
            {
                new Product(){Id = 1, Name = "Book", Color="Red",Price =100,Stock=5},
                new Product(){Id = 2, Name = "Pen", Color="Blue",Price =200,Stock=23},
                new Product(){Id = 3, Name = "Encyclopedia", Color="Green",Price =300,Stock=15},
            };
        }
    }
}
