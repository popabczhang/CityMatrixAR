using UnityEngine;
using System.Collections;

public class Marker : MonoBehaviour {

    VirtualCityModel city;
    int x = 0;
    int z = 0;
    float heightDelta = 10;

	// Use this for initialization
	void Start () {
        this.city = this.transform.parent.GetComponent<VirtualCityModel>();
	}
	
	// Update is called once per frame
	void Update () {
        Building[,] city = this.city.GetCity();

        if (Input.GetButtonDown("MarkerRight") && this.x < city.GetLength(0) - 1)
        {
            this.x += 1;
        } else if (Input.GetButtonDown("MarkerLeft") && this.x > 0)
        {
            this.x -= 1;
        }
        if (Input.GetButtonDown("MarkerForward") && this.z > 0)
        {
            this.z -= 1;
        }
        else if (Input.GetButtonDown("MarkerBack") && this.z < city.GetLength(1) - 1)
        {
            this.z += 1;
        }

        if(Input.GetButtonDown("MarkerUp"))
        {
            this.city.changeBuildingHeight(this.x, this.z, city[x, z].data.height + heightDelta);
        } else if(Input.GetButtonDown("MarkerDown"))
        {
            this.city.changeBuildingHeight(this.x, this.z, city[x, z].data.height - heightDelta);
        }


        Vector3 pos = city[x, z].transform.localPosition;
        pos.y = city[x, z].data.GetVirtualHeight();
        this.transform.localPosition = pos;
	}
}
