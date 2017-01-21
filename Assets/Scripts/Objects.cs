using UnityEngine;

public class Objects : MonoBehaviour
{

    public float forceUp = 200;
    public float forceLeft = 500;
    private Rigidbody2D _rigidBody;
    public Sprite _sprite;
    private SpriteRenderer _rend;

    // Use this for initialization
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rend = GetComponent<SpriteRenderer>();
    }

    public void Shoot()
    {
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
