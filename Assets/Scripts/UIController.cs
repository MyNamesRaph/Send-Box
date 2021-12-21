using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    public ChunkFactory chunkFactoryPrototype;
    public static ChunkFactory ChunkFactory { get; private set; }
    public PlayerController Player { get; set; }

    [Header("UIComponents")]
    public GameObject mainMenu = null;
    public GameObject createWorld = null;
    public GameObject worldLoading = null;
    public GameObject playerHUD = null;
    public GameObject itemPanel = null;

    [Header("UIInputs")]
    public InputField seed;
    public InputField amplification;
    public InputField heightAmplification;
    public InputField caves;
    public InputField scale;
    public InputField heightScale;
    public TMP_Dropdown worldSize;

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
        if (ChunkFactory != null)
        {
            ChunkFactory.TheWorldHasFinishedGenerating += OnWorldGenerated;
        }
        if (chunkFactoryPrototype != null)
        {
            seed.text = Random.Range(int.MinValue, int.MaxValue).ToString();
            amplification.text = chunkFactoryPrototype.amplification.ToString();
            heightAmplification.text = chunkFactoryPrototype.heightAmplification.ToString();
            caves.text = chunkFactoryPrototype.caveThreshold.ToString();
            scale.text = chunkFactoryPrototype.scale.ToString();
            heightScale.text = chunkFactoryPrototype.heightScale.ToString();
        }
    }

    public void OnPlayButtonPressed()
    {
        mainMenu.SetActive(false);
        createWorld.SetActive(true);
    }

    private void OnWorldGenerated()
    {
        worldLoading.SetActive(false);
        playerHUD.SetActive(true);

        foreach (PlaceableObject item in Player.items)
        {
            GameObject panel = Instantiate(itemPanel, playerHUD.transform);
            panel.GetComponent<RawImage>().texture = item.itemTexture;
        }
    }

    public void OnCreateWorldButtonPressed()
    {
        //Set all parameters
        long.TryParse(seed.text, out chunkFactoryPrototype.seed);
        float.TryParse(amplification.text, out chunkFactoryPrototype.amplification);
        float.TryParse(heightAmplification.text, out chunkFactoryPrototype.heightAmplification);
        float.TryParse(caves.text, out chunkFactoryPrototype.caveThreshold);
        float.TryParse(scale.text, out chunkFactoryPrototype.scale);
        float.TryParse(heightScale.text, out chunkFactoryPrototype.heightScale);

        if (worldSize != null)
        {
            switch (worldSize.value)
            {
                case 0:
                    chunkFactoryPrototype.worldsize = ChunkFactory.WorldSize.Tiny;
                    break;
                case 1:
                    chunkFactoryPrototype.worldsize = ChunkFactory.WorldSize.Small;
                    break;
                case 2:
                    chunkFactoryPrototype.worldsize = ChunkFactory.WorldSize.Medium;
                    break;
                case 3:
                    chunkFactoryPrototype.worldsize = ChunkFactory.WorldSize.Large;
                    break;
                case 4:
                    chunkFactoryPrototype.worldsize = ChunkFactory.WorldSize.Huge;
                    break;
                default:
                    break;
            }
        }

        //Create the ChunkFactory and pass it unto the gameplay scene
        ChunkFactory = Instantiate(chunkFactoryPrototype);
        DontDestroyOnLoad(ChunkFactory);
        SceneManager.LoadScene(1);
    }
}
