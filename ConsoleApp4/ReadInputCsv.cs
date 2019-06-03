/* **********************************************************************************************************************************
 *  Author      : Navaneeth Puthiyandi                                                                                              *
 *  Prgram Name : NavigateSimulator                                                                                                 *
 *  Class Name  : ReadInputCsv                                                                                                      *
 *  Version No  : 1.00.001                                                                                                          *
 *                                                                                                                                  *
 *  Description :   This class is capable of parsing a .csv file containing a predefined route (C:\route_example.csv) taken from the*
 *                  commandline arguements passed. The data is stored in a list for further procesing                               *
 *                                                                                                                                  *
 *                                                                                                                                  *
 *                                                                                                                                  *
 *  Updates     :                                                                                                                   *
 *                                                                                                                                  *
 * **********************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace NavigateSimulator
{

    /***************************************************************************************************************************
     Method : ReadInputCsv()

        Read all the rows in the file and store it in a STL.
     ***************************************************************************************************************************/
    public class ReadInputCsv
    {
        private static List<List<string>> ReadCsvFile(string filePath)
        {            
            List<string> Row;
            List<List<string>> myList = new List<List<string>>();
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',', StringSplitOptions.None);
                    Row = new List<string>();
                    foreach (var each in values)
                    {
                        Row.Add(each);                        
                    }

                    myList.Add(Row);
                }
            }

            return myList;
        }

        /***************************************************************************************************************************
        Method : ReadFile(
                    string filePath)

        Iterate through the List and map the values to corresponding class variables in Routinfo.

        ***************************************************************************************************************************/
        public static List<Route> ReadFile(string filePath)
        {
            List<Route> routes = new List<Route>();
            var content = ReadCsvFile(filePath);

            foreach (var eachLine in content)
            {
                if (eachLine[0] == "W" || eachLine[0] == "T")
                {
                    var route = new Route();
                    route.Type = Convert.ToChar(eachLine[0]);                    
                    route.latitude = double.Parse(eachLine[1], CultureInfo.InvariantCulture);                    
                    route.longitude = double.Parse(eachLine[2], CultureInfo.InvariantCulture);
                    route.SpeedLimit = double.Parse((eachLine[3] == string.Empty ? "0" : eachLine[3]), CultureInfo.InvariantCulture);
                    route.Altitude = double.Parse((eachLine[4] == string.Empty ? "0" : eachLine[4]), CultureInfo.InvariantCulture);
                    route.course = double.Parse((eachLine[5] == string.Empty ? "0" : eachLine[5]), CultureInfo.InvariantCulture);
                    route.Slope = double.Parse((eachLine[6] == string.Empty ? "0" : eachLine[6]), CultureInfo.InvariantCulture);
                    route.Distance = double.Parse((eachLine[7] == string.Empty ? "0" : eachLine[7]), CultureInfo.InvariantCulture);
                    route.DistanceInterval = double.Parse((eachLine[8] == string.Empty ? "0" : eachLine[8]), CultureInfo.InvariantCulture);
                    route.Name = (eachLine[9]);
                    route.Description = (eachLine[10]);
                    routes.Add(route);               
                }
            }

            var item = routes[routes.Count - 1];

            foreach (var Route in routes)
            {
                Route.Destination_Distance = (item.Distance - Route.Distance)*1000;
            }

            return routes;
        }

    }

}