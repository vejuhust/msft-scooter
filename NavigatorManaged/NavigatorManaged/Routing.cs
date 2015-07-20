using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BingMapsRESTService.Common.JSON;
using System.Runtime.Serialization.Json;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace NavigatorManaged
{
    public class Routing
    {
        private readonly string key = "As-Xme5h3WPlxHWR8cuncv1xPVLwVr-g0SNUgdBKAAz3H9krqehnKiPosfJBsRcD";

        //public string GetCurrentLocation(string latitude, string longitude)
        //{
        //    string currentLocation = null;
        //    if (!String.IsNullOrEmpty(latitude) && !String.IsNullOrEmpty(longitude))
        //    {
        //        Uri currentLocationURI = new Uri(string.Format("http://dev.ditu.live-int.com/REST/v1/Locations/{0},{1}?key={2}", latitude, longitude, key));
        //        GetURIResponse(currentLocationURI, (x) =>
        //        {
        //            currentLocation = ((BingMapsRESTService.Common.JSON.Location)(x.ResourceSets[0].Resources[0])).Name;
        //            Debug.WriteLine("Current Location is" + currentLocation);
        //        });
        //    }
        //    return currentLocation;
        //}

        public string GetCurrentLocation(string latitude, string longitude)
        {
            string latitudewithoffset;
            string longitudewithoffset;
            string currentLocation = null;
            if (!String.IsNullOrEmpty(latitude) && !String.IsNullOrEmpty(longitude))
            {
                ApplyOffset(latitude, longitude, out latitudewithoffset, out longitudewithoffset);
                if (!String.IsNullOrEmpty(latitudewithoffset) && !String.IsNullOrEmpty(longitudewithoffset))
                {
                    Uri currentLocationURI = new Uri(string.Format("http://dev.ditu.live-int.com/REST/v1/Locations/{0},{1}?key={2}", latitudewithoffset, longitudewithoffset, key));
                    var response = GetURIResponseSynced(currentLocationURI);
                    if (response.ResourceSets[0].Resources[0] != null)
                    {
                        currentLocation = ((BingMapsRESTService.Common.JSON.Location)(response.ResourceSets[0].Resources[0])).Name;
                        Debug.WriteLine("Current Location is" + currentLocation);
                    }              
                }
            }
            
            return currentLocation;
        }

        public void ApplyOffset(string latitude, string longitude, out string latitudewithoffset, out string longitudewithoffset)
        {
            latitudewithoffset = null;
            longitudewithoffset = null;
            var offsetURL = string.Format(@"http://dev.ditu.live-int.com/REST/V1/LocationOffset/singlepoint?Lat={0}&Lon={1}&o=xml&key={2}", latitude, longitude, key);
            try
            {
                HttpWebRequest request = WebRequest.Create(offsetURL) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                var root = xmlDoc.DocumentElement;
                latitudewithoffset = root.GetElementsByTagName(@"ResourceSets")[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText;
                longitudewithoffset = root.GetElementsByTagName(@"ResourceSets")[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                Console.Read();
                //return null;
            }

        }

        public Response GetURIResponseSynced(Uri uri)
        {
            WebClient client = new WebClient();
            Stream data = client.OpenRead(uri);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
            return ser.ReadObject(data) as Response;
        }

        public void GetURIResponse(Uri uri, Action<Response> callback)
        {
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += (o, a) =>
            {
                if (callback != null)
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
                    callback(ser.ReadObject(a.Result) as Response);
                }
            };
            wc.OpenReadAsync(uri);

        }

        public Uri GetRoutingURI(string currentlocation, string destination)
        {
            return new Uri(string.Format("http://dev.ditu.live-int.com/REST/v1/Routes?wayPoint.1={0}&waypoint.2={1}&key={2}", currentlocation, destination, key));
        }

        public List<GPSInstruction.Instruction> GetRoute(string currentlocation, string destination)
        {
            var uri = GetRoutingURI(currentlocation, destination);
            var response = GetURIResponseSynced(uri);
            RouteLeg leg = ((BingMapsRESTService.Common.JSON.Route)(response.ResourceSets[0].Resources[0])).RouteLegs[0];
            var instructions = new GPSInstruction(((BingMapsRESTService.Common.JSON.Route)(response.ResourceSets[0].Resources[0])).RouteLegs[0].ItineraryItems);
            return instructions.InstructionList;

        }
    }
}
