using UnityEngine;

public class ButtonPressEffect : MonoBehaviour
{
    private Sprite pressed;
    private Sprite button;
    private SpriteRenderer rend;

    public void Awake()
    {
        pressed = (Sprite)Resources.Load<Sprite>("Sprites/buttonPressed");
        button = (Sprite)Resources.Load<Sprite>("Sprites/button");
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
