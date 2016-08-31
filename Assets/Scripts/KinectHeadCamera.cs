using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class KinectHeadCamera : MonoBehaviour {

    public int maxBodies = 12;
    public Transform kinect;
    public float spaceUnitsPerMeter;

    KinectSensor sensor;
    BodyFrameReader bodyReader;
    Body[] bodies;

	// Use this for initialization
	void Start () {
        this.bodies = new Body[this.maxBodies];
        this.sensor = KinectSensor.GetDefault();
        this.sensor.Open();
        this.bodyReader = this.sensor.BodyFrameSource.OpenReader();
        this.bodyReader.FrameArrived += this.OnBodyFrameArrived;
    }

    private void OnBodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
    {
        BodyFrame frame = e.FrameReference.AcquireFrame();
        if (frame != null)
        {
            frame.GetAndRefreshBodyData(this.bodies);
        }
    }

    // Update is called once per frame
    void Update () {
        Vector3 chosen = new Vector3();
        foreach(Body b in this.bodies)
        {
            CameraSpacePoint a = b.Joints[JointType.Head].Position;
            chosen.x = a.X;
            chosen.y = a.Y;
            chosen.z = a.Z;
        }
        this.transform.position = chosen;
	}

    void DrawBodies()
    {
        foreach(Body b in this.bodies)
        {
            foreach (System.Collections.Generic.KeyValuePair<JointType, Windows.Kinect.Joint> 
                j in b.Joints)
            {
                Vector3 pos = this.GetVirtualPosition(j.Value.Position);
                UnityEngine.Vector3 angle = this.GetVirtualAngle(b.JointOrientations[j.Key].Orientation);
                Debug.DrawRay(pos, angle + pos);
            }
        }
    }

    Vector3 GetVirtualPosition(CameraSpacePoint a)
    {
        Vector3 output = new Vector3(a.X, a.Y, a.Z);
        output = (output * this.spaceUnitsPerMeter) + this.kinect.position;
        return output;
    }

    UnityEngine.Vector4 GetVirtualAngle(Windows.Kinect.Vector4 a)
    {
        UnityEngine.Vector4 output = new UnityEngine.Vector4(a.X, a.Y, a.Z, a.W);
        return output;
    }
}
