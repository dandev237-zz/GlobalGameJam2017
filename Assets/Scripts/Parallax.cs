using UnityEngine;
using UnityEngine.UI;

public class Parallax : MonoBehaviour
{
    public float speed = 0.1f;
    private Rect rect;

    public void Awake()
    {
        rect = this.gameObject.GetComponent<RawImage>().uvRect;
    }

    // Update is called once per frame
    void Update()
    {
        rect.x += speed * Time.deltaTime;
        this.GetComponent<RawImage>().uvRect = rect;
    }
}
