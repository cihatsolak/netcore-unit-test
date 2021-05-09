using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using UnitTest.Web.Controllers;
using UnitTest.Web.Models;
using UnitTest.Web.Repositories;
using Xunit;

namespace UnitTest.Web.Test.Controllers
{
    /// <summary>
    /// ProductController Test Metotları
    /// </summary>
    public class ProductControllerTest
    {
        #region Fields and Properties
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly ProductsController _productController;

        private IEnumerable<Product> Products { get; set; }
        private Product Product { get; set; }
        #endregion

        #region Ctor
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

            Product = new Product() { Id = 4, Name = "Book", Color = "Red", Price = 100, Stock = 5 };
        }
        #endregion

        /// <summary>
        /// IActionResult Index() ViewResult Testi
        /// </summary>
        [Fact]
        public void Index_ActionExecutes_ReturnView()
        {
            IActionResult result = _productController.Index();

            Assert.IsType<ViewResult>(result);
        }

        /// <summary>
        /// IActionResult Index() Ürün Listesi Testi
        /// </summary>
        [Fact]
        public void Index_ActionExecutes_ReturnProductList()
        {
            //Veri tabanından verileri çekmek yerine mock'lama işlemi yapıyorum.
            _productRepositoryMock.Setup(p => p.GetAll()).Returns(Products);

            IActionResult result = _productController.Index();

            var viewResult = Assert.IsType<ViewResult>(result); //Geriye viewResult dönüyor mu?
            var products = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model); //Dönen model tipini kontrol ettim.
            Assert.Equal<int>(3, products.Count()); //Dönen listenin eleman sayısını kontrol ettim.
        }

        /// <summary>
        /// IActionResult Details() Id null olma durumu
        /// </summary>
        [Fact]
        public void Details_IdIsNull_ReturnRedirectToIndexAction()
        {
            IActionResult result = _productController.Details(null);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result); //Geriye RedirectToActionResult mı dönüyor?
            Assert.Equal("Index", redirectToActionResult.ActionName); //Hangi action name?
        }

        /// <summary>
        /// IActionResult Details() veri tabanında ürün olmama durumu
        /// </summary>
        [Theory]
        [InlineData(1)]
        public void Details_IdInvalid_ReturnNotFound(int id)
        {
            Product productNull = null;
            _productRepositoryMock.Setup(p => p.GetById(id)).Returns(productNull);

            IActionResult result = _productController.Details(id);

            var notFoundResult = Assert.IsType<NotFoundResult>(result); //Geriye RedirectToActionResult mı dönüyor?
            Assert.Equal<int>(404, notFoundResult.StatusCode); //Status code 404 mü?
        }

        /// <summary>
        /// IActionResult Details() veri tabanında ürün olmama durumu
        /// </summary>
        [Theory]
        [InlineData(4)]
        public void Details_IdValid_ReturnViewWithProductModel(int id)
        {
            _productRepositoryMock.Setup(p => p.GetById(id)).Returns(Product);

            IActionResult result = _productController.Details(id);

            var viewResult = Assert.IsType<ViewResult>(result); //Geriye ViewResult mı dönüyor?
            Assert.IsAssignableFrom<Product>(viewResult.Model); //View içerisinde dönen model Product mı?
        }
    }
}
