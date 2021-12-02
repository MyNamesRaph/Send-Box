using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noise;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
    [Header("Components")]
    //public GameObject cube;
    public Material material;

    [Header("Dimensions")]
    public int width = 16;
    public int height = 128;

    [Header("Generation Settings")]
    public long seed = 0;
    public int amplification = 10;
    public int HeightAmplification = 10;

    public float scale = 1;
    public float heightScale = 1;

   [Header("Offsets")]
    public float offsetX = 0;
    public float offsetY = 0;
    public float offsetZ = 0;

    private OpenSimplex2F simplex;

    public bool[,,] data;

    private List<Vector4> verticesDensity;
    private List<Vector3> vertices;
    private Mesh mesh;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        verticesDensity = new List<Vector4>();
        GetComponent<MeshFilter>().mesh = mesh;

        data = new bool[width, height, width];
        offsetX = transform.position.x;
        offsetY = transform.position.y;
        offsetZ = transform.position.z;

        simplex = new OpenSimplex2F(seed);
        GenerateVertices();
        //CombineAllMeshes();

        CreateMesh();
        UpdateMesh();
    }

    void GenerateVertices()
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                for (int z = 0; z < width; ++z) 
                {
                    //float value = GetSimplex3DAt(x + offsetX, y + offsetY, z + offsetZ) * amplification;
                    //value += GetSimplex2DAt(x + offsetX, z + offsetZ) * HeightAmplification;
                    if (y < height * 0.5 /*+ value*/ )
                    {
                        verticesDensity.Add(new Vector4(x, y, z, 1.0F));
                    }
                    else
                    {
                        verticesDensity.Add(new Vector4(x, y, z, 0.0F));
                    }
                }
            }
        }
        
    }

    void CreateMesh()
    {
        List<Vector3> newVertices = new List<Vector3>();
        triangles = MarchingCubes.FindTriangles(verticesDensity,out newVertices);
        vertices = newVertices;
    }



    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        /*int i = 0;
        foreach (Vector4 item in vertices)
        {
            Debug.Log(i.ToString()+item);
            i++;
        }*/
    }

    List<Vector3> Vector4GetXYZ(List<Vector4> vect)
    {
        List<Vector3> output = new List<Vector3>();
        foreach ( Vector4 vector in vect )
        {
            output.Add(new Vector3(vector.x, vector.y, vector.z));
        }
        return output;
    }

    void CombineAllMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);
        GetComponent<MeshRenderer>().material = material;
    }

    float GetSimplex3DAt(float x, float y, float z)
    {
        float xCoord = x / width * scale;
        float yCoord = y / height * scale;
        float zCoord = z / width * scale;

        double sample = simplex.Noise3_XZBeforeY(xCoord, yCoord, zCoord);

        return (float)sample;
    }

    float GetSimplex2DAt(float x, float z)
    {
        float xCoord = x / width * heightScale;
        float zCoord = z / height * heightScale;

        double sample = simplex.Noise2_XBeforeY(xCoord, zCoord);

        return (float)sample;
    }
}
