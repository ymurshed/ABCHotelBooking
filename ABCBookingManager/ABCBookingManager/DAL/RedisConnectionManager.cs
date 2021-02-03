using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using C = ABCBookingManager.Constants;

namespace ABCBookingManager.DAL
{
    internal class RedisConnectionManager
    {
        /// <summary>
        /// Property for holding database connection object.
        /// </summary>
        private static IDatabase redisConnection = null;
        public static IDatabase RedisConnection
        {
            get
            {
                if (null == redisConnection)
                {
                    redisConnection = CreateRedisConnection();
                }
                return redisConnection;
            }
        }

        /// <summary>
        /// This method creates redis server and database connection and returns connection object. 
        /// </summary>
        private static IDatabase CreateRedisConnection()
        {
            try
            {
                ConnectionMultiplexer redis = null;
                IDatabase db0 = null;

                redis = ConnectionMultiplexer.Connect(C.REDIS_CONNECTION_STRING);
                db0 = redis.GetDatabase(C.DATABASE_INDEX);
                return db0;
            }
            catch (RedisException ex1)
            {
                throw new Exception(C.DATABASE_CONNECTION_EXP_1 + ex1.Message);
            }
            catch (Exception ex2)
            {
                throw new Exception(C.DATABASE_CONNECTION_EXP_2 + ex2.Message);
            }
        }
    }
}
