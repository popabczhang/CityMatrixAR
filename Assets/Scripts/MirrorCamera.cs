using UnityEngine;
using System.Collections;

public class MirrorCamera : MonoBehaviour {

    public Vector2 monitorDimensions;
    public Transform focusPoint;

    private Camera cam;

	// Use this for initialization
	void Start () {
        this.cam = this.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 old = this.transform.position;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float depthical = Input.GetAxis("Depthical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 delta = new Vector3(horizontal, vertical, depthical);

        this.transform.Translate(new Vector3(mouseX, mouseY, depthical));
        //this.transform.position = old + delta;
        Debug.Log(this.cam.projectionMatrix);
    }

    void LateUpdate()
    {
        this.cam.fieldOfView = this.calculateFOV();
        this.transform.LookAt(this.focusPoint);
    }

    float calculateFOV()
    {
        float distance = Vector3.Distance(this.transform.position, this.focusPoint.position);
        return 2.0f * Mathf.Atan(monitorDimensions.y * 0.5f / distance) * Mathf.Rad2Deg;
    }
}
