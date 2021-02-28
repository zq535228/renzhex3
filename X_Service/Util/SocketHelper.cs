using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace X_Service.Util {
    public class SocketHelper {

        public static Socket GetSocket() {
            //设定服务端IP地址  
            //IPAddress ip = IPAddress.Parse("223.4.173.93");
            //IPAddress ip = IPAddress.Parse("192.168.1.103");
            IPAddress ip = IPAddress.Parse("116.255.139.227");
            int port = 9778;
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try {
                clientSocket.Connect(new IPEndPoint(ip, port)); //配置服务端IP与端口  
                //EchoHelper.Echo("连接服务端成功！", "系统登录", EchoHelper.EchoType.任务信息);
            } catch {
                EchoHelper.Echo("连接服务端失败，请联系Qin，检查服务端是否在开启！", "系统登录", EchoHelper.EchoType.错误信息);
                Thread.Sleep(5000);
                Environment.Exit(0);
                return null;
            }
            return clientSocket;
        }

        public static int SendData(Socket s, byte[] data) {
            int total = 0;
            int size = data.Length;
            int dataleft = size;
            int sent;

            while (total < size) {
                sent = s.Send(data, total, dataleft, SocketFlags.None);
                total += sent;
                dataleft -= sent;
            }

            return total;
        }

        public static byte[] ReceiveData(Socket s, int size) {
            int total = 0;
            int dataleft = size;
            byte[] data = new byte[size];
            int recv;
            while (total < size) {
                recv = s.Receive(data, total, dataleft, SocketFlags.None);
                if (recv == 0) {
                    data = null;
                    break;
                }

                total += recv;
                dataleft -= recv;
            }
            return data;
        }

        public static int SendVarData(Socket s, byte[] data) {
            int total = 0;
            int size = data.Length;
            int dataleft = size;
            int sent;
            byte[] datasize = new byte[4];
            datasize = BitConverter.GetBytes(size);
            sent = s.Send(datasize);

            while (total < size) {
                sent = s.Send(data, total, dataleft, SocketFlags.None);
                total += sent;
                dataleft -= sent;
            }

            return total;
        }

        public static byte[] ReceiveVarData(Socket s) {
            int total = 0;
            int recv;
            byte[] datasize = new byte[4];
            recv = s.Receive(datasize, 0, 4, SocketFlags.None);
            int size = BitConverter.ToInt32(datasize, 0);
            int dataleft = size;
            byte[] data = new byte[size];
            while (total < size) {
                recv = s.Receive(data, total, dataleft, SocketFlags.None);
                if (recv == 0) {
                    data = null;
                    break;
                }
                total += recv;
                dataleft -= recv;
            }
            return data;
        }

    }
}
