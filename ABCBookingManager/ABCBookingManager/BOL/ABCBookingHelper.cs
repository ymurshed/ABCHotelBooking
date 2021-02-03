using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDM = ABCBookingManager.DAL.RedisDataManager;
using C = ABCBookingManager.Constants; 

namespace ABCBookingManager.BOL
{
    public class ABCBookingHelper : IABCBookingHelper
    {
        #region database connection test method
        /// <summary>
        /// This method is for testing database connectivity.
        /// </summary>
        public string Test_DBConnection()
        {
            return RDM.Test_DBConnection();
        }
        #endregion

        #region actual request
        /// <summary>
        /// This method returns price per night of a given booking id. 
        /// </summary>
        public decimal Get_PricePerNight_For_BookingId(long booking_id)
        {
            try
            {
                var pricePerNight = RDM.Get_PricePerNight(booking_id.ToString());
                return Convert.ToDecimal(pricePerNight);
            }
            catch (FormatException ex1)
            {
                throw new Exception(C.DATA_CONVERSION_EXP + ex1.Message);
            }
            catch (Exception ex2)
            {
                throw new Exception(ex2.Message);
            }
        }

        /// <summary>
        /// This method returns earliest check-in date for a given hotel id.
        /// </summary>
        public string Get_Earliest_CheckInDate_For_HotelId(long hotel_id)
        {
            try
            {
                var earliestCheckInDate = string.Empty;
                var allBookingIds = RDM.Get_AllBookingIds_For_HotelId(hotel_id.ToString());

                if (null != allBookingIds && allBookingIds.Count() > 0)
                {
                    var allCheckInDate = RDM.Get_AllCheckInDate_For_BookingIds(allBookingIds);
                    if (null != allCheckInDate && allCheckInDate.Count() > 0)
                    {
                        earliestCheckInDate = allCheckInDate.Max().ToString();
                    }
                }
                return earliestCheckInDate;
            }
            catch (FormatException ex1)
            {
                throw new Exception(C.DATA_CONVERSION_EXP + ex1.Message);
            }
            catch (Exception ex2)
            {
                throw new Exception(ex2.Message);
            }
        }

        /// <summary>
        /// This method returns top n (eg. n = 5) expensive bookings.
        /// </summary>
        public List<KeyValuePair<string, decimal>> Get_FiveMostExpensiveBookings(int topN = C.LIMIT)
        {
            try
            {
                Dictionary<string, decimal> bookingIdWithTotalPrice = RDM.Get_TotalPrice_For_BookingIds(RDM.Get_AllBookingIds());

                if (null != bookingIdWithTotalPrice && bookingIdWithTotalPrice.Count() >= topN)
                {
                    var topN_BookingIdWithTotalPrice = (from dic in bookingIdWithTotalPrice orderby dic.Value descending select dic).Take(topN).ToList();
                    return topN_BookingIdWithTotalPrice;
                }
                else
                {
                    return null;
                }
            }
            catch (FormatException ex1)
            {
                throw new Exception(C.DATA_CONVERSION_EXP + ex1.Message);
            }
            catch (Exception ex2)
            {
                throw new Exception(ex2.Message);
            }
        }

        /// <summary>
        /// This method returns total tax ABC owes in the system.
        /// </summary>
        public decimal Get_TotalTax_For_TheSystem()
        {
            try
            {
                decimal totalTax = 0;
                List<string> bookingIdList = new List<string>();
                Dictionary<string, decimal> bookingIdWithTotalPrice = new Dictionary<string, decimal>();

                var hotel_ids = RDM.Get_HotelIds_For_TaxableHotels();

                foreach (var hotel in hotel_ids)
                {
                    var booking_ids = RDM.Get_AllBookingIds_For_HotelId(hotel);
                    bookingIdList = RDM.Convert_RedisList_To_StringList(booking_ids);

                    bookingIdWithTotalPrice = RDM.Get_TotalPrice_For_BookingIds(bookingIdList);

                    foreach (var eachTotalPrice in bookingIdWithTotalPrice)
                    {
                        decimal eachTax = Math.Round((eachTotalPrice.Value * C.TAX_RATE), 2, MidpointRounding.AwayFromZero);
                        totalTax += eachTax;
                    }

                    bookingIdList.Clear();
                    bookingIdWithTotalPrice.Clear();
                }
                return totalTax;
            }
            catch (ArgumentException ex1)
            {
                throw new Exception(C.ARGUMENT_EXP + ex1.Message);
            }
            catch (Exception ex2)
            {
                throw new Exception(ex2.Message);
            }
        }
        #endregion
    }
}
