/* **********************************************************************************************************************************
 *  Author      : Navaneeth Puthiyandi                                                                                              *
 *  Prgram Name : Navigation                                                                                                 *
 *  Version No  : 1.00.001                                                                                                          *
 *                                                                                                                                  *
 *  Description :   A simple vehicle simulator capable of parsing a .csv file containing a predefined route (route_example.csv)     *
 *                  and have the simulated vehicle trace out this route in a realistic way. The route contains information about    *
 *                  coordinates, headings, slopes and speed limits.                                                                 *
 *                                                                                                                                  *
 *  Program                                                                                                                         *
 *  arguements  :  <Filename with extension and full path> <custom delay> <debugmode Y or N>                                        *
 *                                                                                                                                  *
 *  Updates :                                                                                                                       *
 *                                                                                                                                  *
 * **********************************************************************************************************************************/

using System;
using System.Collections.Generic;

namespace NavigateSimulator
{
    public class Navigation
    {
        // const double Acc_unit_convertor = 12960; // To convert m/s^2 to  Km/h^2
        // const double Accelerating = 1.5 * Acc_unit_convertor; // In  Km/h^2
        // const double Braking = -2.5 * Acc_unit_convertor; // In  Km/h^2

        const double Accelerating = 1.5; // In  m/s^2
        const double Braking = -2.5; // In  m/s^2
        int Delay = 1;
        bool Debug = false;
        List<Route> Vectors;

       /*************************************************************************************************************************************
       * Constructor to initiate the read method and to set the delay value.
       **************************************************************************************************************************************/
        public Navigation(string routePath, int Delay_in_Seconds, bool debug)
        {
            Vectors = ReadInputCsv.ReadFile(routePath);
            Debug = debug;
            Delay = Delay_in_Seconds;
        }

        /* ************************************************************************************************************************************
        * Method  : public void Run()
        *              
        * This method is used the core navigation process for accelartion and braking scenarios. The calculation is done in meters and seconds
        * which is later converted to kilometer and hours.
        * 
        * The time interval between the records in csv is calculated . This is useful when the delay is lesser or larger than the calculated 
        * intervals , the location data is repeated on regular intervals incase delay is shorter than intervals. If delay is very large than 
        * the time interval then it is skipped untill the accumulated time interval is over the delay.
        * 
        * Debug Mode: As the simulation is not working there is print statements of the records which will be available on running in Debug '
        * mode. (Last parameter on command line set to 'Y')
        **************************************************************************************************************************************/
        public void Run()
        {
            double prevSpeed = 0; // in Km/H
            double prevDistance = 0; // In Km
            var httpPoster = new PostRequest("http://localhost:5000");
            double previous_timeinterval = 0;
            int time_loops = 0;
            //double distance_interval = 0;
            double time_interval = 0;

            foreach (var eachVector in Vectors)
            {
                // Assuming only routes with type T should be considered.
                if (eachVector.Type == 'T')
                {

                    double current_Speed = prevSpeed;
                    //distance_interval = eachVector.Distance - prevDistance;

                    if (prevSpeed < (eachVector.SpeedLimit * 0.277777)) // Initiate Acceleration
                    {
                        // Calculate new speed if Acceleration is applied
                        current_Speed = FinalSpeed(prevSpeed, Accelerating, eachVector.DistanceInterval);

                        // If current speed is greater than permitted rate then limit the speed to the permitted limit
                        if (current_Speed > (eachVector.SpeedLimit*0.277777))
                            current_Speed = (eachVector.SpeedLimit * 0.277777);

                        /*------------------------------------------------------------------------------------
                         Note Run_1.0 .
                         previous row was used : initialize the previous timeinterval
                         Else set the current one to previous before calculating new interval.
                         This can be used to accumulate the time intervals to match with delay.
                         ------------------------------------------------------------------------------------*/
                        if (time_loops != 0) 
                            previous_timeinterval = 0;
                        else
                            previous_timeinterval = time_interval;

                        // new time interval is calculated
                        time_interval = TimeTaken_seconds(current_Speed, prevSpeed, Accelerating, previous_timeinterval);


                    }
                    else if (prevSpeed > (eachVector.SpeedLimit * 0.277777)) // Initiate Braking
                    {
                        // Calculate new speed if braking is applied
                        current_Speed = FinalSpeed(prevSpeed, Braking, eachVector.DistanceInterval);

                        // If current speed is lesser than permitted rate then adjust the speed to the permitted limit
                        if (current_Speed < (eachVector.SpeedLimit * 0.277777))
                            current_Speed = (eachVector.SpeedLimit * 0.277777);

                        /*---------------------
                         Refer Note Run_1.0 . 
                         --------------------*/
                        if (time_loops != 0)  
                            previous_timeinterval = 0;
                        else
                            previous_timeinterval = time_interval;
                        
                        // new time interval is calculated
                        time_interval = TimeTaken_seconds(current_Speed, prevSpeed, Braking, previous_timeinterval);
                    }


                    /*-------------------------------------------------------------------------------------------------------------------------
                    Check if the time interval is too short or too large for the delay. 
                    if there are intervals is same as delay      :  print once 
                    if there are intervals are larger than delay :  Same geo cordinates are delivered to http on regular delays.
                    if the intervals are smaller than the delay  :  The intervals are accumulated and posted when it cross the delay period. 
                    -------------------------------------------------------------------------------------------------------------------------*/
                    time_loops = ((time_interval < (Delay)) ? 0 : ((int)(time_interval / (Delay))));

                    if(time_loops==1)
                    {
                        httpPoster.Push(CreateVector(eachVector, (current_Speed * 3.6)));
                        prevSpeed = current_Speed;
                        prevDistance = eachVector.Distance;

                        // Debug Mode will print the values on standard output.
                        if (Debug)
                        {
                            Console.WriteLine(
                                "{ Latitude: " + eachVector.latitude +
                                "\tLongitude: " + eachVector.longitude +
                                "\tCourse: " + eachVector.course +
                                "\tTruck Speed: " + (current_Speed * 3.6) +
                                "\tSpeedLimit: " + eachVector.SpeedLimit +
                                "}\n"
                                );
                        }

                        System.Threading.Thread.Sleep(Delay * 1000);
                    }
                    else
                    {
                        for (int i = 0; i < time_loops; i++)
                        {
                            if (prevSpeed < (eachVector.SpeedLimit * 0.277777)) // Initiate Acceleration
                            {
                                // Calculate new speed if Acceleration is applied
                                current_Speed = FinalSpeed(prevSpeed, Accelerating, eachVector.DistanceInterval);

                                // If current speed is greater than permitted rate then limit the speed to the permitted limit
                                if (current_Speed > (eachVector.SpeedLimit * 0.277777))
                                    current_Speed = (eachVector.SpeedLimit * 0.277777);

                                /*------------------------------------------------------------------------------------
                                 Note Run_1.0 .
                                 previous row was used : initialize the previous timeinterval
                                 Else set the current one to previous before calculating new interval.
                                 This can be used to accumulate the time intervals to match with delay.
                                 ------------------------------------------------------------------------------------*/
                                if (time_loops != 0)
                                    previous_timeinterval = 0;
                                else
                                    previous_timeinterval = time_interval;

                                // new time interval is calculated
                                time_interval = TimeTaken_seconds(current_Speed, prevSpeed, Accelerating, previous_timeinterval);


                            }
                            else if (prevSpeed > (eachVector.SpeedLimit * 0.277777)) // Initiate Braking
                            {
                                // Calculate new speed if braking is applied
                                current_Speed = FinalSpeed(prevSpeed, Braking, eachVector.DistanceInterval);

                                // If current speed is lesser than permitted rate then adjust the speed to the permitted limit
                                if (current_Speed < (eachVector.SpeedLimit * 0.277777))
                                    current_Speed = (eachVector.SpeedLimit * 0.277777);

                                /*---------------------
                                 Refer Note Run_1.0 . 
                                 --------------------*/
                                if (time_loops != 0)
                                    previous_timeinterval = 0;
                                else
                                    previous_timeinterval = time_interval;

                                // new time interval is calculated
                                time_interval = TimeTaken_seconds(current_Speed, prevSpeed, Braking, previous_timeinterval);
                            }

                            httpPoster.Push(CreateVector(eachVector, (current_Speed * 3.6)));
                            prevSpeed = current_Speed;
                            prevDistance = eachVector.Distance;

                            // Debug Mode will print the values on standard output.
                            if (Debug)
                            {
                                Console.WriteLine(
                                    "{ Latitude: " + eachVector.latitude +
                                    "\tLongitude: " + eachVector.longitude +
                                    "\tCourse: " + eachVector.course +
                                    "\tTruck Speed: " + (current_Speed * 3.6) +
                                    "\tSpeedLimit: " + eachVector.SpeedLimit +
                                    "}\n"
                                    );
                            }
                            System.Threading.Thread.Sleep(Delay * 1000);
                        }
                    }
                        
                    
                }
            }
        }


        /* **********************************************************************************************************************************
        * Method  : private RouteInfo CreateVector(
        *               Route currentVector, 
        *               double finalSpeed)
        *              
        * This method is used to createVector for the cordinates which need to be posted from the csv file
        ***********************************************************************************************************************************/
        private RouteInfo CreateVector(Route currentVector, double current_Speed)
        {
            RouteInfo vector = new RouteInfo();
            vector.latitude = currentVector.latitude;
            vector.longitude = currentVector.longitude;
            vector.course = currentVector.course;
            vector.speed = current_Speed;
            return vector;
        }


        /* **********************************************************************************************************************************
         * Method  : private double FinalSpeed(
         *              double initialSpeed, 
         *              double acceleration, 
         *              double distance)
         *              
         * This method is used to calculate the velocity of the vehicle with help of the initial velocity , acceleration and distance.
         * The equation used to calculate velocity is  =
         * V^2 = U^2 + 2as
         * We take the square root of the above equation.
         ***********************************************************************************************************************************/
        private double FinalSpeed(double initialSpeed, double acceleration, double distance)
        {
            double final = Math.Pow(initialSpeed, 2) + (2 * acceleration * distance);
            return Math.Sqrt(final);
        }


        /* **********************************************************************************************************************************
         * Method  : private double TimeTaken_seconds(
         *              double current_Speed , 
         *              double initialSpeed, 
         *              double acceleration, 
         *              double previous_timeinterval)
         * 
         * This method is used to calculate the time taken between two records in the csv file. This is helpful to post the data on regular intervals.
         * if the time taken is less than the delay. We can accumulate it until the delay time is crossed. Otherwise if the csv contains multiple duplicate 
         * records we will be posting the results uneccessarily. Hence short interval data can be skipped. Also when there is a large distance between the rows
         * you can post the geo locations on regular intervals. 
         * 
         * (This function can be useful when speed need be updated in such cases (time interval less or greater than the delay) - but not done yet)
         ***********************************************************************************************************************************/
        private double TimeTaken_seconds(double current_Speed , double initialSpeed, double acceleration, double previous_timeinterval)
        {
            double time_elapsed = (((current_Speed - initialSpeed) / acceleration) + previous_timeinterval);
            return time_elapsed;
        }
    }
}
