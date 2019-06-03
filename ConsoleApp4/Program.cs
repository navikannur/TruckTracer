/* **********************************************************************************************************************************
 *  Author      : Navaneeth Puthiyandi                                                                                              *
 *  Prgram Name : Program                                                                                                 *
 *  Version No  : 1.00.001                                                                                                          *
 *                                                                                                                                  *
 *  Description :   A simple vehicle simulator capable of parsing a .csv file containing a predefined route (route_example.csv)     *
 *                  and have the simulated vehicle trace out this route in a realistic way. The route contains information about    *
 *                  coordinates, headings, slopes and speed limits.                                                                 *
 *                                                                                                                                  *
 *  Program                                                                                                                         *
 *  arguements  :  <Filename with extension and full path> <custom delay> <debugmode Y or N>                                        *
 *                                                                                                                                  *
 *  Updates     :                                                                                                                   *
 *                                                                                                                                  *
 * **********************************************************************************************************************************/
 
using System;
using System.IO;

namespace NavigateSimulator
{
    class Program
    {
        /***************************************************************************************************************************
        Main Method, setting default arguements values. Could be changed to config file. couldn't find the assembly dll 
        in my Visual studio, Hence default value is set inside main menu method.
        ***************************************************************************************************************************/
        static void Main(string[] args)
        {
            bool debug = true;
            int Delay_in_Seconds = 1;

            if (args.Length == 0)
            {
                // args[0] does not exist because it is an empty array.
                // assign it with an new array of string instead.
                Console.WriteLine("Enter mandatory parameter : Filename\n");
                args = new string[] { Console.ReadLine(), };
            }

            //Delay in seconds
            if (args.Length > 1)
            {
                Delay_in_Seconds = Convert.ToInt32(args[1]);
            }

            //Disable Debug print outs 
            if (args.Length > 2)
            {
                if (args[2].ToLower() == "n")
                    debug = false;
            }

            // validate if the file exist
            if (File.Exists(args[0]))
            {
                var navigate = new Navigation(args[0], Delay_in_Seconds, debug);
                navigate.Run();
            }
            else
            {
                Console.WriteLine("File does not exist\n");
            }
        }
    }
}
