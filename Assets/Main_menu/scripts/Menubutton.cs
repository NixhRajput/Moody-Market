using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite pressedSprite;
    public string buttonId;
    public SpriteRenderer buttonSpriteRenderer;

    void Awake()
    {
        if (buttonSpriteRenderer == null)
            buttonSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if (pressedSprite != null)
            buttonSpriteRenderer.sprite = pressedSprite;
    }

    void OnMouseUp()
    {
        if (normalSprite != null)
            buttonSpriteRenderer.sprite = normalSprite;

        MenuManager.Instance.OnButtonClicked(buttonId);
    }
}