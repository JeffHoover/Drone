using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Drone2
{
    // string something = Encoding.ASCII.GetString(toBytes);
    class Program
    {
        static void Main(string[] args)
        {
            _udpClient = new UdpClient(5556);
            _udpClient.Connect("192.168.1.1", 5556);

            SendCommand("REF", "256");

            Thread.Sleep(1000);

            SendCommand("REF", "512");

            Thread.Sleep(5000);

            var leftRight = 0f;
            var frontBack = 0f;
            var verticalSpeed = 0f;
            var angularSpeed = 0f;

            for (var i = 0; i < 5000; i += 10)
            {
                switch (i)
                {
                    case 0:
                        verticalSpeed = 0.7f;
                        break;

                    case 1000:
                        angularSpeed = 0.7f;
                        break;

                    case 2000:
                        verticalSpeed = 0.0f;
                        break;
                }

                SendCommand("PCMD", "1," +
                    ConvertFloat(leftRight) + "," +
                    ConvertFloat(frontBack) + "," +
                    ConvertFloat(verticalSpeed) + "," +
                    ConvertFloat(angularSpeed));
                Thread.Sleep(10);
            }

            Thread.Sleep(3000);

            SendCommand("REF", "0");

            Thread.Sleep(1000);

            _udpClient.Close();
        }

        static void SendCommand(string command, string param)
        {
            var transmit = "AT*" + command + "=" + _sequenceNumber++ + "," + param + "\r";
            var bytes = Encoding.ASCII.GetBytes(transmit);
            _udpClient.Send(bytes, bytes.Length);
        }

        static string ConvertFloat(float value)
        {
            var b = BitConverter.GetBytes(value);
            return BitConverter.ToInt32(b, 0).ToString();
        }

        private static int _sequenceNumber = 1000;
        private static UdpClient _udpClient;
    }
}
