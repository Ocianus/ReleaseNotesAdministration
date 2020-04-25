﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using ReleaseNotes.Controllers;
using ReleaseNotes.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace test.ReleaseNotesTests.Controllers
{
    public class ProductControllerTest
    {
        private readonly Mock<IHttpClientFactory> _mockClientFactory;
        private Mock<HttpClient> _mockProductsClient;
        private readonly ProductController _controller;

        public ProductControllerTest()
        {
            _mockClientFactory = new Mock<IHttpClientFactory>();
            _mockProductsClient = new Mock<HttpClient>();
            _controller = new ProductController(_mockClientFactory.Object);
        }

        [Fact]
        public async Task ListAllProducts_Should_Return_View_With_List()
        {
            // Arrange
            // HttpResponseMessage with a StatusCode of OK (200) and Conent of products
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[{\"productId\":1,\"productName\":\"Talent Recruiter\",\"productImage\":\"pic-recruiter.png\"},{\"productId\":2,\"productName\":\"Talent Manager\",\"productImage\":\"pic-manager.png\"},{\"productId\":3,\"productName\":\"Talmundo\",\"productImage\":\"logo_talmundo.png\"},{\"productId\":10,\"productName\":\"ReachMee\",\"productImage\":\"reachmeelogo.png\"},{\"productId\":12,\"productName\":\"Webrecruiter\",\"productImage\":\"webrecruiter-logo.png\"}]")
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
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesApiClient"))
                                    .Returns(httpClient);

            var controller = new ProductController(httpClientFactoryMock.Object);

            // Act
            var result = await controller.ListAllProducts();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<ProductViewModel>>(viewResult.ViewData.Model);
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
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesApiClient")).Returns(httpClient);

            var controller = new ProductController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.ListAllProducts());
        }
    }
}