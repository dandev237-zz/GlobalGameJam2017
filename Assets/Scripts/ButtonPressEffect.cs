using UnityEngine;

public class ButtonPressEffect : MonoBehaviour
{
    private Sprite pressed;
    private Sprite button;
    private SpriteRenderer rend;

    public void Awake()
    {
        pressed = Resources.Load<Sprite>("Sprites/buttonPressed");
        button = Resources.Load<Sprite>("Sprites/button");
        rend = this.GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        rend.sprite = pressed;
    }

    public void OnMouseUp()
    {
        rend.sprite = button;
    }
}
