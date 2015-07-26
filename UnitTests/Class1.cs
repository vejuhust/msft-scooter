using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavigatorManaged;
using System.Diagnostics;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var navigation = new Navigator(/*ChangePortName is needed*/);
            var logger = new Logger(navigation);
            //var route1 = navigation.GetRoute(@"北京市鼓楼东大街46号", @"三里屯");
            //var routing = new Routing();
            var destination = @"独墅湖体育馆";
            if (args.Length > 0)
            {
                destination = args[0];
            }

            try
            {
                navigation.Connect();
                Stopwatch s = new Stopwatch();
                s.Start();

                while (s.Elapsed < TimeSpan.FromSeconds(600))
                {
                    var currentLocation = navigation.GetCurrentLocation();
                    if (!String.IsNullOrEmpty(currentLocation))
                    {
                        //var route1 = navigation.GetRoute(@"北京市鼓楼东大街46号", @"三里屯");
                        var route = navigation.GetRoute(currentLocation, destination);
                        Console.WriteLine(@"Current Location is :", navigation.GetCurrentLocation());
                    }
                    
                }
            }
            
            catch
            {
                navigation.Disconnect();
            }

            finally
            {
                navigation.Disconnect();
            }
         


           
        }
    }
}
