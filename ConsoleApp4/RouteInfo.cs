/* **********************************************************************************************************************************
 *  Author      : Navaneeth Puthiyandi                                                                                              *
 *  Prgram Name : NavigateSimulator                                                                                                 *
 *  Class Name  : RouteInfo                                                                                                         *
 *  Version No  : 1.00.001                                                                                                          *
 *                                                                                                                                  *
 *  Description :   This class is used for getter and setter of various entities. These variables are later refered in other class  *
 *                  files                                                                                                           *
 *                                                                                                                                  *
 *                                                                                                                                  *
 *                                                                                                                                  *
 *  Updates     :                                                                                                                   *
 *                                                                                                                                  *
 * **********************************************************************************************************************************/
namespace NavigateSimulator
{
    public class RouteInfo
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double course { get; set; }
        public double speed { get; set; }
    }


    public class Route : RouteInfo
    {
        public char Type { get; set; }
        public double Altitude { get; set; }
        public double Slope { get; set; }
        public double SpeedLimit { get; set; }
        public double Distance { get; set; }
        public double DistanceInterval { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Destination_Distance { get; set; }
    }

    /*
    public class StopInfo : RouteInfo
    {
        public double DistanceBalance { get; set; }
        public double TimeBalance { get; set; }
    } 
    */
}