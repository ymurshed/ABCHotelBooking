using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ABM = ABCBookingManager;

namespace ABCBookingManagerUnitTest
{
    [TestClass]
    public class UnitTestUsingMocking
    {
        #region helper methods for creating mock data 
        Dictionary<string, decimal> bookingData;
        void LoadBookingData()
        {
            bookingData = new Dictionary<string, decimal>();
            bookingData.Add("123", 245.57M);
            bookingData.Add("345", 105.78M);
            bookingData.Add("789", 800.10M);
            bookingData.Add("847", 300.71M);
            bookingData.Add("546", 600.80M);
            bookingData.Add("946", 500.99M);
            bookingData.Add("257", 947.05M);
            bookingData.Add("607", 400.19M);
            bookingData.Add("347", 1000.11M);
            bookingData.Add("106", 50.06M);
        }

        string GetDate()
        {
            Random gen = new Random();
            DateTime start = new DateTime(2014, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range)).ToString();
        }

        List<KeyValuePair<string, decimal>> GetExpensiveBookingData(int limit)
        {
            LoadBookingData();
            List<KeyValuePair<string, decimal>> expBooking = new List<KeyValuePair<string, decimal>>();

            expBooking = (from dic in bookingData orderby dic.Value descending select dic).Take(limit).ToList();
            return expBooking;
        }

        decimal GetTotalTax()
        {
            decimal totalTax = 0;
            LoadBookingData();

            foreach (var data in bookingData)
            {
                decimal eachTax = Math.Round((data.Value * 0.07M), 2, MidpointRounding.AwayFromZero);
                totalTax += eachTax;
            }
            return totalTax;
        }
        #endregion

        [TestMethod]
        public void TestMock_PricePerNight_MeetsExpectation()
        {
            decimal price = 15;
            long booking_id = 200123;

            var mockABC = new Mock<ABM.BOL.IABCBookingHelper>();
            mockABC.Setup(test => test.Get_PricePerNight_For_BookingId(It.IsAny<long>())).Returns(new Random().Next(1, 2000));

            var pricePerNight = mockABC.Object.Get_PricePerNight_For_BookingId(booking_id);
            Assert.IsTrue(pricePerNight > price, "Price per night is not a valid price compared to the given lowest price. output = " + pricePerNight);
        }

        [TestMethod]
        public void TestMock_CheckInDate_MeetsExpectation()
        {
            long hotel_id = 1042;
            string sample_date = "2015-1-1";
            DateTime expectedDate = Convert.ToDateTime(sample_date); 

            var mockABC = new Mock<ABM.BOL.IABCBookingHelper>();
            mockABC.Setup(test => test.Get_Earliest_CheckInDate_For_HotelId(It.IsAny<long>())).Returns(GetDate());

            var earliestCheckInDate = Convert.ToDateTime(mockABC.Object.Get_Earliest_CheckInDate_For_HotelId(hotel_id));
            Assert.IsTrue(earliestCheckInDate >= expectedDate, "Expected earliest check-in date is older than the given expected date. output = " + earliestCheckInDate);
        }

        [TestMethod]
        public void TestMock_TopMostExpensiveBookingPrice_MeetsExpectation()
        {
            int limit = 5;
            var mockABC = new Mock<ABM.BOL.IABCBookingHelper>();
            mockABC.Setup(test => test.Get_FiveMostExpensiveBookings(limit)).Returns(GetExpensiveBookingData(limit));

            var expensiveBookings = mockABC.Object.Get_FiveMostExpensiveBookings(limit);
            Assert.AreEqual(limit, expensiveBookings.Count(), "Top most expensive booking count not met with the expected count. output = " + expensiveBookings.Count());
        }

        [TestMethod]
        public void TestMock_TotalTax_MeetsExpectation()
        {
            var mockABC = new Mock<ABM.BOL.IABCBookingHelper>();
            mockABC.Setup(test => test.Get_TotalTax_For_TheSystem()).Returns(GetTotalTax());

            var totalTax = mockABC.Object.Get_TotalTax_For_TheSystem();
            Assert.IsTrue(totalTax > 0, "Total tax is invalid. output = " + totalTax);
        }
    }
}
