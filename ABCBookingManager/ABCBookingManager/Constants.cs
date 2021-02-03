using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCBookingManager
{
    public class Constants
    {
        #region database connection string
        internal const int DATABASE_INDEX = 0;
        internal const string SERVER_IP = "127.0.0.1";
        internal const string PORT_NUMBER = "6391";
        internal const string REDIS_CONNECTION_STRING = SERVER_IP + ":" + PORT_NUMBER;
        public const string CONNECTION_MESSAGE_PASS = "Database connection passed.";
        public const string CONNECTION_MESSAGE_FAIL = "Database connection failed.";
        #endregion

        #region abc database attributes
        internal const string BOOKING = "booking:";
        internal const string PRICE_PER_NIGHT = "price_per_night";
        internal const string NIGHTS = "nights";
        internal const string CHECK_IN_DATE = "check_in_date";
        internal const string HOTEL_ID = "hotel_id";

        internal const string BOOKING_PRICES = "booking_prices";

        internal const string HOTEL_BOOKINGS = "hotel_bookings:";

        internal const string HOTEL_IDS = "hotel_ids";

        internal const string HOTELS_CHARGING_TAX = "hotels_charging_tax";
        #endregion

        #region custom exception message 
        internal const string DATABASE_CONNECTION_EXP_1 = "Error occurred while connecting to redis server!\nDetails: ";
        internal const string DATABASE_CONNECTION_EXP_2 = "Database connection error!\nDetails: ";
        internal const string DATABASE_CONNECTION_EXP_3 = "Database operation error!\nDetails: ";
        internal const string INDEX_OUTOF_RANGE_EXP = "Index out of range error!\nDetails: ";
        internal const string DATA_CONVERSION_EXP = "Data conversion error!\nDetails: ";
        internal const string ARGUMENT_EXP = "Argument error!\nDetails: ";
        public const string COMMON_EXP = "Exception Occurred!\nDetails: ";
        #endregion

        #region count
        internal const int BOOKING_ID_START_INDEX = 200000;
        internal const int BOOKING_ID_END_INDEX = 249999;
        internal const decimal TAX_RATE_IN_PERCENTAGE = 7;
        internal const decimal TAX_RATE = TAX_RATE_IN_PERCENTAGE / 100;
        internal const int LIMIT = 5;
        #endregion

        #region output message
        public const string request1 = "1) Price($) per night for booking id ";
        public const string request2 = "2) Earliest check in date for hotel id ";
        public const string request3 = "3) Five most expensive bookings (by total price), output as follows -> booking id : total price($) =";
        public const string request4 = "4) Total tax($) ABC owes on all bookings in the system = ";
        #endregion
    }
}
