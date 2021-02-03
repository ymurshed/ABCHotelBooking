using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = ABCBookingManager.Constants;

namespace ABCBookingManager.BOL
{
    public interface IABCBookingHelper
    {
        string Test_DBConnection();

        decimal Get_PricePerNight_For_BookingId(long booking_id);

        string Get_Earliest_CheckInDate_For_HotelId(long hotel_id);

        List<KeyValuePair<string, decimal>> Get_FiveMostExpensiveBookings(int topN = C.LIMIT);

        decimal Get_TotalTax_For_TheSystem();
    }
}
