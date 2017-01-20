
/*
    -----------------------
    UDP-Receive (send to)
    -----------------------
    // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]

    // > receive
    // 127.0.0.1 : 8051

    // send
    // nc -u 127.0.0.1 8051
*/


using UnityEngine;
using UnityEditor;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{

    // receiving Thread
    Thread receiveThread;

    // udpclient object
    UdpClient client;

    // public
    // public string IP = "127.0.0.1"; default local
    public int port; // define > init

    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = ""; // clean up this from time to time!

    public string udpString;

    private ThreadController threadController;

    class ThreadController
    {
        public bool ShouldExecute { get; set; }
    }

    public void Start()
    {
      threadController = new ThreadController{ShouldExecute = true};
      Thread receiveThread = new Thread(ReceiveData);
      receiveThread.IsBackground = true;
      receiveThread.Start(threadController);
    }

    // OnGUI
    void OnGUIi()
    {
        Rect rectObj = new Rect(40, 10, 200, 400);
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        GUI.Box(rectObj, "# UDPReceive\n127.0.0.1 " + port + " #\n"
                   + "shell> nc -u 127.0.0.1 : " + port + " \n"
                   + "\nLast Packet: \n" + lastReceivedUDPPacket
                   + "\n\nAll Messages: \n" + allReceivedUDPPackets
               , style);
    }

    void OnDisable()
    {
      if ( receiveThread!= null)
      receiveThread.Abort();

      client.Close();
    }

    // receive thread
    private void ReceiveData(object inp)
    {
        var tc = (ThreadController) inp;
        client = new UdpClient(port);
        while (tc.ShouldExecute)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);

                lastReceivedUDPPacket = text;
                udpString = text;
                //allReceivedUDPPackets = allReceivedUDPPackets + text;

            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
                return;
            }
        }
    }

    // getLatestUDPPacket
    // cleans up the rest
    public string getLatestUDPPacket()
    {
        allReceivedUDPPackets = "";
        return lastReceivedUDPPacket;
    }
}
