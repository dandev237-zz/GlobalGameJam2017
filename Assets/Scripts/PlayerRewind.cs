using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewind : MonoBehaviour {

    public GameObject player;
    public float rewindTime;
    private float elapsedTime, rewindActivationTime, remainingRewindTime;
    private bool rewinding, jumping;
    private Stack<PlayerPosition> recordedPositions;

	void Start () {
        recordedPositions = new Stack<PlayerPosition>();
        elapsedTime = 0.0f;
        rewindActivationTime = 0.0f;
        rewindTime = 2.0f;
        rewinding = false;
        jumping = false;
	}

    private void Update()
    {

        if (!rewinding)
        {
            elapsedTime += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) && !jumping)
            {
                jumping = true;
            }
        }
        else
        {
            elapsedTime -= Time.deltaTime * 10;
            if(elapsedTime <= recordedPositions.Peek().GetTime())
            {
                jumping = true;
                PlayerController.Jump();
                recordedPositions.Pop();
            }
            if(rewindActivationTime - rewindTime >= elapsedTime)
            {
                rewinding = false;
                Parallax.speed = 0.1f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!rewinding && jumping && collision.gameObject.tag.Equals("Ground"))
        {
            jumping = false;
            RecordPosition(elapsedTime, Parallax.speed);
        }
        else if(!rewinding && !jumping && collision.gameObject.tag.Equals("PickupRewind"))
        {
            Rewind();
            GameObject.Destroy(collision.gameObject);
        }
    }

    private void RecordPosition(float jumpTime, float playerSpeed)
    {
        PlayerPosition positionToRecord = new PlayerPosition(jumpTime, playerSpeed);
        recordedPositions.Push(positionToRecord);
    }

    private void Rewind()
    {
        rewinding = true;
        rewindActivationTime = elapsedTime;

        Parallax.speed = -1f;
    }

    struct PlayerPosition
    {
        float time;
        float speed;

        public PlayerPosition(float jumpTime, float playerSpeed)
        {
            time = jumpTime;
            speed = playerSpeed;
        }

        public float GetTime()
        {
            return time;
        }

        public float GetSpeed()
        {
            return speed;
        }
    }
}
