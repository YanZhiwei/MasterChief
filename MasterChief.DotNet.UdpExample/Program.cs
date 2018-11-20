using MasterChief.DotNet4._5.Utilities.Communication;
using MasterChief.DotNet4._5.Utilities.Model;
using MasterChief.DotNet4.Utilities.Common;
using System;
using System.Threading.Tasks;

namespace MasterChief.DotNet.UdpExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            UdpAppServer server = new UdpAppServer("127.0.0.1", 32123);

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    UdpAppReceived received = await server.Receive();
                    Console.WriteLine(string.Format("{0} 主站收到：{1}", DateTime.Now.FormatDate(1), received.Message));
                    server.Reply(received.Message, received.Sender);
                    if (received.Message == "quit")
                    {
                        break;
                    }
                }
            });


            UdpAppClient client = UdpAppClient.ConnectTo("127.0.0.1", 32123);

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        UdpAppReceived received = await client.Receive();
                        Console.WriteLine(DateTime.Now.FormatDate(1) + " 终端收到：" + received.Message);
                        if (received.Message.Contains("quit"))
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
            });

            string read;
            do
            {
                read = Console.ReadLine();
                client.Send(read);
            } while (read != "quit");
        }
    }
}
