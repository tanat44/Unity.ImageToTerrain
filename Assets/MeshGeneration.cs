using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{

    const int SIZE = 100;
    const float MAX_HEIGHT = 0.2f;
    List<List<float>> heightMap;
    public Material mat;
    void Start()
    {
        //RandomHeightMap();
        LoadImageHeightMap();

        var mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        for (int j = 0; j < SIZE; ++j)
        {
            for (int i = 0; i < SIZE; ++i)
            {
                vertices.Add(new Vector3(((float)i)/SIZE * 2.0f, heightMap[j][i], ((float)j)/SIZE*2.0f));
            }
        }

        for (int j = 0; j < SIZE -1; ++j)
        {
            for (int i = 0; i < SIZE - 1; ++i)
            {
                triangles.Add(j * SIZE + i);
                triangles.Add((j + 1) * SIZE + i);
                triangles.Add(j * SIZE + i + 1);

                triangles.Add(j * SIZE + i + 1);
                triangles.Add((j+1) * SIZE + i);
                triangles.Add((j+1) * SIZE + i + 1);
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();


        GameObject go = new GameObject("terrain");
        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = mesh;
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.material = mat;

    }

    void LoadImageHeightMap()
    {
        var fileData = File.ReadAllBytes("Assets/world.jpeg");
        Texture2D texture = new Texture2D(SIZE, SIZE);
        texture.LoadImage(fileData);
        heightMap = new List<List<float>>();

        int vStep = (int) texture.height / SIZE;
        int hStep = (int) texture.width / SIZE;
        float minHeight = Mathf.Infinity;
        float maxHeight = 0.0f;
        for (int j = 0; j < SIZE; ++j)
        {
            var row = new List<float>();
            for (int i = 0; i < SIZE; ++i)
            {
                var color = texture.GetPixel(i*hStep, j*vStep);
                float value = color.grayscale;
                row.Add(value);

                minHeight = value < minHeight ? value : minHeight;
                maxHeight = value > maxHeight ? value : maxHeight;
            }
            heightMap.Add(row);
        }

        // normalize heightmap
        float range = maxHeight - minHeight;
        for (int j = 0; j < SIZE; ++j)
        {
            for (int i = 0; i < SIZE; ++i)
            {
                heightMap[j][i] = (heightMap[j][i] - minHeight)/ range * MAX_HEIGHT;
            }
        }
    }

    void RandomHeightMap()
    {
        heightMap = new List<List<float>>();
        for (int i = 0; i < 10; ++i)
        {
            var row = new List<float>();
            for (int j = 0; j < 10; ++j)
            {
                row.Add(Random.Range(0, 2));
            }
            heightMap.Add(row);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
