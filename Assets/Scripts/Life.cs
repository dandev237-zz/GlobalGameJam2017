using UnityEngine;

public class Life : MonoBehaviour
{
    private static int life;

    public void Awake()
    {
        life = 3;
    }

    public static void Hit()
    {
        life--;
    }

    public static int GetLife()
    {
        return life;
    }
}
