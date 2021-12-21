using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Noise;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    [Header("Components")]
    public Material material;

    [Header("Dimensions")]
    public int width = 16;
    public int height = 128;

    [Header("Generation Settings")]
    public long seed = 0;
    public float amplification = 10;
    public float heightAmplification = 1;
    public float caveThreshold = -3;
    public float scale = 0.05F;
    public float heightScale = 0.01F;

    [Header("Offsets")]
    public float offsetX = 0;
    public float offsetY = 0;
    public float offsetZ = 0;

    private OpenSimplex2F simplex;

    private Vector4[,,] verticesDensity;
    private List<Vector3> vertices;
    private Mesh mesh;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        verticesDensity = new Vector4[width,height,width];

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.MarkDynamic();

        GetComponent<MeshRenderer>().material = material;
        GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.TwoSided;

        offsetX = transform.position.x;
        offsetY = transform.position.y;
        offsetZ = transform.position.z;

        simplex = new OpenSimplex2F(seed);
        GenerateVertices();

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
                    float value = GetSimplex3DAt(x + offsetX, y + offsetY, z + offsetZ) * amplification;
                    float heightValue = GetSimplex2DAt(x + offsetX, z + offsetZ) * heightAmplification;
                    if (y > height * 0.5 + (value+heightValue))
                    {
                        //TODO: remove xyz to only have density
                        verticesDensity[x, y, z] = new Vector4(x, y, z, 1.0F);   
                    }
                    else if (value < caveThreshold)
                    {
                        verticesDensity[x, y, z] = new Vector4(x, y, z, 1.0F);
                    }
                    else
                    {
                        verticesDensity[x, y, z] = new Vector4(x, y, z, 0.0F);
                    }
                }
            }
        }
        
    }

    void CreateMesh()
    {
        List<Vector3> newVertices = new List<Vector3>();
        triangles = MarchingCubes.FindTriangles(verticesDensity,out newVertices,width,height);
        vertices = newVertices;
    }



    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    float GetSimplex3DAt(float x, float y, float z)
    {
        float xCoord = x  * scale;
        float yCoord = y  * scale;
        float zCoord = z  * scale;

        double sample = simplex.Noise3_XZBeforeY(xCoord, yCoord, zCoord);

        return (float)sample;
    }

    float GetSimplex2DAt(float x, float z)
    {
        float xCoord = x * heightScale;
        float zCoord = z * heightScale;

        double sample = simplex.Noise2_XBeforeY(xCoord, zCoord);

        return (float)sample;
    }
}
