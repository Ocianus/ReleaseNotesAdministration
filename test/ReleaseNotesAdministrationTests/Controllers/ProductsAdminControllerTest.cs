﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Moq.Protected;
using ReleaseNotesAdministration.Controllers;
using ReleaseNotesAdministration.Models;
using ReleaseNotesAdministration.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace test.ReleaseNotesAdministrationTests.Controllers
{
    public class ProductsAdminControllerTest
    {
        private readonly Mock<IHttpClientFactory> _mockClientFactory;
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly ProductsAdminController _controller;

        public ProductsAdminControllerTest()
        {
            _mockClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpClient = new Mock<HttpClient>();
            _controller = new ProductsAdminController(_mockClientFactory.Object);
        }

        [Fact]
        public async Task ListAllProducts_Should_Return_List_With_Products()
        {
            // Arrange
            // HttpResponseMessage with a StatusCode of OK (200) and Content of products
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "[{\"productId\":1,\"productName\":\"Talent Recruiter\",\"productImage\":\"pic-recruiter.png\"}," +
                    "{\"productId\":2,\"productName\":\"Talent Manager\",\"productImage\":\"pic-manager.png\"}," +
                    "{\"productId\":3,\"productName\":\"Talmundo\",\"productImage\":\"logo_talmundo.png\"}]")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.GetAsync("/Product/");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            var result = await controller.ListAllProducts();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(3, ((List<ProductAdminViewModel>)viewResult.ViewData.Model).Count);

            var products = viewResult.ViewData.Model as IList<ProductAdminViewModel>;
            Assert.Equal(1, products[0].ProductId);
            Assert.Equal("Talent Recruiter", products[0].ProductName);
            Assert.Equal("pic-recruiter.png", products[0].ProductImage);

            Assert.Equal(2, products[1].ProductId);
            Assert.Equal("Talent Manager", products[1].ProductName);
            Assert.Equal("pic-manager.png", products[1].ProductImage);

            Assert.Equal(3, products[2].ProductId);
            Assert.Equal("Talmundo", products[2].ProductName);
            Assert.Equal("logo_talmundo.png", products[2].ProductImage);

            Assert.IsAssignableFrom<List<ProductAdminViewModel>>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task ListAllProducts_Should_Throw_Exception()
        {
            // Arrange
            // HttpResponseMessage with a StatusCode of NotFound and Content of an empty string
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.GetAsync("/Product/");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.ListAllProducts());
        }

        [Fact]
        public async Task ViewProduct_Should_Return_View_With_product()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // HttpResponseMessage with a StatusCode of OK (200) and Content of products
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"productId\": 1,\"productName\":\"Talent Recruiter\",\"productImage\":\"pic-recruiter.png\"}")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.GetAsync($"/Product/{Id}");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            var result = await controller.ViewProduct(Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(1, ((ProductAdminViewModel)viewResult.ViewData.Model).ProductId);
            Assert.Equal("Talent Recruiter", ((ProductAdminViewModel)viewResult.ViewData.Model).ProductName);
            Assert.Equal("pic-recruiter.png", ((ProductAdminViewModel)viewResult.ViewData.Model).ProductImage);

            Assert.IsAssignableFrom<ProductAdminViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task ViewProduct_Should_Throw_Exception()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // HttpResponseMessage with a StatusCode of OK (200) and Conent of products
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.GetAsync($"/Product/{Id}");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.ViewProduct(Id));
        }

        [Fact]
        public void Create_Should_Return_View()
        {
            //Arrange
            var controller = _controller;

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public async Task CreateProduct_Should_Create_Product()
        {
            // Arrange
            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("CreateProduct", "Success"));

            // testProduct for creation of product
            ProductAdminApiModel testProduct = new ProductAdminApiModel
            {
                ProductId = 13,
                ProductName = "Talent Helper",
                ProductImage = "pic-helper.png"
            };

            // HttpResponseMessage with a StatusCode of OK (200) and Conent of products
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"productId\":13,\"productName\":\"Talent Helper\",\"productImage\":\"pic-helper.png\"}")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.PostAsync("/Product/", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.CreateProduct(testProduct);

            // Assert
            Assert.Matches(@"^[A-Za-z0-9\s\-_,\.;:!()+']{3,99}$",
                testProduct.ProductName);
            Assert.Matches(@"^([a-zA-Z0-9\s_\\.\-\(\):])+(.jpg|.JPG|.jpeg|.JPEG|.png|.PNG|.gif|.GIF)$",
                testProduct.ProductImage);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ListAllProducts", viewResult.ActionName);
        }

        [Fact]
        public async Task CreateProduct_Should_Return_Create_View_Regex_Fails()
        {
            // Arrange
            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("CreateProduct", "Failed"));

            // testProduct for creation of product
            ProductAdminApiModel testProduct = new ProductAdminApiModel
            {
                ProductId = 13,
                ProductName = "@@£fslfø",
                ProductImage = "pic-helper"
            };

            // HttpResponseMessage with a StatusCode of OK (200) and Conent of products
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.PostAsync("/Product/", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.CreateProduct(testProduct);

            // Assert
            Assert.DoesNotMatch(@"^[A-Za-z0-9\s\-_,\.;:!()+']{3,99}$",
                testProduct.ProductName);
            Assert.DoesNotMatch(@"^([a-zA-Z0-9\s_\\.\-\(\):])+(.jpg|.JPG|.jpeg|.JPEG|.png|.PNG|.gif|.GIF)$",
                testProduct.ProductImage);

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public async Task CreateProduct_Should_Throw_Exception()
        {
            // Arrange
            // testProduct for creation of product
            ProductAdminApiModel testProduct = new ProductAdminApiModel
            {
                ProductId = 13,
                ProductName = "Talent Helper",
                ProductImage = "pic-helper.png"
            };

            // HttpResponseMessage with a StatusCode of NotFound and Content of an empty string
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.PostAsync("/Product/", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.CreateProduct(testProduct));
        }

        [Fact]
        public async Task EditProduct_With_Only_IdAsParameter_Should_Return_View_With_Updated_Product()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // HttpResponseMessage with a StatusCode of OK (200) and Conent of products
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"ProductId\":1,\"ProductName\":\"Talent Recruiter\",\"ProductImage\":\"pic-recruiter.png\"}")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.GetAsync($"/Product/{Id}");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            var result = await controller.EditProduct(Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(1, ((ProductAdminViewModel)viewResult.ViewData.Model).ProductId);
            Assert.Equal("Talent Recruiter", ((ProductAdminViewModel)viewResult.ViewData.Model).ProductName);
            Assert.Equal("pic-recruiter.png", ((ProductAdminViewModel)viewResult.ViewData.Model).ProductImage);

            Assert.IsAssignableFrom<ProductAdminViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task EditProduct_With_Only_IdAsParameter_Should_Throw_Exception()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // HttpResponseMessage with a StatusCode of NotFound and Content of an empty string
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.GetAsync($"/Product/{Id}");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.EditProduct(Id));
        }

        [Fact]
        public async Task EditProduct_Should_Redirect_To_ViewProduct()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("EditProduct", "Success"));

            // testProduct 
            ProductAdminViewModel testProduct = new ProductAdminViewModel
            {
                ProductId = 1,
                ProductName = "Talent Recruiter",
                ProductImage = "pic-recruiter.png"
            };

            // HttpResponseMessage with a StatusCode of NotFound and Content of an empty string
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"ProductId\":1,\"ProductName\":\"Talent Recruiter\",\"ProductImage\":\"pic-recruiter.png\"}")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.PutAsync($"/Product/{Id}", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.EditProduct(Id, testProduct);

            // Assert
            Assert.Matches(@"^[a-zA-Z0-9, _ - ! ?. ""]*$",
                testProduct.ProductName);
            Assert.Matches(@"^([a-zA-Z0-9\s_\\.\-\(\):])+(.jpg|.JPG|.jpeg|.JPEG|.png|.PNG|.gif|.GIF)$",
                testProduct.ProductImage);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ViewProduct", viewResult.ActionName);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task EditProduct_Should_Return_Edit_View_Regex_Fails()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // mocking TempData
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("EditProduct", "Failed"));

            // testProduct 
            ProductAdminViewModel testProduct = new ProductAdminViewModel
            {
                ProductId = 1,
                ProductName = "@@@}@£$£][",
                ProductImage = "elephant_picture"
            };

            // HttpResponseMessage with a StatusCode of NotFound and Content of an empty string
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"ProductId\":1,\"ProductName\":\"@@@}@£$£][\",\"ProductImage\":\"elephant_picture\"}")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.PutAsync($"/Product/{Id}", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.EditProduct(Id, testProduct);

            // Assert
            Assert.DoesNotMatch(@"^[a-zA-Z0-9, _ - ! ?. ""]*$",
                testProduct.ProductName);
            Assert.DoesNotMatch(@"^([a-zA-Z0-9\s_\\.\-\(\):])+(.jpg|.JPG|.jpeg|.JPEG|.png|.PNG|.gif|.GIF)$",
                testProduct.ProductImage);

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public async Task EditProduct_Should_Throw_Exception()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // testProduct 
            ProductAdminViewModel testProduct = new ProductAdminViewModel
            {
                ProductId = 1,
                ProductName = "Talent Recruiter",
                ProductImage = "pic-recruiter.png"
            };

            // HttpResponseMessage with a StatusCode of NotFound and Content of an empty string
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.PutAsync($"/Product/{Id}", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.EditProduct(Id, testProduct));
        }

        [Fact]
        public async Task DeleteProduct_Should_Delete_Product_And_RedirectToAction()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("DeleteProduct", "Success"));

            // HttpResponseMessage with a StatusCode of OK (200) and Conent of products
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"productId\":1,\"productName\":\"Talent Recruiter\",\"productImage\":\"pic-recruiter.png\"}")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            // requesting delete of product with Id
            var httpClientResult = await httpClient.DeleteAsync($"/Product/{Id}");

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.DeleteProduct(Id);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ListAllProducts", viewResult.ActionName);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_Should_Throw_Exception()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // HttpResponseMessage with a StatusCode of OK (200) and Conent of products
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            // requesting delete of product with Id
            var httpClientResult = await httpClient.DeleteAsync($"/Product/{Id}");

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ProductsAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.DeleteProduct(Id));
        }
    }
}
