using UnityEngine;

public enum TileState { Unplowed, Plowed, Planted, Stage1, Stage2, Stage3 }
public enum CropType { None, Carrot, Tomato, Wheat }

public class CropTile : MonoBehaviour
{
    [Header("Child References")]
    public GameObject unplowedObject;
    public GameObject plowedObject;
    public GameObject cropObject;

    [Header("Crop Sprites")]
    public Sprite[] carrotStages;
    public Sprite[] tomatoStages;
    public Sprite[] wheatStages;

    [HideInInspector] public TileState state = TileState.Unplowed;
    [HideInInspector] public CropType cropType = CropType.None;

    private SpriteRenderer cropRenderer;
    private float growTimer = 0f;
    public float growInterval = 5f;
    private int growStage = 0;

    void Awake()
    {
        cropRenderer = cropObject.GetComponent<SpriteRenderer>();
        plowedObject.SetActive(false);
        cropObject.SetActive(false);
    }

    void Update()
    {
        if (state == TileState.Planted || state == TileState.Stage1 || state == TileState.Stage2)
        {
            growTimer += Time.deltaTime;
            if (growTimer >= growInterval)
            {
                growTimer = 0f;
                AdvanceGrowth();
            }
        }
    }

    public void Plow()
    {
        if (state != TileState.Unplowed) return;
        Destroy(unplowedObject);
        plowedObject.SetActive(true);
        state = TileState.Plowed;
        Debug.Log("Tile plowed!");
    }

    public void Plant(CropType crop)
    {
        if (state != TileState.Plowed) return;
        cropType = crop;
        growStage = 0;
        growTimer = 0f;
        cropObject.SetActive(true);
        state = TileState.Planted;
        AdvanceGrowth();
        Debug.Log("Planted: " + crop);
    }

    public string Harvest()
    {
        if (state != TileState.Stage3) return null;
        string result = cropType.ToString();
        cropObject.SetActive(false);
        cropType = CropType.None;
        growStage = 0;
        state = TileState.Plowed;
        return result;
    }

    void AdvanceGrowth()
    {
        growStage++;

        Sprite[] stages = cropType switch
        {
            CropType.Carrot => carrotStages,
            CropType.Tomato => tomatoStages,
            CropType.Wheat => wheatStages,
            _ => null
        };

        if (stages != null && growStage - 1 < stages.Length)
            cropRenderer.sprite = stages[growStage - 1];

        state = growStage switch
        {
            1 => TileState.Planted,
            2 => TileState.Stage1,
            3 => TileState.Stage2,
            _ => TileState.Stage3
        };

        Debug.Log("Crop stage: " + state);
    }
}