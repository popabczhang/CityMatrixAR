﻿using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{
    public float slideFactor = (float)0.2;

    internal BuildingData data;
    private MeshFilter mFilter;
    private float topGap = 0.01F;
    private Sprite sprite;
    private float minHeight = 0.1F;
    private float targetHeight;

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
        this.mFilter = GetComponent<MeshFilter>();
        this.initializeMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates this building with the given data, making any other changes that are ramifaications of that change.
    /// </summary>
    /// <param name="newData">The data to replace the old with.</param>
    internal void updateData(BuildingData newData)
    {
        this.data = newData;
        this.recolor(this.data.getColors());
        this.changeHeight(this.data.height/this.data.width);
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

    internal void changeHeight(float h)
    {
        this.targetHeight = h < this.minHeight ? this.minHeight : h;
        StartCoroutine("HeightSlide");
    }

    IEnumerator HeightSlide()
    {
        Vector3 scale = this.transform.localScale;
        while(scale.y != this.targetHeight)
        {
            scale.y += (this.targetHeight - scale.y) * this.slideFactor;
            if(Mathf.Abs(this.targetHeight - scale.y) < 0.01)
            {
                scale.y = this.targetHeight;
            }
            this.transform.localScale = scale;
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
