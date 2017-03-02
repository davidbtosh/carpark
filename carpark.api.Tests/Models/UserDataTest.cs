using carpark.api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace carpark.api.Tests.Models
{
    [TestClass]
    public class UserDataTest
    {

        [TestMethod]
        public void UserDataExitHourSameDayTest()
        {
            DateTime entry = DateTime.Parse("2017-03-01 18:00:00");
            DateTime exit = DateTime.Parse("2017-03-01 19:30:00");
            UserData ud = new UserData(entry, exit);
            
            // Assert
            Assert.AreEqual(19.5, ud.ExitHour);

        }

        [TestMethod]
        public void UserDataExitHourNextDayTest()
        {
            // Arrange
            DateTime entry = DateTime.Parse("2017-03-01 18:00:00");
            DateTime exit = DateTime.Parse("2017-03-02 05:00:00");
            UserData ud = new UserData(entry, exit);
           
            // Assert
            Assert.AreEqual(29, ud.ExitHour);
           
        }

        [TestMethod]
        public void UserDataExitHourThreeDayTest()
        {
            // Arrange
            DateTime entry = DateTime.Parse("2017-03-01 18:00:00");
            DateTime exit = DateTime.Parse("2017-03-04 18:00:00");
            UserData ud = new UserData(entry, exit);
            
            // Assert
            Assert.AreEqual(90, ud.ExitHour);

        }


        [TestMethod]
        public void UserDataTotalHoursSameDayTest()
        {
            DateTime entry = DateTime.Parse("2017-03-01 18:00:00");
            DateTime exit = DateTime.Parse("2017-03-01 19:00:00");
            UserData ud = new UserData(entry, exit);
            
            // Assert
            Assert.AreEqual(1, ud.TotalHours);

        }

        [TestMethod]
        public void UserDataTotalHoursNextDayTest()
        {
            // Arrange
            DateTime entry = DateTime.Parse("2017-03-01 18:00:00");
            DateTime exit = DateTime.Parse("2017-03-02 05:00:00");
            UserData ud = new UserData(entry, exit);
           
            // Assert
            Assert.AreEqual(11, ud.TotalHours);

        }

        [TestMethod]
        public void UserDataTotalHoursThreeDayTest()
        {
            // Arrange
            DateTime entry = DateTime.Parse("2017-03-01 18:00:00");
            DateTime exit = DateTime.Parse("2017-03-04 18:00:00");
            UserData ud = new UserData(entry, exit);           

            // Assert
            Assert.AreEqual(72, ud.TotalHours);

        }

        [TestMethod]
        public void UserDataTotalDaysSameDayTest()
        {
            DateTime entry = DateTime.Parse("2017-03-01 18:00:00");
            DateTime exit = DateTime.Parse("2017-03-01 19:00:00");
            UserData ud = new UserData(entry, exit);

            // Assert
            Assert.AreEqual(1, ud.TotalHours);

        }

        [TestMethod]
        public void UserDataTotalDaysNextDayTest()
        {
            // Arrange
            DateTime entry = DateTime.Parse("2017-03-01 18:00:00");
            DateTime exit = DateTime.Parse("2017-03-02 05:00:00");
            UserData ud = new UserData(entry, exit);

            // Assert
            Assert.AreEqual(11, ud.TotalHours);

        }

        [TestMethod]
        public void UserDataTotalDaysThreeDayTest()
        {
            // Arrange
            DateTime entry = DateTime.Parse("2017-03-01 18:00:00");
            DateTime exit = DateTime.Parse("2017-03-04 18:00:00");
            UserData ud = new UserData(entry, exit);

            // Assert
            Assert.AreEqual(72, ud.TotalHours);

        }

        [TestMethod]
        public void UserDataDayOfWeekTest()
        {
            // Arrange
            
            DateTime exit = DateTime.Parse("2017-03-12 18:00:00");

            DateTime startEntry = DateTime.Parse("2017-03-05 18:00:00");

            UserData ud = new UserData(startEntry, exit);
            
            for(int i = 1; i <= 7; i++)
            {
                ud.Entry = startEntry.AddDays(i);
                Assert.AreEqual(i, ud.EntryDoW);
            }

        }

    }
}
