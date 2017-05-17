using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restup.HttpMessage.Headers.Request;
using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.UnitTests.TestHelpers;

namespace Restup.Webserver.UnitTests.Rest
{
    [TestClass]
    public class RestRouteHandlerTests_XmlAcceptCharset : RestRouteHandlerTests
    {        
        [TestMethod]
        [DataRow("utf-8")]
        [DataRow("utf-16")]
        public void WhenAGetRequestWithAcceptMediaTypeXmlAndAnAcceptEncodingIsReceived_ThenTheResponseIsCorrectlyEncoded(string charset)
        {
            new FluentRestRouteHandlerTests()
                .Given
                    .ControllersIsRegistered<XmlTestController>("/api", new ProductStore())
                .When
                    .RequestHasArrived("/api/products",
                        acceptCharsets: new[] { charset },
                        acceptMediaTypes: new[] { "application/xml" })
                .Then
                    .AssertLastResponse(x => x.ContentCharset, charset)
                    .AssertLastResponseContent($"﻿<?xml version=\"1.0\" encoding=\"{charset}\"?><Product xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Id>{Product.DefaultId}</Id><Name>{Product.DefaultName}</Name><Currency>{Product.DefaultCurrency}</Currency></Product>");
        }

        [TestMethod]
        [DataRow("utf-8")]
        [DataRow("utf-16")]
        public void WhenAPostRequestWithAcceptMediaTypeXmlAndAnAcceptEncodingIsReceived_ThenTheRequestIsCorrectlyReceived(string charset)
        {
            var productStore = new ProductStore();
            var id = 2;
            var name = "new name";
            var currency = "£";

            var encoding = Encoding.GetEncoding(charset);

            new FluentRestRouteHandlerTests()
                .Given
                    .ControllersIsRegistered<XmlTestController>("/api", productStore)
                .When
                    .RequestHasArrived("/api/products",
                        method: HttpMethod.POST,
                        acceptCharsets: new[] { charset },
                        acceptMediaTypes: new[] { "application/xml" },
                        contentType: "application/xml",
                        contentCharset: charset,
                        content: encoding.GetBytes($"﻿<?xml version=\"1.0\" encoding=\"{charset}\"?><Product xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Id>{id}</Id><Name>{name}</Name><Currency>{currency}</Currency></Product>"))
                .Then
                    .AssertLastResponseHasNoHeaderOf<ContentTypeHeader>()
                    .AssertLastResponse(x => x.ResponseStatus, HttpResponseStatus.Created);

            Assert.AreEqual(1, productStore.PostedProducts.Count);

            var postedProduct = productStore.PostedProducts.Single();
            Assert.AreEqual(id, postedProduct.Id);
            Assert.AreEqual(name, postedProduct.Name);
            Assert.AreEqual(currency, postedProduct.Currency);
        }

        [RestController(InstanceCreationType.PerCall)]
        public class XmlTestController
        {
            private readonly ProductStore productStore;

            public XmlTestController(ProductStore productStore)
            {
                this.productStore = productStore;
            }

            [UriFormat("/products")]
            public IGetResponse GetProduct()
            {                
                return new GetResponse(GetResponse.ResponseStatus.OK, productStore.Get());
            }

            [UriFormat("/products")]
            public IPostResponse PostProduct([FromContent] Product product)
            {
                productStore.Post(product);
                return new PostResponse(PostResponse.ResponseStatus.Created);
            }
        }

        public class ProductStore
        {
            public ConcurrentQueue<Product> PostedProducts = new ConcurrentQueue<Product>();

            public Product Get()
            {
                return new Product(Product.DefaultId, Product.DefaultName, Product.DefaultCurrency);                
            }

            public void Post(Product product)
            {
                PostedProducts.Enqueue(product);
            }
        }

        public class Product
        {
            public const int DefaultId = 1;
            public const string DefaultName = "default";
            public const string DefaultCurrency = "£";

            public int Id { get; set; }
            public string Name { get; set; }
            public string Currency { get; set; }

            // xml serialisation
            private Product()
            {

            }

            public Product(int id, string name, string currency)
            {
                Id = id;
                Name = name;
                Currency = currency;
            }
        }
    }
}