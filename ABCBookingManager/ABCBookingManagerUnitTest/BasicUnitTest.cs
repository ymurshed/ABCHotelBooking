using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ABM = ABCBookingManager;

namespace ABCBookingManagerUnitTest
{
    [TestClass]
    public class BasicUnitTest
    {
        [TestMethod]
        public void Test_DatabaseConnectivity()
        {
            ABM.BOL.IABCBookingHelper IABC = new ABM.BOL.ABCBookingHelper();

            string expectedConnectionMsg = ABM.Constants.CONNECTION_MESSAGE_PASS;
            string actualConnectionMsg = IABC.Test_DBConnection();
            Assert.AreEqual(expectedConnectionMsg, actualConnectionMsg, "Database connection failed.");
        }

        [TestMethod]
        public void Test_PricePerNight_MeetsExpectation()
        {
            ABM.BOL.IABCBookingHelper IABC = new ABM.BOL.ABCBookingHelper();

            long booking_id = 200123;
            decimal expectedPricePerNight = 233.74M;
            decimal actualPricePerNight = IABC.Get_PricePerNight_For_BookingId(booking_id);
            Assert.AreEqual(expectedPricePerNight, actualPricePerNight, "Price per night not met to the expected price for the given booking id.");
        }

        [TestMethod]
        public void Test_PricePerNight_IsValid()
        {
            ABM.BOL.IABCBookingHelper IABC = new ABM.BOL.ABCBookingHelper();

            long booking_id = 250000;
            decimal expectedPricePerNight = 0M;
            decimal actualPricePerNight = IABC.Get_PricePerNight_For_BookingId(booking_id);
            Assert.AreNotEqual(expectedPricePerNight, actualPricePerNight, "Price per night not found for the given booking id.");
        }

        [TestMethod]
        public void Test_Earliest_CheckInDate_MeetsExpectation()
        {
            ABM.BOL.IABCBookingHelper IABC = new ABM.BOL.ABCBookingHelper();

            long hotel_id = 1042;
            string expectedEarliestCheckInDate = "2015-10-27";
            string actualEarliestCheckInDate = IABC.Get_Earliest_CheckInDate_For_HotelId(hotel_id);
            Assert.AreEqual(expectedEarliestCheckInDate, actualEarliestCheckInDate, "Earliest check-in date not met to the expected date for the given hotel id.");
        }

        [TestMethod]
        public void Test_Earliest_CheckInDate_IsValid()
        {
            ABM.BOL.IABCBookingHelper IABC = new ABM.BOL.ABCBookingHelper();

            long hotel_id = 2000;
            string expectedEarliestCheckInDate = string.Empty;
            string actualEarliestCheckInDate = IABC.Get_Earliest_CheckInDate_For_HotelId(hotel_id);
            Assert.AreNotEqual(expectedEarliestCheckInDate, actualEarliestCheckInDate, "Earliest check-in date not found for the given hotel id.");
        }

        [TestMethod]
        public void Test_RequestForExpensiveBookings_ExceedLimit_ShouldThrowNullReference()
        {
            ABM.BOL.IABCBookingHelper IABC = new ABM.BOL.ABCBookingHelper();

            int limit = int.MaxValue;
            var topN_BookingIdWithTotalPrice = IABC.Get_FiveMostExpensiveBookings(limit);
            Assert.IsNull(topN_BookingIdWithTotalPrice, "Limit did not exceed for the request of expensive booking list.");
        }

        [TestMethod]
        public void Test_TopMostExpensiveBookingId_MeetsExpectation()
        {
            ABM.BOL.IABCBookingHelper IABC = new ABM.BOL.ABCBookingHelper();

            long booking_id = 227884;
            var top1_BookingIdWithTotalPrice = IABC.Get_FiveMostExpensiveBookings(1);
            Assert.AreEqual(booking_id.ToString(), top1_BookingIdWithTotalPrice[0].Key, "Top most expensive booking id not met to the expected booking id.");
        }

        [TestMethod]
        public void Test_TopMostExpensiveBookingPrice_MeetsExpectation()
        {
            ABM.BOL.IABCBookingHelper IABC = new ABM.BOL.ABCBookingHelper();

            decimal total_price = 6996.92M;
            var top1_BookingIdWithTotalPrice = IABC.Get_FiveMostExpensiveBookings(1);
            Assert.AreEqual(total_price, top1_BookingIdWithTotalPrice[0].Value, "Top most expensive booking price not met to the expected booking price.");
        }

        [TestMethod]
        public void Test_TotalTax_MeetsExpectation()
        {
            ABM.BOL.IABCBookingHelper IABC = new ABM.BOL.ABCBookingHelper();

            decimal expectedTotalTax = 890251.48M;
            decimal actualTotalTax = IABC.Get_TotalTax_For_TheSystem();
            Assert.AreEqual(expectedTotalTax, actualTotalTax, "Total tax not met to the expected total tax.");
        }

        [TestMethod]
        public void Test_TotalTax_IsValid()
        {
            ABM.BOL.IABCBookingHelper IABC = new ABM.BOL.ABCBookingHelper();

            decimal expectedTotalTax = 0;
            decimal actualTotalTax = IABC.Get_TotalTax_For_TheSystem();
            Assert.AreNotEqual(expectedTotalTax, actualTotalTax, "Total tax is invalid.");
        }
    }
}

