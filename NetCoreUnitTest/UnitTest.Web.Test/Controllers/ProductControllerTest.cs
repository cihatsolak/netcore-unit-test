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
        private readonly ProductsController _productsController;

        private IEnumerable<Product> Products { get; set; }
        private Product Product { get; set; }
        #endregion

        #region Ctor
        public ProductControllerTest()
        {
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _productsController = new ProductsController(_productRepositoryMock.Object);

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
            IActionResult result = _productsController.Index();

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

            IActionResult result = _productsController.Index();

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
            IActionResult result = _productsController.Details(null);

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

            IActionResult result = _productsController.Details(id);

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

            IActionResult result = _productsController.Details(id);

            var viewResult = Assert.IsType<ViewResult>(result); //Geriye ViewResult mı dönüyor?
            Assert.IsAssignableFrom<Product>(viewResult.Model); //View içerisinde dönen model Product mı?
        }
        #endregion

        #region Create Action Test Methods
        /// <summary>
        /// HTTP Get - Create Action metotunun test edilmesi
        /// </summary>
        [Fact]
        public void CreateGET_ActionExecutes_ReturnViewResult()
        {
            IActionResult actionResult = _productsController.Create();

            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.Null(viewResult.Model);
        }

        /// <summary>
        /// HTTP Post - Create Action metotuna geçersiz product modelin test edilmesi
        /// </summary>
        [Fact]
        public void CreatePOST_InvalidModelState_ReturnViewAndProductModel()
        {
            _productsController.ModelState.AddModelError("Name", "Name is required");

            IActionResult actionResult = _productsController.Create(Product);

            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.NotNull(viewResult.Model);
        }

        /// <summary>
        /// HTTP Post - Create Action metotuna geçerli product modelin test edilmesi
        /// </summary>

        [Fact]
        public void CreatePOST_ValidModelState_ReturnRedirectToIndexAction()
        {
            _productRepositoryMock.Setup(p => p.Insert(Product));

            IActionResult actionResult = _productsController.Create(Product);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("Index", redirectToActionResult.ActionName); //Hangi action name?
        }

        /// <summary>
        /// Http Post - Create Action metotu içindeki repository insert metotunun test edilmesi
        /// </summary>
        [Fact]
        public void CreatePOST_ValidModelState_RepositoryInCreateMethodsExecute()
        {
            Product productEntity = null;
            //It.IsAny<Product>() : İçerisine herhangi bir product gelebilir
            _productRepositoryMock.Setup(p => p.Insert(It.IsAny<Product>())).Callback<Product>(x => productEntity = x);

            IActionResult actionResult = _productsController.Create(Product);

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
        public void CreatePOST_InValidModelState_RepositoryInCreateMethodsExecute()
        {
            //ModelState de hata olması için rastgele bir hata ekliyorum.
            _productsController.ModelState.AddModelError("Name", "Name is required");

            _productRepositoryMock.Setup(p => p.Insert(Product));

            IActionResult actionResult = _productsController.Create(Product);

            //ProductRepository içerisindeki Insert metotu ModelState invalid oldugu için hiç çalışmaması lazım.
            _productRepositoryMock.Verify(repo => repo.Insert(It.IsAny<Product>()), Times.Never);

            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.IsAssignableFrom<Product>(viewResult.Model);
            Assert.Null(viewResult.StatusCode);
        }

        #endregion

        #region Edit Action Test Methods
        [Fact]
        public void EditGET_IdNull_ReturnNotFound()
        {
            IActionResult actionResult = _productsController.Edit(null);

            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult);
            Assert.Equal<int>(404, notFoundResult.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public void EditGET_IdIsNull_ReturnNotFound(int id)
        {
            Product product = null;
            _productRepositoryMock.Setup(p => p.GetById(id)).Returns(product);

            IActionResult actionResult = _productsController.Edit(id);

            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult);
            Assert.Equal<int>(404, notFoundResult.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public void EditGET_ActionExecutes_ReturnViewWithProductEntity(int id)
        {
            _productRepositoryMock.Setup(p => p.GetById(id)).Returns(Product);

            IActionResult actionResult = _productsController.Edit(id);

            var viewResult = Assert.IsType<ViewResult>(actionResult);
            var resultProduct = Assert.IsAssignableFrom<Product>(viewResult.Model);
            Assert.Equal(Product.Id, resultProduct.Id);
        }

        [Theory]
        [InlineData(1)]
        public void EditPOST_IdIsNotEqualProductId_ReturnNotFound(int id)
        {
            IActionResult actionResult = _productsController.Edit(id, Product);

            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult);
            Assert.Equal<int>(404, notFoundResult.StatusCode);
        }

        [Theory]
        [InlineData(4)]
        public void EditPOST_InvalidModelState_ReturnViewWithProduct(int id)
        {
            //ModelState.IsValid'i geçemesin diye herhangi bir hata.
            _productsController.ModelState.AddModelError("Name", "Herhangi bir hata.");

            IActionResult actionResult = _productsController.Edit(id, Product);

            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.IsType<Product>(viewResult.Model);
        }

        [Theory]
        [InlineData(4)]
        public void EditPOST_ValidModelState_ReturnRedirectToActionIndex(int id)
        {
            _productRepositoryMock.Setup(p => p.Update(Product));

            IActionResult actionResult = _productsController.Edit(id, Product);

            //Bu istekde Update metotunun 1 kere çalışması gerek. Times.Once ile bunun kontrolünü yapıyoruz.
            _productRepositoryMock.Verify(p => p.Update(It.IsAny<Product>()), Times.Once);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
        #endregion

        #region Delete Action Test Methods
        [Fact]
        public void DeleteGET_IdIsNull_ReturnRedirectToActionIndex()
        {
            IActionResult actionResult = _productsController.Delete(null);

            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult);
            Assert.Equal<int>(404, notFoundResult.StatusCode);
        }

        [Theory]
        [InlineData(4)]
        public void DeleteGET_IdValid_ReturnNotFound(int id)
        {
            Product product = null;
            _productRepositoryMock.Setup(p => p.GetById(id)).Returns(product);

            IActionResult actionResult = _productsController.Delete(id);

            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult);
            Assert.Equal<int>(404, notFoundResult.StatusCode);
        }

        [Theory]
        [InlineData(4)]
        public void DeleteGET_ActionExecute_ReturnViewProduct(int id)
        {
            _productRepositoryMock.Setup(p => p.GetById(id)).Returns(Product);

            IActionResult actionResult = _productsController.Delete(id);

            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.IsAssignableFrom<Product>(viewResult.Model);
        }

        [Theory]
        [InlineData(4)]
        public void DeleteComfirmedPOST_ActionExecute_ReturnRedirectToActionIndex(int id)
        {
            _productRepositoryMock.Setup(p => p.GetById(id)).Returns(Product);
            _productRepositoryMock.Setup(p => p.Delete(Product));

            IActionResult actionResult = _productsController.DeleteConfirmed(id);

            _productRepositoryMock.Verify(p => p.Delete(It.IsAny<Product>()), Times.Once); //1 kere çalışması lazım.

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
        #endregion
    }
}
