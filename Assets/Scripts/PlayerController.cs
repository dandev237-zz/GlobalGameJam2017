using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private float _force = 5;
    private bool jump = false;

    public void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !jump)
        {
            jump = true;
            _rigidBody.AddForce(Vector2.up * _force, ForceMode2D.Impulse);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            jump = false;
        }
    }
}
