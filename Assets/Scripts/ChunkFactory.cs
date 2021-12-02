using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkFactory : MonoBehaviour
{

    [Header("Components")]
    public Chunk chunkPrototype;
    public GameObject cube;
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

    //Offsets
    private int offsetX = 0;
    private int offsetY = 0;
    private int offsetZ = 0;

    private int chunkCounterX;
    private int chunkCounterZ;


    private void Start()
    {
        //chunkPrototype.cube = cube;
        chunkPrototype.material = material;
        chunkPrototype.width = width;
        chunkPrototype.height = height;
        chunkPrototype.seed = seed;
        chunkPrototype.amplification = amplification;
        chunkPrototype.scale = scale;
        chunkPrototype.heightScale = heightScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            chunkPrototype.amplification = amplification;
            chunkPrototype.scale = scale;
            chunkPrototype.heightScale = heightScale;
            Instantiate(chunkPrototype, new Vector3(offsetX, offsetY, offsetZ), Quaternion.identity);
            chunkCounterX++;

            if (chunkCounterX > 8)
            {
                chunkCounterZ++;
                chunkCounterX = 0;
                offsetZ = chunkCounterZ * width;
            }
            offsetX = chunkCounterX * width;
        }
    }
}
