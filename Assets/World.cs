using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World : MonoBehaviour
{
    public static Vector3 worldDimensions = new Vector3(3, 3, 3);
    public static Vector3 chunkDimensions = new Vector3(10, 10, 10);
    public GameObject chunkPrefab;
    public GameObject mCamera;
    public GameObject fpc;
    public Slider loadingBar;


    void Start()
    {
        loadingBar.maxValue = worldDimensions.x * worldDimensions.y * worldDimensions.z;
        StartCoroutine(BuildWorld());
    }


    // Start is called before the first frame update
    IEnumerator BuildWorld()
    {
        for (int z = 0; z < worldDimensions.z; z++)
        {
            for (int y = 0; y < worldDimensions.y; y++)
            {
                for (int x = 0; x < worldDimensions.x; x++)
                {
                    GameObject chunk = Instantiate(chunkPrefab);
                    Vector3 position = new Vector3(x * chunkDimensions.x, y * chunkDimensions.y, z * chunkDimensions.z);
                    chunk.GetComponent<Chunk>().CreateChunk(chunkDimensions, position);
                    loadingBar.value++;
                    yield return null;
                }
            }
        }

        mCamera.SetActive(false);

        float xpos = worldDimensions.x * chunkDimensions.x / 2f;
        float zpos = worldDimensions.z * chunkDimensions.x / 2;
        Chunk c = chunkPrefab.GetComponent<Chunk>();
        float ypos = MeshUtils.fBM(xpos, zpos, c.octaves, c.scale, c.heightScale, c.heightOffset) + 10;
        fpc.transform.position = new Vector3(xpos, ypos, zpos);
        loadingBar.gameObject.SetActive(false);
        fpc.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }
}