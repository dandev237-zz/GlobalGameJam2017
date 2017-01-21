using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeed : MonoBehaviour {

    public float speedMultiplier;
    public float accelerationSeconds, timeCounter;
    private bool accelerated;
    private float originalSpeed;

    private void Start()
    {
        accelerated = false;
        timeCounter = 0.0f;
    }

    // Update is called once per frame
    void Update () {
        if (accelerated)
        {
            timeCounter += Time.deltaTime;
            if(timeCounter >= accelerationSeconds)
            {
                accelerated = false;
                PlayerController.speed = originalSpeed;
                timeCounter = 0.0f;
            }
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!accelerated && collision.gameObject.tag.Equals("PickupSpeed"))
        {
            accelerated = true;
            originalSpeed = PlayerController.speed;
            PlayerController.speed = speedMultiplier;
        }
    }
}
