using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Moq;
using Castle.Core.Logging;
using RoomBookingApp.Api.Controllers;
using Microsoft.Extensions.Logging;

namespace RoomBookingApp.Api
{

    public class UnitTest1
    {

        [Fact]
        public void Should_Return_Forecast_Results()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            //Act
            var result = controller.Get();

            //Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBeGreaterThan(1);
            

        }

    }
}
