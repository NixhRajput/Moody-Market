using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public Sprite pressedSprite;
    public string buttonId;
    public SpriteRenderer buttonSpriteRenderer;

    private Sprite normalSprite;

    void Start()
    {
        if (buttonSpriteRenderer == null)
            buttonSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (buttonSpriteRenderer != null)
            normalSprite = buttonSpriteRenderer.sprite;
    }

    void OnMouseDown()
    {
        if (pressedSprite != null)
            buttonSpriteRenderer.sprite = pressedSprite;
    }

    void OnMouseUp()
    {
        buttonSpriteRenderer.sprite = normalSprite;
        Debug.Log($"[Button] AudioManager null? {AudioManager.Instance == null}");
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonBeep();
        MenuManager.Instance.OnButtonClicked(buttonId);
    }
}