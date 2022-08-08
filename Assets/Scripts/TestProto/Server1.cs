using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;

public class Server1 : MonoBehaviour
{
    readonly string _ip = "192.168.7.230";
    readonly int _port = 1024;

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 100), "Send Public IP"))
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(_ip, _port);
            NetworkStream stream = new NetworkStream(socket);
            StreamWriter sw = new StreamWriter(stream);
            StreamReader sr = new StreamReader(stream);

            sw.WriteLine("111");
            sw.Flush();

            string st = sr.ReadLine();
            print(st);

            sw.Close();
            stream.Close();
            socket.Close();
        }
    }
}