using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{
    private MeshFilter mFilter;
    private float topGap = (float) 0.01;
    public float height = 1;
    public float slideFactor = (float) 0.2;

    private float tempTime = 0;

    // Use this for initialization
    void Start()
    {
        mFilter = GetComponent<MeshFilter>();
        initializeMesh();
        changeHeight(height);
    }

    // Update is called once per frame
    void Update()
    {
        tempTime += Time.deltaTime;
        if(tempTime > 5)
        {
            this.changeHeight(Random.Range(1, 10));
            tempTime = 0;
        }
    }

    internal void changeHeight(float h)
    {
        this.height = h;
        StartCoroutine("HeightSlide");
    }

    IEnumerator HeightSlide()
    {
        Vector3 scale = this.transform.localScale;
        while(scale.y != this.height)
        {
            scale.y += (this.height - scale.y) * this.slideFactor;
            if(Mathf.Abs(this.height - scale.y) < 0.01)
            {
                scale.y = this.height;
            }
            this.transform.localScale = scale;
            yield return null;
        }
    }

    void initializeMesh()
    {
        mFilter.mesh = new Mesh();
        Vector3[] newVerts = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0),

            new Vector3(0, 1, 0),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 0),

            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 1, 1),
            new Vector3(1, 0, 1),

            new Vector3(0, 0, 1),
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),

            new Vector3(0, 0, 1),
            new Vector3(0, 1, 1),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, 0),

            new Vector3(1, 0, 1),
            new Vector3(1, 1, 1),
            new Vector3(0, 1, 1),
            new Vector3(0, 0, 1),
        };

        Vector3[] normals = new Vector3[]
        {
            new Vector3(0, 0, -1),
            new Vector3(0, 0, -1),
            new Vector3(0, 0, -1),
            new Vector3(0, 0, -1),

            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0),

            new Vector3(1, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 0),

            new Vector3(0, -1, 0),
            new Vector3(0, -1, 0),
            new Vector3(0, -1, 0),
            new Vector3(0, -1, 0),

            new Vector3(-1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(-1, 0, 0),

            new Vector3(0, 0, -1),
            new Vector3(0, 0, -1),
            new Vector3(0, 0, -1),
            new Vector3(0, 0, -1)
        };

        int[] triangles = new int[]
        {
            0, 2, 3, 0, 1, 2,
            4, 6, 7, 4, 5, 6,
            8, 10, 11, 8, 9, 10,
            12, 14, 15, 12, 13, 14,
            16, 18, 19, 16, 17, 18,
            20, 22, 23, 20, 21, 22

        };

        mFilter.mesh.vertices = newVerts;
        mFilter.mesh.normals = normals;
        mFilter.mesh.triangles = triangles;
    }
}
