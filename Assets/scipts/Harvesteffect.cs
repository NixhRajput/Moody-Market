using UnityEngine;
using System.Collections;

public class HarvestEffect : MonoBehaviour
{
    [Header("Animation Prefabs (each has Animator on it)")]
    public GameObject carrotAnimPrefab;
    public GameObject tomatoAnimPrefab;
    public GameObject wheatAnimPrefab;

    [Header("Crop Icons for floating popup")]
    public Sprite carrotIcon;
    public Sprite tomatoIcon;
    public Sprite wheatIcon;

    public void PlayHarvest(CropType cropType, Vector3 tilePosition)
    {
        GameObject animPrefab = cropType switch
        {
            CropType.Carrot => carrotAnimPrefab,
            CropType.Tomato => tomatoAnimPrefab,
            CropType.Wheat => wheatAnimPrefab,
            _ => null
        };

        Sprite icon = cropType switch
        {
            CropType.Carrot => carrotIcon,
            CropType.Tomato => tomatoIcon,
            CropType.Wheat => wheatIcon,
            _ => null
        };

        if (animPrefab != null)
        {
            GameObject anim = Instantiate(animPrefab, tilePosition, Quaternion.identity);
            Animator animator = anim.GetComponent<Animator>();
            if (animator != null)
            {
                float clipLength = animator.GetCurrentAnimatorStateInfo(0).length;
                if (clipLength <= 0) clipLength = 1f;
                Destroy(anim, clipLength + 0.1f);
            }
            else
            {
                Destroy(anim, 1.2f);
            }
        }

        if (icon != null)
            StartCoroutine(FloatIcon(icon, tilePosition));
    }

    IEnumerator FloatIcon(Sprite icon, Vector3 startPos)
    {
        GameObject obj = new GameObject("FloatingIcon");
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = icon;
        sr.sortingOrder = 10;
        obj.transform.position = startPos + Vector3.up * 0.5f;
        obj.transform.localScale = Vector3.zero;

        float elapsed = 0f;
        float popDuration = 0.2f;
        float floatDuration = 0.6f;
        float fadeDuration = 0.3f;

        while (elapsed < popDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / popDuration;
            obj.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 0.4f, t);
            yield return null;
        }

        elapsed = 0f;
        Vector3 basePos = obj.transform.position;
        while (elapsed < floatDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / floatDuration;
            obj.transform.position = basePos + Vector3.up * (t * 0.5f);
            yield return null;
        }

        elapsed = 0f;
        Color c = sr.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            sr.color = c;
            yield return null;
        }

        Destroy(obj);
    }
}