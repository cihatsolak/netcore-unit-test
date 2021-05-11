using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTest.Web.Controllers.WebApi;
using UnitTest.Web.Models;
using UnitTest.Web.Repositories;
using UnitTest.Web.Utilities;
using Xunit;

namespace UnitTest.Web.Test.Controllers.WebApi
{
    public class ApiProductControllerTest
    {
        private readonly Mock<IRepository<Product>> _productMockRepository;
        private readonly ApiProductsController _apiProductsController;
        private readonly Helper _helper;

        private List<Product> Products { get; set; }
        private Product Product { get; set; }

        public ApiProductControllerTest()
        {
            _productMockRepository = new Mock<IRepository<Product>>();
            _apiProductsController = new ApiProductsController(_productMockRepository.Object);
            _helper = new Helper();

            Products = new List<Product>
            {
                new Product(){Id = 1, Name = "Book", Color="Red",Price =100,Stock=5},
                new Product(){Id = 2, Name = "Pen", Color="Blue",Price =200,Stock=23},
                new Product(){Id = 3, Name = "Encyclopedia", Color="Green",Price =300,Stock=15},
            };

            Product = new Product() { Id = 4, Name = "Book", Color = "Red", Price = 100, Stock = 5 };
        }

        [Theory]
        [InlineData(4, 5, 9)]
        public void Add_SampleValues_ReturnTotal(int a, int b, int total)
        {
            var result = _helper.Add(a, b);

            Assert.IsType<int>(result);
            Assert.Equal(total, result);
        }

        [Fact]
        public async void GetProducts_ActionExecute_ReturnOkResultWithProduct()
        {
            _productMockRepository.Setup(p => p.GetAllAsync()).ReturnsAsync(Products); //Db'ye gidiceğine taklit ettim.

            IActionResult actionResult = await _apiProductsController.GetProducts();

            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okObjectResult.Value);

            Assert.Equal<int>(3, products.Count());
            Assert.NotNull(products);
        }

        [Theory]
        [InlineData(5)]
        public async void GetProduct_IdInValid_ReturnNotFound(int productId)
        {
            Product product = null;
            _productMockRepository.Setup(p => p.GetByIdAsync(productId)).ReturnsAsync(product);

            IActionResult actionResult = await _apiProductsController.GetProduct(productId);

            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult);
            Assert.Equal<int>(404, notFoundResult.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async void GetProduct_IdValid_ReturnOkResult(int productId)
        {
            Product product = Products.First(p => p.Id == productId);
            _productMockRepository.Setup(p => p.GetByIdAsync(productId)).ReturnsAsync(product);

            IActionResult actionResult = await _apiProductsController.GetProduct(productId);

            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var resultProduct = Assert.IsAssignableFrom<Product>(okObjectResult.Value);

            Assert.Equal(product.Id, resultProduct.Id);
            Assert.Equal(product.Name, resultProduct.Name);
            Assert.NotNull(resultProduct);
        }

        [Theory]
        [InlineData(1)]
        public async void PutProduct_IdIsNotEqualProduct_ReturnBadRequestResult(int productId)
        {
            Product product = Products.First(p => p.Id == productId);
            IActionResult actionResult = await _apiProductsController.PutProduct(2, product);

            var badRequestResult = Assert.IsType<BadRequestResult>(actionResult);
            Assert.Equal<int>(400, badRequestResult.StatusCode);
        }

        [Theory]
        [InlineData(4)]
        public async void PutProduct_ActionExecute_ReturnNoContentResult(int productId)
        {
            _productMockRepository.Setup(p => p.UpdateAsync(Product));

            IActionResult actionResult = await _apiProductsController.PutProduct(productId, Product);

            _productMockRepository.Verify(p => p.UpdateAsync(It.IsAny<Product>()), Times.Once); //1 kere çalışması gerekir. 1 den fazla çalışmaz.

            var noContentResult = Assert.IsType<NoContentResult>(actionResult);
            Assert.Equal<int>(204, noContentResult.StatusCode);
        }

        [Fact]
        public async void PostProduct_ActionExecute_ReturnCreatedAction()
        {
            var product = new Product
            {
                Color = "Deneme",
                Name = "Deneme",
                Price = 100,
                Stock = 1
            };

            _productMockRepository.Setup(p => p.InsertAsync(product)).Returns(Task.CompletedTask);

            IActionResult actionResult = await _apiProductsController.PostProduct(product);

            _productMockRepository.Verify(p => p.InsertAsync(It.IsAny<Product>()), Times.Once); //Mock: metotun 1 kere çalıştığının dogrulaması

            var createdToActionResult = Assert.IsType<CreatedAtActionResult>(actionResult);
            Assert.Equal("GetProduct", createdToActionResult.ActionName);
            Assert.IsAssignableFrom<Product>(createdToActionResult.Value);
        }

        [Theory]
        [InlineData(6)]
        public async void DeleteProduct_IdInValid_ReturnNotFoundResult(int productId)
        {
            Product product = null;
            _productMockRepository.Setup(p => p.GetByIdAsync(productId)).ReturnsAsync(product);

            ActionResult<Product> actionResult = await _apiProductsController.DeleteProduct(productId);

            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
            Assert.Equal<int>(404, notFoundResult.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteProduct_ActionExecute_ReturnNoContent(int productId)
        {
            _productMockRepository.Setup(p => p.GetByIdAsync(productId)).ReturnsAsync(Product);
            _productMockRepository.Setup(p => p.DeleteAsync(Product)).Returns(Task.CompletedTask);

            ActionResult<Product> actionResult = await _apiProductsController.DeleteProduct(productId);

            _productMockRepository.Verify(p => p.DeleteAsync(It.IsAny<Product>()), Times.Once);

            var noContentResult = Assert.IsType<NoContentResult>(actionResult.Result);
            Assert.Equal<int>(204, noContentResult.StatusCode);
        }
    }
}
