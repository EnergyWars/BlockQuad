using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public struct PerlinSettings
{
    public float heightScale;
    public float scale;
    public int octaves;
    public float heightOffset;
    public float probability;

    public PerlinSettings(float hs, float s, int o, float ho, float p)
    {
        this.heightScale = hs;
        this.scale = s;
        this.octaves = o;
        this.heightOffset = ho;
        this.probability = p;
    }
}

public class World : MonoBehaviour
{
    public static Vector3Int worldDimensions = new Vector3Int(4, 4, 4);
    public static Vector3Int chunkDimensions = new Vector3Int(10, 10, 10);
    public GameObject chunkPrefab;
    public GameObject mCamera;
    public GameObject fpc;
    public Slider loadingBar;

    public static PerlinSettings surfaceSettings;
    public PerlinGrapher surface;

    public static PerlinSettings stoneSettings;
    public PerlinGrapher stone;

    public static PerlinSettings diamondTSettings;
    public PerlinGrapher diamondT;

    public static PerlinSettings diamondBSettings;
    public PerlinGrapher diamondB;

    public static PerlinSettings caveSettings;
    public Perlin3DGrapher caves;


    void Start()
    {
        loadingBar.maxValue = worldDimensions.x * worldDimensions.z;

        surfaceSettings = new PerlinSettings(
            surface.heightScale, surface.scale, surface.octaves, surface.heightOffset,
            surface.probability);

        stoneSettings = new PerlinSettings(
            stone.heightScale, stone.scale, stone.octaves, stone.heightOffset,
            stone.probability);

        diamondTSettings = new PerlinSettings(
            diamondT.heightScale, diamondT.scale, diamondT.octaves, diamondT.heightOffset,
            diamondT.probability);

        diamondBSettings = new PerlinSettings(
            diamondB.heightScale, diamondB.scale, diamondB.octaves, diamondB.heightOffset,
            diamondB.probability);

        caveSettings = new PerlinSettings(
            caves.heightScale, caves.scale, caves.octaves, caves.heightOffset,
            caves.DrawCutOff);

        StartCoroutine(BuildWorld());
    }

    void BuildChunkColumn(int x, int z)
    {
        for (int y = 0; y < worldDimensions.y; y++)
        {
            GameObject chunk = Instantiate(chunkPrefab);
            Vector3Int position = new Vector3Int(x * chunkDimensions.x, y * chunkDimensions.y, z * chunkDimensions.z);
            chunk.GetComponent<Chunk>().CreateChunk(chunkDimensions, position);
        }
    }


    // Start is called before the first frame update
    IEnumerator BuildWorld()
    {
        for (int z = 0; z < worldDimensions.z; z++)
        {
            for (int x = 0; x < worldDimensions.x; x++)
            {
                BuildChunkColumn(x, z);
                loadingBar.value++;
                yield return null;
            }
        }

        mCamera.SetActive(false);

        int xpos = worldDimensions.x * chunkDimensions.x / 2;
        int zpos = worldDimensions.z * chunkDimensions.x / 2;
        int ypos = (int) MeshUtils.fBM(xpos, zpos, surfaceSettings.octaves, surfaceSettings.scale,
            surfaceSettings.heightScale, surfaceSettings.heightOffset) + 10;
        fpc.transform.position = new Vector3Int(xpos, ypos, zpos);
        loadingBar.gameObject.SetActive(false);
        fpc.SetActive(true);
    }

    private void Update()
    {
    }
}