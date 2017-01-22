using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeed : MonoBehaviour
{

    public float speedMultiplier;
    public float accelerationSeconds, timeCounter;
    private bool accelerated;
    private float originalSpeed;

    void Start()
    {
        accelerated = false;
        timeCounter = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (accelerated)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= accelerationSeconds)
            {
                accelerated = false;
                Parallax.speed = originalSpeed;
                timeCounter = 0.0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!accelerated && collision.gameObject.tag.Equals("PickupSpeed"))
        {
            Collission();
            collision.gameObject.SetActive(false);
        }
    }

    public void Collission()
    {
        accelerated = true;
        originalSpeed = Parallax.speed;
        Parallax.speed = originalSpeed * speedMultiplier;
    }
}
