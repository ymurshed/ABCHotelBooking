using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using RCM = ABCBookingManager.DAL.RedisConnectionManager;
using C = ABCBookingManager.Constants; 

namespace ABCBookingManager.DAL
{
    internal class RedisDataManager
    {
        #region helper methods for data access
        /// <summary>
        /// This method tests database connectivity and sends connection message.
        /// </summary>
        internal static string Test_DBConnection()
        {
            string connectoin_msg = C.CONNECTION_MESSAGE_PASS;
            try
            {
                var connection = RCM.RedisConnection.Ping();
                if (null != connection)
                    connectoin_msg = C.CONNECTION_MESSAGE_PASS;
                else
                    connectoin_msg = C.CONNECTION_MESSAGE_FAIL;
            }
            catch (Exception ex)
            {
                connectoin_msg = C.CONNECTION_MESSAGE_FAIL;
            }
            return connectoin_msg;
        } 

        /// <summary>
        /// This method returns price per night of a given booking id. 
        /// </summary>
        internal static RedisValue Get_PricePerNight(string booking_id)
        {
            try
            {
                var pricePerNight = RCM.RedisConnection.HashGet(C.BOOKING + booking_id, C.PRICE_PER_NIGHT);
                return pricePerNight;
            }
            catch (RedisException ex1)
            {
                throw new Exception(C.DATABASE_CONNECTION_EXP_1 + ex1.Message);
            }
            catch (Exception ex2)
            {
                throw new Exception(C.DATABASE_CONNECTION_EXP_3 + ex2.Message);
            }
        }

        /// <summary>
        /// This method returns all booking ids for a given hotel id.
        /// </summary>
        internal static RedisValue[] Get_AllBookingIds_For_HotelId(string hotel_id)
        {
            try
            {
                var allBookingIds = RCM.RedisConnection.ListRange(C.HOTEL_BOOKINGS + hotel_id, 0, -1);
                return allBookingIds;
            }
            catch (RedisException ex1)
            {
                throw new Exception(C.DATABASE_CONNECTION_EXP_1 + ex1.Message);
            }
            catch (Exception ex2)
            {
                throw new Exception(C.DATABASE_CONNECTION_EXP_3 + ex2.Message);
            }
        }

        /// <summary>
        /// This method returns all check-in date for a set of booking ids.
        /// </summary>
        internal static RedisValue[] Get_AllCheckInDate_For_BookingIds(RedisValue[] booking_ids)
        {
            try
            {
                int i = 0;
                RedisValue[] checkInDateList = new RedisValue[booking_ids.Length];

                foreach (var id in booking_ids)
                {
                    checkInDateList[i++] = RCM.RedisConnection.HashGet(C.BOOKING + id, C.CHECK_IN_DATE);
                }
                return checkInDateList;
            }
            catch (IndexOutOfRangeException ex1)
            {
                throw new Exception(C.INDEX_OUTOF_RANGE_EXP + ex1.Message);
            }
            catch (RedisException ex2)
            {
                throw new Exception(C.DATABASE_CONNECTION_EXP_1 + ex2.Message);
            }
            catch (Exception ex3)
            {
                throw new Exception(C.DATABASE_CONNECTION_EXP_3 + ex3.Message);
            }
        }

        /// <summary>
        /// This method returns all booking ids from the given dump database.
        /// </summary>
        internal static List<string> Get_AllBookingIds()
        {
            List<string> allBookingIds = new List<string>();

            for (int id = C.BOOKING_ID_START_INDEX; id <= C.BOOKING_ID_END_INDEX; id++)
            {
                allBookingIds.Add(id.ToString());
            }
            return allBookingIds;
        }

        /// <summary>
        /// This method returns a dictonary containing the total price of each booking id along with booking id for a set of booking ids.
        /// </summary>
        internal static Dictionary<string, decimal> Get_TotalPrice_For_BookingIds(List<string> allBookingIds)
        {
            try
            {
                Dictionary<string, decimal> bookingIdWithTotalPrice = new Dictionary<string, decimal>();

                foreach (var bookingId in allBookingIds)
                {
                    decimal totalPrice = 1;
                    var allKeys = RCM.RedisConnection.HashGetAll(C.BOOKING + bookingId);

                    foreach (var k in allKeys)
                    {
                        if (k.Name.Equals(C.PRICE_PER_NIGHT) || k.Name.Equals(C.NIGHTS))
                            totalPrice *= Convert.ToDecimal(k.Value);
                    }

                    bookingIdWithTotalPrice.Add(bookingId, totalPrice);
                }
                return bookingIdWithTotalPrice;
            }
            catch (FormatException ex1)
            {
                throw new Exception(C.DATA_CONVERSION_EXP + ex1.Message);
            }
            catch (RedisException ex2)
            {
                throw new Exception(C.DATABASE_CONNECTION_EXP_1 + ex2.Message);
            }
            catch (Exception ex3)
            {
                throw new Exception(C.DATABASE_CONNECTION_EXP_3 + ex3.Message);
            }
        }

        /// <summary>
        /// This method returns all hotel ids for taxable hotels.
        /// </summary>
        internal static RedisValue[] Get_HotelIds_For_TaxableHotels()
        {
            try
            {
                var taxableHotelIds = RCM.RedisConnection.SetMembers(C.HOTELS_CHARGING_TAX);
                return taxableHotelIds;
            }
            catch (RedisException ex1)
            {
                throw new Exception(C.DATABASE_CONNECTION_EXP_1 + ex1.Message);
            }
            catch (Exception ex2)
            {
                throw new Exception(C.DATABASE_CONNECTION_EXP_3 + ex2.Message);
            }
        }

        /// <summary>
        /// This method converts redis list to string list.
        /// </summary>
        internal static List<string> Convert_RedisList_To_StringList(RedisValue[] redis_list)
        {
            try
            {
                List<string> str_list = new List<string>();

                foreach (var rl in redis_list)
                {
                    str_list.Add(rl.ToString());
                }
                return str_list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
