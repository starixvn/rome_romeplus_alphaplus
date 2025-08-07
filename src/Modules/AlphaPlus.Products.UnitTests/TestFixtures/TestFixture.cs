using AlphaPlus.Products.Configurations;
using AlphaPlus.Products.Models;
using Microsoft.Extensions.Configuration;

namespace AlphaPlus.Products.UnitTests.TestFixtures
{
    public class TestFixture
    {
        public Fixture Fixture { get; }
        public Mock<IConfiguration> MockConfiguration { get; }
        public Mock<IConfigurationSection> MockConfigurationSection { get; }

        public TestFixture()
        {
            Fixture = new Fixture();
            MockConfiguration = new Mock<IConfiguration>();
            MockConfigurationSection = new Mock<IConfigurationSection>();
        }

        public ProductConfiguration CreateProductConfiguration()
        {
            return new ProductConfiguration
            {
                Products = new ProductItem[]
                {
                    new ProductItem { ProductId = 1, Enabled = true, EndSupportDate = DateTime.Now.AddDays(30) },
                    new ProductItem { ProductId = 2, Enabled = true, EndSupportDate = DateTime.Now.AddDays(30) },
                    new ProductItem { ProductId = 3, Enabled = false, EndSupportDate = DateTime.Now.AddDays(30) },
                    new ProductItem { ProductId = 4, Enabled = true, EndSupportDate = DateTime.Now.AddDays(-1) }
                }
            };
        }

        public Customer CreateCustomer(int id = 1, byte level = 1, bool isCashMember = false)
        {
            return new Customer(
                id,
                0,
                $"customer{id}",
                level,
                0,
                isCashMember,
                Enumerable.Empty<CustomerProductStatus>(),
                null);
        }

        public Customer CreateCustomerWithProductStatus(int id, byte level, IEnumerable<CustomerProductStatus> productStatus)
        {
            return new Customer(
                id,
                0,
                $"customer{id}",
                level,
                productStatus);
        }

        public List<CustomerProductStatus> CreateProductStatuses(params (int productId, bool canBeSeen, int? groupId)[] statuses)
        {
            return statuses.Select(s => new CustomerProductStatus(
                s.productId,
                !s.canBeSeen, // disabled is opposite of canBeSeen
                !s.canBeSeen, // hidden is opposite of canBeSeen
                s.groupId
            )).ToList();
        }

        public Mock<ICustomer> CreateMockCustomer(int id = 1, byte level = 1)
        {
            var mockCustomer = new Mock<ICustomer>();
            mockCustomer.Setup(x => x.Id).Returns(id);
            mockCustomer.Setup(x => x.CustomerLevelValue).Returns(level);
            mockCustomer.Setup(x => x.Username).Returns($"customer{id}");
            mockCustomer.Setup(x => x.GetProductStatus()).Returns(Enumerable.Empty<CustomerProductStatus>());
            return mockCustomer;
        }

        public Mock<ICustomer> CreateMockCustomerWithProductStatus(int id, byte level, IEnumerable<CustomerProductStatus> productStatus)
        {
            var mockCustomer = CreateMockCustomer(id, level);
            mockCustomer.Setup(x => x.GetProductStatus()).Returns(productStatus);
            return mockCustomer;
        }
    }
}
