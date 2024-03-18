using FluentAssertions;
using KodyPromocyjneAPI.BusinessLayer.Services;
using KodyPromocyjneAPI.Controllers;
using KodyPromocyjneAPI.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KodyPromocyjneAPI.Tests.Controller
{
    public class PromoCodeControllersTests
    {
        private readonly Mock<IPromoCodeServices> _promoCodeServices;
        private readonly Mock<IChangeLogServices> _changeLogServices;
        private PromoCodeController _controller;
        private List<PromoCode> _promoCodes;
        private PromoCode _promoCode;

        public PromoCodeControllersTests()
        {
            _promoCodeServices = new Mock<IPromoCodeServices>();
            _changeLogServices = new Mock<IChangeLogServices>();
        }

        [SetUp]
        public void Setup()
        {
            _controller = new PromoCodeController(
                _promoCodeServices.Object,
                _changeLogServices.Object);

           _promoCodes = new List<PromoCode>
            {
                new PromoCode { Id = 1, Name = "Zielony", NumberOfPossibleUses = 10,  IsActive = true },
                new PromoCode { Id = 2, Name = "Czerwony", NumberOfPossibleUses = 20, IsActive = true },
                new PromoCode { Id = 3, Name = "Zolty", NumberOfPossibleUses = 30, IsActive = true }
            };

            _promoCode = new PromoCode { Id = 1, Name = "Zielony", NumberOfPossibleUses = 10, IsActive = true };
        }

        [Test]
        public void PromoCodeController_GetPromoCodes_ReturnsOk()
        {
            // Arrange
            _promoCodeServices.Setup(x => x.GetAllCodesAsync()).ReturnsAsync(_promoCodes);

            // Act
            var result = _controller.GetPromoCodes();

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(_promoCodes));
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void PromoCodeController_GetPromoCodes_Returns500()
        {
            // Arrange
            _promoCodeServices.Setup(x => x.GetAllCodesAsync()).ThrowsAsync(new Exception("Unit Test"));

            // Act
            var result = _controller.GetPromoCodes();

            // Assert
            result.Should().NotBeNull();
            var statusCodeResult = result.Result as ObjectResult;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task PromoCodeController_GetPromoCodeById_ReturnsOk()
        {
            // Arrange
            _promoCodeServices.Setup(x => x.GetCodeByIdWithTransactionAsync(_promoCode.Id)).ReturnsAsync(_promoCode);

            // Act
            var result = await _controller.GetPromoCodeById(_promoCode.Id);

            //Assert
            result.Should().NotBeNull();
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(_promoCode));
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void PromoCodeController_PostPromoCode_ReturnOK()
        {
            // Arrange
            _promoCodeServices.Setup(x => x.CheckActiveCodeExistenceAsync(_promoCode)).ReturnsAsync(false);

            // Act
            var result = _controller.PostPromoCode(_promoCode);

            //Assert
            result.Should().NotBeNull();
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }
    }
}
