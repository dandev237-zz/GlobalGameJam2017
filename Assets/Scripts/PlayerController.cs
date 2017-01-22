using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private static Rigidbody2D _rigidBody;
    private static float _force = 5;
    private static bool jump = false, finish = false;
    public static float speed { get; set; }
    private static float timer { get; set; }
    private static Animator _animator, _armAnimator, _wavesAnimator;
    private static Sprite _sprite;
    private static GameObject backgroundPanelWin, panelCredits, _background;
    private bool finished;
    private int life;

    public void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _animator = this.GetComponent<Animator>();
        _armAnimator = GameObject.Find("Player/Player/arm").GetComponent<Animator>();
        _wavesAnimator = GameObject.Find("Player/Player/waves").GetComponent<Animator>();
        _sprite = (Sprite)Resources.Load<Sprite>("Sprites/gameOver");
        _background = GameObject.Find("Background");
        _background.GetComponent<Canvas>().sortingOrder = 0;
        backgroundPanelWin = GameObject.Find("Background/PanelWin");
        panelCredits = GameObject.Find("Background/PanelCredits");
        panelCredits.SetActive(false);
        backgroundPanelWin.SetActive(false);
        finished = false;
        life = 3;
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

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag.Equals("disco"))
        {
            PutBackgroundInFront();
            Destroy(collision.gameObject);
            backgroundPanelWin.SetActive(true);
            backgroundPanelWin.GetComponent<Animator>().Play("WinScreen");
            Invoke("ShowCredits", 5);
            finish = true;
        }
        else if (collision.tag.Equals("Enemy"))
        {
            life--;
            if (life <= 0 && !finished)
            {
                finished = true;
                PutBackgroundInFront();
                GameOver();
            }
        }
    }

    public void ShowCredits()
    {
        panelCredits.SetActive(true);
        panelCredits.GetComponent<Animator>().Play("credits");
        backgroundPanelWin.SetActive(false);
        Invoke("LoadMain", 5);
    }


    public void GameOver()
    {
        backgroundPanelWin.SetActive(true);
        backgroundPanelWin.GetComponent<Image>().sprite = _sprite;
        backgroundPanelWin.GetComponent<Animator>().Play("WinScreen");
        finish = true;
        Invoke("ShowCredits", 5);
    }

    public void LoadMain()
    {
        SceneManager.LoadScene("Main");
    }

    private void PutBackgroundInFront()
    {
        _background.GetComponent<Canvas>().sortingOrder = 999;
    }
}
