using System;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;

namespace NavigatorManaged
{
    internal class ComPortReader
    {
        private SerialPort _serialPort;

        public LatitudeLongitude Latitude { get; private set; }
        public LatitudeLongitude Longitude { get; private set; }

        public GpsSpeed Speed { get; private set; }

        public string Coordinates
        {
            get
            {
                return new Coordinates(Latitude, Longitude).ToString();
            }
            private set
            {
            }
        }

        public ComPortReader(string portName = @"COM3", int baudRate = 9600)
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudRate;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            Coordinates = null;
        }

        public bool IsOpen()
        {
            return _serialPort.IsOpen;
        }

        public bool OpenPort()
        {
            try
            {
                _serialPort.Open();
                Task readThread = Task.Run(() =>
                {
                    while (true)
                    {
                        
                            string message = @"";
                            try
                            {
                                if (_serialPort.BytesToRead > 0)
                                {
                                    message = _serialPort.ReadLine();
                                }
                                
                            }
                            catch
                            {
                                continue;
                            }
                             
                            var tokens = message.Split(',');
                            if (tokens[0].StartsWith("$GPGGA"))
                            {
                                if (!String.IsNullOrEmpty(tokens[2]) && !String.IsNullOrEmpty(tokens[4]))
                                {
                                    Debug.WriteLine(message);
                                    Latitude = new LatitudeLongitude(tokens[2], tokens[3]);
                                    Longitude = new LatitudeLongitude(tokens[4], tokens[5]);

                                    Debug.WriteLine(@"Parsed Latitude and Longitude are {0},{1}", Latitude.ToString(), Longitude.ToString());
                                }

                            }

                            if (tokens[0].StartsWith("$GPRMC"))
                            {
                                if (!String.IsNullOrEmpty(tokens[7]))
                                {
                                    Debug.WriteLine(message);
                                    Speed = new GpsSpeed(tokens[7]);
                                    Debug.WriteLine(@"Current Speed is  {0},{1}", Speed.ToString());
                                }
                            }
                        }
    
                });
                return true;
            }
            catch (Exception e)
            {
                
                Console.WriteLine("The port may be in use - {0}", e.Message);
                return false;
            }

        }

        public bool ClosePort()
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                    return true;
                }
                
                return false;          
            }
            catch
            {
                return false;
            }
            
        }

        //public void OnTimedEvent()
        //{
        //    string message = _serialPort.ReadLine();
        //    var tokens = message.Split(',');
        //    if (tokens[0].StartsWith("GPGGA"))
        //    {
        //        if (String.IsNullOrEmpty(tokens[2]) && String.IsNullOrEmpty(tokens[3]))
        //        {
        //            Latitude = tokens[2];
        //            Longitude = tokens[3];
        //        }

        //    }

        //    //Console.WriteLine("The cordinates as of now are {0}", Coordinates);
        //}

    }
}
