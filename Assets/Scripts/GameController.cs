using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    private ChunkFactory chunkFactory;
    public Camera worldCamera;
    public PlayerController player;
    private UIController UI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UI = UIController.Instance;
        chunkFactory = UIController.ChunkFactory;
        chunkFactory.TheWorldHasFinishedGenerating += OnWorldGenerated;
        float chunkWidth = chunkFactory.CHUNK_WIDTH;
        float chunkHeight = chunkFactory.CHUNK_HEIGHT;
        int worldSize = (int)chunkFactory.worldsize;
        float halfWorldSize = (chunkWidth * worldSize - worldSize) * 0.5F;
        worldCamera.transform.position = new Vector3(halfWorldSize,chunkHeight,halfWorldSize);
        worldCamera.orthographicSize = halfWorldSize;
    }

    private void OnWorldGenerated()
    {
        //Spawn player at the same location as the world camera
        UI.Player = Instantiate(player, worldCamera.transform.position, Quaternion.identity);
    }
}
