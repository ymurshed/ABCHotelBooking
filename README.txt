Introduction 
============= 
In this exercise you will use Redis to do some simple analytics on a snapshot of hotel booking data. Redis is an in-memory data structure server that is fast and easy to use.
 
Set up
=========  
1. Download the latest stable version of Redis (3.2) from http://redis.io/download . Windows binaries can be found at https://github.com/MSOpenTech/redis/releases  
2. Decide which programming language you want to use to solve this problem and find a suitable client at http://redis.io/clients . If you are using C#, we recommend you do not use the ServiceStack client as it has a limit on the number of queries. 
3. If you’ve not used Redis before, check out the interactive tutorial at http://try.redis.io/  and the introduction to Redis data types at http://redis.io/topics/data-types-intro . 
4. Unzip the supplied database (which I have provided separately) and cd to that directory 
5. Start the server by running redis-server redis.conf in that directory. The configuration file will run the server in the foreground and listen on port 6391. 
6. In another command shell, run the CLI client with redis-cli -p 6391 
7. In the client, type the command hgetall booking:200000 . 
If everything worked, you should get a list of keys and values. 
 
Data store structure 
======================
 
The supplied data store models a simulated snapshot of the last 50,000 hotel bookings we have received. 
 A booking has a unique integer ID called the booking_id. 
 Each booking is for a particular hotel, referred to using the hotel_id. 
 A hotel can have zero, one or more bookings in the system. 
 Each booking has a check in date, a price per night excluding tax and the number of nights stayed. 
 Prices are in US$ and the rounding convention is to round to the nearest cent ($0.01), round up half-cents. 
 For example $1.2310 -> $1.23, $4.5650 -> $4.57.  The total price of the booking is simply the price per night multiplied by the number of nights stayed. 
 
The supplied database has the following structures 
 hotel_ids – a set containing all the hotel IDs known by the system 
 hotels_charging_tax – a set containing all the hotel IDs from hotel_ids for which we have to pay tax 
 hotel_bookings:HOTEL_ID – each of these is a list of booking IDs for the given hotel ID. For example, the key hotel_bookings:1077 contains a list of all booking IDs for hotel ID 1077. 
 booking:BOOKING_ID – each of these is a hash containing the following keys: o hotel_id o price_per_night o check_in_date o nights 
 booking_prices – a sorted set where the score is the total price of the booking and the members are booking IDs  
 
Use the Redis CLI client to explore the data store. For example, below I use the CLI to find out how many bookings there are for hotel ID 1065 by printing the length of the list hotel_booking:1065. In the second command, I find the price per night for booking ID 200456. Note that you may get different values when you try this on your copy of the database. 
 
127.0.0.1:6391> llen hotel_bookings:1065 (integer) 623 127.0.0.1:6391> hget booking:200456 price_per_night "1294" 
 
The Actual Problem 
====================
 
Using the redis supplied database (dump.rdb), write a program that will do the following 
 
1. Print the price per night for booking id 200123. 
2. Print the earliest check in date for hotel ID 1042. 
3. Find the five most expensive bookings (by total price, not price per night) and print the booking IDs and total price for each, in descending order of total price. 
4. Calculate how much tax we owe on all bookings in the system. Assume that the tax rate is 7% and apply this to the total price (using the rounding conventions) for each booking for each hotel that charges tax, then sum these up. Note that we want to see the tax amount, not the booking price + tax. 