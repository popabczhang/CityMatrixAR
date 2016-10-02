using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{
    public enum Type
    {
        FLAT, WIREFRAME, SOLID, MESH
    }

    [Header("View Info")]
    public VirtualCityView virtualCityView;
    private Type _type;
    public Type ViewType {
        get { return _type; }
        set { _type = value;
            this.UpdateView();
        }
    }

    [Header("View Meshes")]
    public GameObject meshPrefab;
    public GameObject flatPrefab;

    [Header("Solid and Wireframe Properties")]
    private float targetHeight;
    public float Height
    {
        get { return targetHeight; }
        set { this.targetHeight = value;
            StartCoroutine("HeightSlide");
        }
    }
    public float spriteTopGap = 0.01F;
    public float heightSlideFactor = (float)0.2;
    private Sprite sprite;
    private float minHeight = 0.1F;

    [Header("Wireframe Properties")]
    public Material wireframeMaterial = null;
    public Color wireframeColor = Color.white;
    public bool drawWireframe = false;
    private float wireframeWidth = 0.015f;
    private LineRenderer[] wireframe = new LineRenderer[12];

    // Use this for initialization
    void Start()
    {
        SpriteRenderer sr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        Texture2D newTexture = Instantiate(sr.sprite.texture) as Texture2D;
        sr.sprite = 
            Sprite.Create(newTexture, 
            new Rect(0, 0, sr.sprite.texture.width, sr.sprite.texture.height),
            new Vector2(0.5f, 0.5f), 7f);
        this.sprite = sr.sprite;
        if(wireframeMaterial == null)
        {
            wireframeMaterial = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.drawWireframe)
        {
            this.DrawWireframe();
        }
    }

    void UpdateView()
    {
        Transform flat = this.transform.Find("FlatView");
        if (flat != null) flat.gameObject.SetActive(false);
        Transform mesh = this.transform.Find("MeshView");
        if (mesh != null) mesh.gameObject.SetActive(false);
        Transform solid = this.transform.Find("SolidView");
        if (solid != null) solid.gameObject.SetActive(false);
        Transform sprite = this.transform.Find("TopSprite");
        sprite.gameObject.SetActive(false);
        this.drawWireframe = false;

        this.transform.localScale = new Vector3(1, 1, 1);

        float scale;
        Vector3 bounds;
        switch(this._type)
        {
            case Type.FLAT:
                if(flat != null) Destroy(flat.gameObject);
                flat = Instantiate(this.flatPrefab).transform;
                bounds = GetBounds(flat);
                scale = 1f / Mathf.Max(bounds.x, bounds.z);
                flat.localScale = new Vector3(1, 1, 1);
                flat.name = "FlatView";
                flat.parent = this.transform;
                flat.localPosition = new Vector3(0, 0, 0);
                flat.gameObject.SetActive(true);
                break;
            case Type.MESH:
                if (mesh != null) Destroy(mesh.gameObject);
                mesh = Instantiate(this.meshPrefab).transform;
                bounds = GetBounds(mesh);
                scale = 1f / Mathf.Max(bounds.x, bounds.z);
                mesh.localScale = new Vector3(1, 1, 1);
                mesh.name = "MeshView";
                mesh.parent = this.transform;
                mesh.localPosition = new Vector3(0, 0, 0);
                mesh.gameObject.SetActive(true);
                break;
            case Type.SOLID:
                sprite.gameObject.SetActive(true);
                if (solid == null)
                {
                    solid = new GameObject().transform;
                    solid.parent = this.transform;
                    solid.localPosition = new Vector3(0, 0, 0);
                }
                break;
            case Type.WIREFRAME:
                this.drawWireframe = true;
                break;
            default:
                break;
        }
    }

    Vector3 GetBounds(Transform a)
    {
        Vector3 max = new Vector3(0, 0, 0);
        MeshRenderer parentRender = this.GetComponent<MeshRenderer>();
        if(parentRender != null)
        {
            max = parentRender.bounds.size;
        }
        foreach(Transform t in a) {
            max = Vector3.Max(max, GetBounds(a));
        }
        return max;
    }

    void DrawWireframe()
    {
        float h = this.transform.localScale.y;
        Vector3 pos = this.transform.position;
        Vector3 scale = this.transform.lossyScale;
        Vector3 L00 = new Vector3(pos.x, pos.y, pos.z);
        Vector3 L10 = new Vector3(pos.x + scale.x, pos.y, pos.z);
        Vector3 L01 = new Vector3(pos.x, pos.y, pos.z + scale.z);
        Vector3 L11 = new Vector3(pos.x + scale.x, pos.y, pos.z + scale.z);
        Vector3 U00 = new Vector3(pos.x, pos.y + scale.y, pos.z);
        Vector3 U10 = new Vector3(pos.x + scale.x, pos.y + scale.y, pos.z);
        Vector3 U01 = new Vector3(pos.x, pos.y + scale.y, pos.z + scale.z);
        Vector3 U11 = new Vector3(pos.x + scale.x, pos.y + scale.y, pos.z + scale.z);

        DrawLine(0, L00, L01, this.wireframeColor, this.wireframeWidth);
        DrawLine(1, L10, L11, this.wireframeColor, this.wireframeWidth);
        DrawLine(2, L00, L10, this.wireframeColor, this.wireframeWidth);
        DrawLine(3, L01, L11, this.wireframeColor, this.wireframeWidth);

        DrawLine(4, L00, U00, this.wireframeColor, this.wireframeWidth);
        DrawLine(5, L10, U10, this.wireframeColor, this.wireframeWidth);
        DrawLine(6, L01, U01, this.wireframeColor, this.wireframeWidth);
        DrawLine(7, L11, U11, this.wireframeColor, this.wireframeWidth);

        DrawLine(8, U00, U01, this.wireframeColor, this.wireframeWidth);
        DrawLine(9, U10, U11, this.wireframeColor, this.wireframeWidth);
        DrawLine(10, U00, U10, this.wireframeColor, this.wireframeWidth);
        DrawLine(11, U01, U11, this.wireframeColor, this.wireframeWidth);
    }

    void DrawLine(int id, Vector3 a, Vector3 b, Color color, float width)
    {
        if (this.wireframe[id] == null)
        {
            GameObject line = new GameObject();
            line.transform.parent = this.transform;
            line.AddComponent<LineRenderer>();
            LineRenderer lr = line.GetComponent<LineRenderer>();
            lr.material = this.wireframeMaterial;
            lr.SetColors(color, color);
            lr.SetWidth(width, width);
            lr.SetPosition(0, a);
            lr.SetPosition(1, b);
            this.wireframe[id] = lr;
        } else
        {
            LineRenderer lr = this.wireframe[id];
            lr.SetPositions(new Vector3[]
            {
                a, b
            });
            lr.SetColors(color, color);
            lr.SetWidth(width, width);
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

    internal void recolor(Color[] colors)
    {
        this.sprite.texture.SetPixels(colors);
        this.sprite.texture.Apply();
    }

    IEnumerator HeightSlide()
    {
        Transform solid = this.transform.Find("SolidView");
        if(solid == null)
        {
            yield break;
        }
        Vector3 scale = solid.localScale;
        while(scale.y != this.targetHeight)
        {
            scale.y += (this.targetHeight - scale.y) * this.heightSlideFactor;
            if(Mathf.Abs(this.targetHeight - scale.y) < 0.01)
            {
                scale.y = this.targetHeight;
            }
            solid.transform.localScale = scale;
            this.repositionTopSprite(scale.y);
            yield return null;
        }
    }

    void repositionTopSprite(float scale)
    {
        Vector3 old = this.transform.GetChild(0).localPosition;
        old.y = 1 + 5 / (scale * 1000);
        this.transform.GetChild(0).localPosition = old;
    }

    void initializeMesh(Transform container)
    {
        container.gameObject.AddComponent<MeshRenderer>();
        MeshFilter mFilter = container.gameObject.AddComponent<MeshFilter>();
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
