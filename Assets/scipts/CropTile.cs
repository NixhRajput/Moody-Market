using System.Collections;
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

    [Header("Harvest Animation")]
    public Animator harvestAnimator;
    public string carrotAnimTrigger = "HarvestCarrot";
    public string tomatoAnimTrigger = "HarvestTomato";
    public string wheatAnimTrigger = "HarvestWheat";

    [Header("Floating Icon")]
    public GameObject floatingIconPrefab;
    public Sprite carrotIcon;
    public Sprite tomatoIcon;
    public Sprite wheatIcon;
    public Vector3 floatOffset = new Vector3(0, 1f, 0);

    [HideInInspector] public TileState state = TileState.Unplowed;
    [HideInInspector] public CropType cropType = CropType.None;

    [Header("Grow Times Per Stage (seconds)")]
    public float wheatGrowInterval = 15f;
    public float carrotGrowInterval = 25f;
    public float tomatoGrowInterval = 35f;

    private SpriteRenderer cropRenderer;
    private float growTimer = 0f;
    private int growStage = 0;

    float GetGrowInterval()
    {
        return cropType switch
        {
            CropType.Wheat => wheatGrowInterval,
            CropType.Carrot => carrotGrowInterval,
            CropType.Tomato => tomatoGrowInterval,
            _ => 20f
        };
    }

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
            if (growTimer >= GetGrowInterval())
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

    public (string crop, string seed, int amount) Harvest()
    {
        if (state != TileState.Stage3) return (null, null, 0);

        string cropName = cropType.ToString();
        string seedName = cropName + "Seed";
        CropType harvested = cropType;

        cropObject.SetActive(false);
        cropType = CropType.None;
        growStage = 0;
        state = TileState.Plowed;

        PlayHarvestAnimation(harvested);
        ShowFloatingIcon(harvested);

        return (cropName, seedName, 2);
    }

    void PlayHarvestAnimation(CropType crop)
    {
        if (harvestAnimator == null) return;
        string trigger = crop switch
        {
            CropType.Carrot => carrotAnimTrigger,
            CropType.Tomato => tomatoAnimTrigger,
            CropType.Wheat => wheatAnimTrigger,
            _ => null
        };
        if (trigger != null) harvestAnimator.SetTrigger(trigger);
    }

    void ShowFloatingIcon(CropType crop)
    {
        if (floatingIconPrefab == null) return;

        Sprite icon = crop switch
        {
            CropType.Carrot => carrotIcon,
            CropType.Tomato => tomatoIcon,
            CropType.Wheat => wheatIcon,
            _ => null
        };

        if (icon == null) return;

        GameObject floater = Instantiate(floatingIconPrefab, transform.position + floatOffset, Quaternion.identity);
        SpriteRenderer sr = floater.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = icon;

        StartCoroutine(FloatAndFade(floater));
    }

    IEnumerator FloatAndFade(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        float duration = 1f;
        float elapsed = 0f;
        Vector3 startPos = obj.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            obj.transform.position = startPos + Vector3.up * t * 0.5f;
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 1f - t;
                sr.color = c;
            }
            yield return null;
        }

        Destroy(obj);
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