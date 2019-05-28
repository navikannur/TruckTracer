using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace NavigateSimulator
{
    public class HttpPost
    {

        private string Url = "http://0.0.0.0:5000";

        //static string JsonContent
        //{
        //    get
        //    {
        //        var routeContent = new RouteInfo();
        //        routeContent.latitude = 59.16006;
        //        routeContent.longitude = 17.64528;
        //        routeContent.course = 23.56;
        //        routeContent.speed = 76.4;

        //        return JsonConvert.SerializeObject(routeContent);
        //    }
        //}

        public HttpPost(string url)
        {
            Url = url;
        }

        public void Push(RouteInfo vector)
        {
            string jsonPost = JsonCreater(vector);
            test(jsonPost);
            //test2(jsonPost);
            //test3(jsonPost);
        }

        private void test(string jsonContent)
        {
            HttpClient client = new HttpClient();
            var json = new Dictionary<string, string>()
            {  {"\"latitude\"", "59.16006" },
               { "\"longitude\"", "17.64528"},
                {"\"course\"","23.56" },
                {"\"speed\"", "76.4" }
                              };

            var content = new StringContent(jsonContent);
            //var content = new FormUrlEncodedContent(json);
            var response = client.PostAsync(Url, content);

            var responseString = response.Status;// ReadAsStringAsync();
        }

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

    }

}