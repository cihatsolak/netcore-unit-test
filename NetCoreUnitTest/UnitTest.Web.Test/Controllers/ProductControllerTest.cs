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

        #region Index Action Test Methods
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
        #endregion

        #region Details Action Test Methods
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
        #endregion

        #region Create Action Test Methods
        /// <summary>
        /// HTTP Get - Create Action metotunun test edilmesi
        /// </summary>
        [Fact]
        public void Create_ActionExecutes_ReturnViewResult()
        {
            IActionResult actionResult = _productController.Create();

            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.Null(viewResult.Model);
        }

        /// <summary>
        /// HTTP Post - Create Action metotuna geçersiz product modelin test edilmesi
        /// </summary>
        [Fact]
        public void Create_InvalidModelState_ReturnViewAndProductModel()
        {
            _productController.ModelState.AddModelError("Name", "Name is required");

            IActionResult actionResult = _productController.Create(Product);

            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.NotNull(viewResult.Model);
        }

        /// <summary>
        /// HTTP Post - Create Action metotuna geçerli product modelin test edilmesi
        /// </summary>

        [Fact]
        public void Create_ValidModelState_ReturnRedirectToIndexAction()
        {
            _productRepositoryMock.Setup(p => p.Insert(Product));

            IActionResult actionResult = _productController.Create(Product);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("Index", redirectToActionResult.ActionName); //Hangi action name?
        }

        /// <summary>
        /// Http Post - Create Action metotu içindeki repository insert metotunun test edilmesi
        /// </summary>
        [Fact]
        public void Create_ValidModelState_RepositoryInCreateMethodsExecute()
        {
            Product productEntity = null;
            //It.IsAny<Product>() : İçerisine herhangi bir product gelebilir
            _productRepositoryMock.Setup(p => p.Insert(It.IsAny<Product>())).Callback<Product>(x => productEntity = x);

            IActionResult actionResult = _productController.Create(Product);

            //ProductRepository içerisindeki Insert metotu en az 1 kere çalıştı mı?
            _productRepositoryMock.Verify(repo => repo.Insert(It.IsAny<Product>()), Times.Once);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("Index", redirectToActionResult.ActionName); //Hangi action name?
            Assert.Equal<int>(Product.Id, productEntity.Id);
        }

        /// <summary>
        /// Http Post - Create Action metotu içindeki repository insert metotunun test edilmesi
        /// </summary>
        [Fact]
        public void Create_InValidModelState_RepositoryInCreateMethodsExecute()
        {
            //ModelState de hata olması için rastgele bir hata ekliyorum.
            _productController.ModelState.AddModelError("Name", "Name is required");

            _productRepositoryMock.Setup(p => p.Insert(Product));

            IActionResult actionResult = _productController.Create(Product);

            //ProductRepository içerisindeki Insert metotu ModelState invalid oldugu için hiç çalışmaması lazım.
            _productRepositoryMock.Verify(repo => repo.Insert(It.IsAny<Product>()), Times.Never);

            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.IsAssignableFrom<Product>(viewResult.Model);
            Assert.Null(viewResult.StatusCode);
        }

        #endregion
    }
}
