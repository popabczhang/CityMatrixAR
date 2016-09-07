using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System.Net.Sockets;
using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading;

public class KinectHeadCamera : MonoBehaviour {

    public int maxBodies = 12;
    public Transform kinect;
    public float spaceUnitsPerMeter;
    public bool useUdp = false;
    public int udpPort = 1234;
    public int udpWait = 10;

    KinectSensor sensor;
    BodyFrameReader bodyReader;
    List<Body> bodies;

    Vector3 kinectHeadPos;

    UdpClient udp;
    IPEndPoint ipEP;

	// Use this for initialization
	void Start () {
        this.bodies = new List<Body>();
        this.kinectHeadPos = this.transform.position;
        if (this.useUdp)
        {
            this.ipEP = new IPEndPoint(IPAddress.Any, this.udpPort);
            this.udp = new UdpClient(this.ipEP);
            this.QueryUdp(null);
        } else
        {
            this.sensor = KinectSensor.GetDefault();
            this.sensor.Open();
            this.bodyReader = this.sensor.BodyFrameSource.OpenReader();
            this.bodyReader.FrameArrived += this.OnBodyFrameArrived;
        }
    }


    // Update is called once per frame
    void Update () {
        if(!this.useUdp)
        {
            Vector3 chosen = new Vector3();
            foreach (Body b in this.bodies)
            {
                CameraSpacePoint a = b.Joints[JointType.Head].Position;
                chosen.x = a.X;
                chosen.y = a.Y;
                chosen.z = a.Z;
            }
        }
        Vector3 pos = this.GetVirtualPosition(this.kinectHeadPos);
        pos.z = Math.Min(0, pos.z);
        this.transform.position = pos;

    }

    private void OnBodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
    {
        BodyFrame frame = e.FrameReference.AcquireFrame();
        if (frame != null)
        {
            frame.GetAndRefreshBodyData(this.bodies);
        }
    }

    void QueryUdp(IAsyncResult previous)
    {
        if(previous == null)
        {
            Debug.Log("Starting");
            this.udp.BeginReceive(new AsyncCallback(QueryUdp), null);
        } else
        {
            byte[] data = this.udp.EndReceive(previous, ref this.ipEP);
            string[] parsed = Encoding.ASCII.GetString(data).Split(',');
            this.kinectHeadPos = new Vector3(float.Parse(parsed[0]), float.Parse(parsed[1]), float.Parse(parsed[2]));
            Debug.Log(this.kinectHeadPos);
            Thread.Sleep(this.udpWait);
            this.udp.BeginReceive(new AsyncCallback(QueryUdp), null);
        }

    }

    void DrawBodies()
    {
        foreach(Body b in this.bodies)
        {
            foreach (System.Collections.Generic.KeyValuePair<JointType, Windows.Kinect.Joint> 
                j in b.Joints)
            {
                Vector3 pos = this.GetVirtualPosition(this.CSP2Vector3(j.Value.Position));
                UnityEngine.Vector3 angle = this.GetVirtualAngle(b.JointOrientations[j.Key].Orientation);
                Debug.DrawRay(pos, angle + pos);
            }
        }
    }

    Vector3 GetVirtualPosition(Vector3 a)
    {
        a = a * this.spaceUnitsPerMeter;
        a.z = a.z * -1;
        a = a + this.kinect.position;
        return a;
    }

    Vector3 CSP2Vector3(CameraSpacePoint a)
    {
        return new Vector3(a.X, a.Y, a.Z);
    }

    UnityEngine.Vector4 GetVirtualAngle(Windows.Kinect.Vector4 a)
    {
        UnityEngine.Vector4 output = new UnityEngine.Vector4(a.X, a.Y, a.Z, a.W);
        return output;
    }
}
