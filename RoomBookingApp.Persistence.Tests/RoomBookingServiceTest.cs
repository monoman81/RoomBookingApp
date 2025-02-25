using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Persistence
{
    public class RoomBookingServiceTest
    {

        [Fact]
        public void Should_Return_Available_Rooms()
        {
            //Arrange
            var date = new DateTime(2025, 02, 22);

            var connString = "DataSource=:memory:";
            var conn = new SqliteConnection(connString);
            conn.Open();

            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseSqlite(conn)
                .Options;

            using var context = new RoomBookingAppDbContext(dbOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();   
            
            //context.Rooms.Add(new Room { Id = 1, Name = "Room 1" });
            //context.Rooms.Add(new Room { Id = 2, Name = "Room 2" });
            //context.Rooms.Add(new Room { Id = 3, Name = "Room 3" });

            context.RoomBookings.Add(new RoomBooking { Id = 1, RoomId = 1, Date = date, Email="crpn81@yahoo.com.mx", FullName="Carlos Ponce" });
            context.RoomBookings.Add(new RoomBooking { Id = 2, RoomId = 2, Date = date.AddDays(-1), Email = "monoman81@yahoo.com.mx", FullName = "Farid Ponce" });

            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            //Act
            var availableRooms = roomBookingService.GetAvailableRooms(date);

            //Assert
            Assert.Equal(2, availableRooms.Count());
            Assert.Contains(availableRooms, q => q.Id == 2);
            Assert.Contains(availableRooms, q => q.Id == 3);
            Assert.DoesNotContain(availableRooms, q => q.Id == 1);  

        }

        [Fact]
        public void Should_Save_Room_Booking()
        {

            //var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
            //    .UseInMemoryDatabase("AvailableRoomTest")
            //    .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            //    .Options;

            //Arrange
            var connString = "DataSource=:memory:";
            var conn = new SqliteConnection(connString);
            conn.Open();

            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseSqlite(conn)
                .Options;

            var roomBooking = new RoomBooking { RoomId = 1, Date = DateTime.Now.AddDays(1).Date, Email = "crpn81@yahoo.com.mx", FullName = "Carlos Ponce" };

            using var context = new RoomBookingAppDbContext(dbOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var roomBookingService = new RoomBookingService(context);

            //Act
            roomBookingService.Save(roomBooking);

            //Assert
            var bookings = context.RoomBookings.ToList();
            var booking = Assert.Single(bookings);

            Assert.Equal(roomBooking.Date, booking.Date);
            Assert.Equal(roomBooking.RoomId, booking.RoomId);

        }

    }
}
