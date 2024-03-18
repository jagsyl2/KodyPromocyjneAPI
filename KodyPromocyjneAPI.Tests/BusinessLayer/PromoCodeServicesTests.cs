using KodyPromocyjneAPI.BusinessLayer.Services;
using KodyPromocyjneAPI.DataLayer;
using KodyPromocyjneAPI.DataLayer.Models;
using Moq;

namespace KodyPromocyjneAPI.Tests.BusinessLayer
{
    public class PromoCodeServicesTests
    {
        private readonly Func<IPromoCodesDbContext> _dbContextFactory;
        private readonly Mock<IChangeLogServices> _changeLogServices;
        private PromoCodeServices _controller;

        public PromoCodeServicesTests()
        {
            _dbContextFactory = () => new PromoCodesInMemoryDbContext();
            _changeLogServices = new Mock<IChangeLogServices>();
        }

        [SetUp]
        public void Setup()
        {
            using (var context = _dbContextFactory())
            {
                var promoCodes = context.PromoCodes.ToList();

                context.PromoCodes.RemoveRange(promoCodes);

                context.PromoCodes.AddRange(
                    new PromoCode { Id = 1, Name = "Zielony", NumberOfPossibleUses = 10, IsActive = true },
                    new PromoCode { Id = 2, Name = "Czerwony", NumberOfPossibleUses = 20, IsActive = true },
                    new PromoCode { Id = 3, Name = "Zolty", NumberOfPossibleUses = 30, IsActive = true }
                    );

                context.SaveChanges();
            }

            _controller = new PromoCodeServices(
                _dbContextFactory, 
                _changeLogServices.Object);
        }

        [Test]
        public void PromoCodeServices_GetAllCodesAsync_ReturnsCodes()
        {
            using (var context = _dbContextFactory())
            {
                var result = _controller.GetAllCodesAsync().Result;

                Assert.That(result.Count, Is.EqualTo(3));
                Assert.That(result[2].Name, Is.EqualTo("Zolty"));
            }
        }

        [Test]
        public void PromoCodeServices_DeleteAsync_DeletesCode()
        {
            var code = new PromoCode { Id = 1, Name = "Zielony", NumberOfPossibleUses = 10, IsActive = true };

            _controller.DeleteAsync(code).Wait();

            using (var context = _dbContextFactory())
            {
                var result = context.PromoCodes.ToList();

                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result[0].Name, Is.EqualTo("Czerwony"));
            }
        }

        [Test]
        public void PromoCodeServices_GetCodeByIdWithTransactionAsync_ReturnCode()
        {
            using(var context = _dbContextFactory())
            {
                var result = _controller.GetCodeByIdWithTransactionAsync(2).Result;

                Assert.That(result.Id, Is.EqualTo(2));
                Assert.That(result.Name, Is.EqualTo("Czerwony"));
                Assert.That(result.NumberOfPossibleUses, Is.EqualTo(19));
            }
        }
    }
}