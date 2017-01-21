using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static Rigidbody2D _rigidBody;
    private static float _force = 5;
    private static bool jump = false;

    public void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !jump)
        {
            Jump();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            jump = false;
        }
    }

    public static void Jump()
    {
        jump = true;
        _rigidBody.AddForce(Vector2.up * _force, ForceMode2D.Impulse);
    }
}
