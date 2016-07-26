using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{
    public float height = 1;
    public float slideFactor = (float)0.2;

    private MeshFilter mFilter;
    private float topGap = (float) 0.01;
    private Sprite sprite;

    private float tempTime = 0;

    // Use this for initialization
    void Start()
    {
        this.sprite = this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        this.mFilter = GetComponent<MeshFilter>();
        this.initializeMesh();
        this.changeHeight(height);
    }

    // Update is called once per frame
    void Update()
    {
        tempTime += Time.deltaTime;
        if(tempTime > 5)
        {
            this.changeHeight(Random.Range(1, 10));
            tempTime = 0;
            Color[] newColors = new Color[49];
            for(int i = 0; i < 49; i ++) 
            {
                newColors[i] = new Color(Random.value, Random.value, Random.value, (float) i / 50);
            }
            this.recolor(newColors);
        }
    }

    /// <summary>
    /// Gets a 2D flattened array (row by row) of the colors of each pixel on the top of this building.
    /// </summary>
    /// <returns>The array of colors.</returns>
    Color[] getColors()
    {
        return this.sprite.texture.GetPixels();
    }

    void recolor(Color[] colors)
    {
        this.sprite.texture.SetPixels(colors);
        this.sprite.texture.Apply();
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
