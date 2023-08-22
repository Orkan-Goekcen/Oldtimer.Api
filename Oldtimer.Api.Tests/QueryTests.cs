//using Xunit;
//using Moq;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Oldtimer.Api.Queries;
//using Oldtimer.Api.Data;
//using System.Collections.Generic;
//using System.Linq;

//namespace Oldtimer.Api.Tests
//{
//    public class QueryTests
//    {
//        [Fact]
//        public async Task GetAllOldtimerQueryGetAllCars()
//        {
//            // Arrange
//            var mockCarsData = new List<Car>
//            {
//                new Car
//                {
//                    Id = 1,
//                    Brand = "Porsche",
//                    Model = "911",
//                    LicensePlate = "PORSCH123",
//                    YearOfConstruction = "1969",
//                    Colors = Car.Color.Red,
//                    Sammler = null
//                },
//                new Car
//                {
//                    Id = 2,
//                    Brand = "Ford",
//                    Model = "Mustang",
//                    LicensePlate = "MUST123",
//                    YearOfConstruction = "1970",
//                    Colors = Car.Color.Blue,
//                    Sammler = null
//                },
//                // Weitere Autos hier hinzufügen
//            }.AsQueryable();

//            var mockDbSet = new Mock<DbSet<Car>>();
//            mockDbSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(mockCarsData.Provider);
//            mockDbSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(mockCarsData.Expression);
//            mockDbSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(mockCarsData.ElementType);
//            mockDbSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(mockCarsData.GetEnumerator());

//            var mockContext = new Mock<ApiContext>();
//            mockContext.Setup(c => c.Cars).Returns(mockDbSet.Object);

//            var query = new GetAllOldtimerQuery();
//            var handler = new GetAllOldtimerQueryHandler(mockContext.Object);

//            // Act
//            var result = await handler.Handle(query, CancellationToken.None);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(mockCarsData.Count(), result.Count);
//        }
//    }
//}
