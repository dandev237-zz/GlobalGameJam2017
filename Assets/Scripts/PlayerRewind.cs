using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewind : MonoBehaviour {

    public GameObject player;
    public float rewindDistance;
    private bool rewinding, jumping;
    private Stack<PlayerPosition> recordedPositions;

	void Start () {
        recordedPositions = new Stack<PlayerPosition>();
        rewinding = false;
        jumping = false;
	}

    private void Update()
    {
        if (!rewinding)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !jumping)
            {
                jumping = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!rewinding && jumping && collision.gameObject.tag.Equals("Ground"))
        {
            jumping = false;
            RecordPosition(player.transform, PlayerController.speed);
        }
        else if(!rewinding && !jumping && collision.gameObject.tag.Equals("PickupRewind"))
        {
            Rewind();
            GameObject.Destroy(collision.gameObject);
        }
    }

    private void RecordPosition(Transform playerTransform, float playerSpeed)
    {
        PlayerPosition positionToRecord = new PlayerPosition(playerTransform, playerSpeed);
        recordedPositions.Push(positionToRecord);
    }

    private void Rewind()
    {
        rewinding = true;

        bool keepLooking = true;
        float distanceRewinded = 0.0f;

        if(recordedPositions.Count > 0)
        {
            while (recordedPositions.Count > 0 && keepLooking)
            {
                PlayerPosition recordedPosition = recordedPositions.Pop();
                if (recordedPosition.GetPosition().x < player.transform.position.x - (rewindDistance - distanceRewinded))
                {
                    //The point is farther away than the rewind distance we have left
                    keepLooking = false;

                    player.transform.Translate(new Vector2(player.transform.position.x - (rewindDistance - distanceRewinded), 0.0f) * Time.deltaTime); //CONSIDER SPEED
                }
                else
                {
                    //Store the distance we are about to rewind
                    distanceRewinded += player.transform.position.x - recordedPosition.GetPosition().x;

                    //Create rewind vector
                    Vector2 rewindVector = new Vector2(player.transform.position.x - recordedPosition.GetPosition().x, player.transform.position.y);

                    //Translate the player
                    player.transform.Translate(rewindVector * Time.deltaTime * recordedPosition.GetSpeed()); //CONSIDER SPEED

                    //Issue a jump command
                    PlayerController.Jump();
                }
            }
        }
        else
        {
            //Move player in a straight line towards the maximum possible distance
            player.transform.Translate(new Vector2(player.transform.position.x - rewindDistance, 0.0f) * Time.deltaTime); //CONSIDER SPEED
        }

        //Empty the stack
        recordedPositions.Clear();

        rewinding = false;
    }

    struct PlayerPosition
    {
        Vector2 position;
        float speed;

        public PlayerPosition(Transform playerTransform, float playerSpeed)
        {
            position = playerTransform.position;
            speed = playerSpeed;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public float GetSpeed()
        {
            return speed;
        }
    }
}
