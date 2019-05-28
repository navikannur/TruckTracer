using System;
using System.Collections.Generic;

namespace NavigateSimulator
{
    public class Navigation
    {
        const double Acc_unit_convertor = 12960; // To convert m/s^2 to  Km/h^2
        const double Accelerating = 1.5 * Acc_unit_convertor; // In  Km/h^2
        const double Braking = -2.5 * Acc_unit_convertor; // In  Km/h^2

        int Delay = 1000;
        bool Debug = false;
        List<Route> Vectors;
        public Navigation(string routePath, int delay, bool debug)
        {
            Vectors = ReadInputCsv.ReadFile(routePath);
            Debug = debug;
            Delay = delay;
        }

        public void Run()
        {
            double prevSpeed = 0; // in Km/H
            double prevDistance = 0; // In Km
            var httpPoster = new HttpPost("http://localhost:5000");

            foreach (var eachVector in Vectors)
            {
                // Assuming only routes with type T should be considered.
                if (eachVector.Type == 'T')
                {

                    double finalSpeed = prevSpeed;
                    if (prevSpeed < eachVector.SpeedLimit) // Acceleration
                    {
                        finalSpeed = FinalSpeed(prevSpeed, Accelerating, eachVector.Distance - prevDistance);
                        if (finalSpeed > eachVector.SpeedLimit)
                            finalSpeed = eachVector.SpeedLimit;
                    }
                    else if (prevSpeed > eachVector.SpeedLimit)// Braking
                    {
                        finalSpeed = FinalSpeed(prevSpeed, Braking, eachVector.Distance - prevDistance);
                        if (finalSpeed < eachVector.SpeedLimit)
                            finalSpeed = eachVector.SpeedLimit;
                    }

                    double time_interval = TimeTaken_seconds(finalSpeed, eachVector.DistanceInterval);


                    int time_loops = ((time_interval < Delay) ? 0 : ((int)(time_interval / Delay)));



                    for (int i=0; i<time_loops; i++)
                    {
                        httpPoster.Push(CreateVector(eachVector, finalSpeed));
                        prevSpeed = finalSpeed;
                        prevDistance = eachVector.Distance;

                        if (Debug) // Debug Mode
                        {
                            Console.WriteLine(
                                "> Latitude: " + eachVector.latitude + ", " +
                                "Longitude: " + eachVector.longitude + ", " +
                                "Course: " + eachVector.course + ", " +
                                "Speed: " + finalSpeed + ", " +
                                "  SpeedLimit: " + eachVector.SpeedLimit
                                );
                        }

                        System.Threading.Thread.Sleep(Delay);
                    }    
                    
                }
            }
        }

        private RouteInfo CreateVector(Route currentVector, double finalSpeed)
        {
            RouteInfo vector = new RouteInfo();
            vector.latitude = currentVector.latitude;
            vector.longitude = currentVector.longitude;
            vector.course = currentVector.course;
            return vector;
        }


        /// <summary>
        /// Calculate final Speed, using final problem.
        /// V^2 = U^2 + 2as
        /// </summary>
        /// <param name="initialSpeed"></param>
        /// <param name="acceleration"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private double FinalSpeed(double initialSpeed, double acceleration, double distance)
        {
            double final = Math.Pow(initialSpeed, 2) + (2 * acceleration * distance);
            return Math.Sqrt(final);
        }

        private double TimeTaken_seconds(double velocity , double distance_interval)
        {
            if (velocity == 0 || distance_interval == 0)
                return 0;
            else
                return ((distance_interval / velocity)*3600);
        }
    }
}
