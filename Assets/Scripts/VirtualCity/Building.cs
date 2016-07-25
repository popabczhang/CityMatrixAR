using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{
    private MeshFilter mFilter;

    // Use this for initialization
    void Start()
    {
        mFilter = GetComponent<MeshFilter>();
        initializeMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void changeHeight(float h)
    {
        Vector3 old = GetComponent<Transform>().localScale;
        old.y = h;
        GetComponent<Transform>().localScale = old;
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
