using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABM = ABCBookingManager;

namespace ABCBookingManagerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string errorMsg = string.Empty;

            try
            {
                ABM.BOL.IABCBookingHelper IABC = new ABM.BOL.ABCBookingHelper();
                
                // (1) Get price per night
                long booking_id = 200123;
                var pricePerNight = IABC.Get_PricePerNight_For_BookingId(booking_id);
                Console.WriteLine(ABM.Constants.request1 + booking_id + " = " + pricePerNight);

                // (2) Get earliest check-in date
                long hotel_id = 1042;
                var earliestCheckInDate = IABC.Get_Earliest_CheckInDate_For_HotelId(hotel_id);
                Console.WriteLine(ABM.Constants.request2 + hotel_id + " = " + earliestCheckInDate);

                // (3) Get top 5 expensive bookings
                var top5_BookingIdWithTotalPrice = IABC.Get_FiveMostExpensiveBookings();
                Console.WriteLine(ABM.Constants.request3);
                foreach (var item in top5_BookingIdWithTotalPrice)
                {
                    Console.WriteLine(item.Key + " : " + item.Value);
                }

                // (4) Get total tax ABC owes
                decimal totalTax = IABC.Get_TotalTax_For_TheSystem();
                Console.WriteLine(ABM.Constants.request4 + totalTax);
            }
            catch (Exception ex)
            {
                errorMsg = ABM.Constants.COMMON_EXP + ex.Message;
            }
            finally
            {
                Console.WriteLine(errorMsg);
            }
            Console.ReadLine();
        }
    }
}
