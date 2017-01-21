using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static Rigidbody2D _rigidBody;
    private static float _force = 5;
    private static bool jump = false;
    public static float speed { get; set; }
    private static Animator _animator, _armAnimator, _wavesAnimator;

    public void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _animator = this.GetComponent<Animator>();
        _armAnimator = GameObject.Find("Player/Player/arm").GetComponent<Animator>();
        _wavesAnimator = GameObject.Find("Player/Player/waves").GetComponent<Animator>();
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
        _animator.Play("jump");
        _armAnimator.Play("jumpArm");
    }

    public static void Scream()
    {
        _armAnimator.Play("scream");
        _wavesAnimator.Play("waves");
    }
}
