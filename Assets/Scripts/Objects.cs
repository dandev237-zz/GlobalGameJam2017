using UnityEngine;

public class Objects : MonoBehaviour
{

    private float forceUp = 200;
    private float forceLeft = 1000;
    private Rigidbody2D _rigidBody;
    public Sprite _sprite;
    private SpriteRenderer _rend;

    // Use this for initialization
    void Start()
    {
        _rend = GetComponent<SpriteRenderer>();
        Invoke("Delete", 10);

    }


    private void Delete()
    {
        Destroy(this.gameObject);
    }

    public void Shoot(Transform transform)
    {
        this.transform.position = transform.position;
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.AddForce(Vector2.up * forceUp);
        _rigidBody.AddForce(Vector2.left * forceLeft);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (_sprite != null)
        {
            _rend.sprite = _sprite;
        }
    }
}
