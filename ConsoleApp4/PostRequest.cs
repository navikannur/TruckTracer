/* **********************************************************************************************************************************
 *  Author      : Navaneeth Puthiyandi                                                                                  
 *  Prgram Name : PostRequest                                                                                              
 *  Class Name  : RouteInfo                                                                                             
 *  Version No  : 1.00.001                                                                                              
 *                                                                                                                      
 *  Description :   This class is used for post the data to http using POST request.
 *                  files                                                                                               
 *                                                                                                                      
 *  Issues      : The post request is not working, despite of trying out multiple options, Guess simulator is not responding.
 *                For this reason, I am printing the post data in the standard output screen. The option is on by default.
 *                But if the codinates need not be printed on screen then you can use the debug = 'N' option in the program
 *                arguement.
 *  Updates     :                                                                                                       
 *                                                                                                                      
 * **********************************************************************************************************************************/
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

using System;
using System.Linq;

namespace NavigateSimulator
{
    class PostRequest
    {
        private string Url = "http://0.0.0.0:5000";
        private string status;

        public PostRequest(string url)
        {
            Url = url;
        }

        public String Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public void Push(RouteInfo vector)
        {
            //string jsonPost = JsonCreater(vector);

            double dlatitude = Math.Round(vector.latitude, 6);
            double dlongitude = Math.Round(vector.longitude, 6);
            double dcourse = Math.Round(vector.course, 2);
            double dspeed = Math.Round(vector.speed, 2);

            string jsonPost = "{\"latitude\": " + dlatitude +
                        ",\"longitude\": " + dlongitude +
                        ",\"course\": " + dcourse +
                        ",\"speed\": " + dspeed + "}";

            httppost_test(jsonPost);

            // string responsender = httppost_test(jsonPost);

            //test(jsonPost);
            //test2(jsonPost);
            //test3(jsonPost);
        }
        private void test(string jsonContent)
        {
            HttpClient client = new HttpClient();

            /*     var json = new Dictionary<string, string>()
            {  {"\"latitude\"", "59.16006" },
               { "\"longitude\"", "17.64528"},
                {"\"course\"","23.56" },
                {"\"speed\"", "76.4" }
                              }; */

            //var content = new FormUrlEncodedContent(json);

            var content = new StringContent(jsonContent);
            var response = client.PostAsync(Url, content);
            var responseString = response.Status; // ReadAsStringAsync();

        }

        private string JsonCreater(double lat, double lon, double cou, double spe)
        {
            var routeContent = new RouteInfo();
            routeContent.latitude = lat;
            routeContent.longitude = lon;
            routeContent.course = cou;
            routeContent.speed = spe;

            return JsonConvert.SerializeObject(routeContent);
        }


        private string JsonCreater(RouteInfo routeContent)
        {
            return JsonConvert.SerializeObject(routeContent);
        }

        public void httppost_test(string jsonPost)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                streamWriter.Write(jsonPost);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }

            }
            catch (WebException e)
            {
                Console.WriteLine("***********************************************************\n");
                Console.WriteLine("WebMapApp : Not responding, Please check if it is been run \n");
                Console.WriteLine("***********************************************************\n");
            }

        }

        
        /*
             private void test2(string jsonContent)
             {
                 HttpClient client = new HttpClient();

                 var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                 var response = client.PostAsync(Url, httpContent);

                 // var responseString = response.ToString();// ReadAsStringAsync();
                 // return responseString;
             }

             private void test3(string jsonContent)
             {
                 var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                 httpWebRequest.ContentType = "application/json";
                 httpWebRequest.Method = "POST";
                 using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                 {
                     streamWriter.Write(jsonContent);
                     streamWriter.Flush();
                     streamWriter.Close();
                 }
                 var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
             } 


        private void httppost_test(string jsonContent)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonContent);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }*/

        /*
            public string httppost_test(string jsonPost)
            {
            // Create a request using a URL that can receive a post.

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);

            httpWebRequest.Method = "POST";

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonPost);

            // Set the ContentType property of the WebRequest.
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            // Set the ContentLength property of the WebRequest.
            httpWebRequest.ContentLength = byteArray.Length;

            // Get the request stream.
            dataStream = httpWebRequest.GetRequestStream();

            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);

            // Close the Stream object.
            dataStream.Close();

            // Get the original response.
            WebResponse response = httpWebRequest.GetResponse();

            this.Status = ((HttpWebResponse)response).StatusDescription;

            // Get the stream containing all content returned by the requested server.
            dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            // Read the content fully up to the end.
            string responseFromServer = reader.ReadToEnd();

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
            }*/





    }
}


