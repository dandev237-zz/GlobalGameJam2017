using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewind : MonoBehaviour {

    public GameObject player;
    public float rewindDistance;
    private Stack<PlayerPosition> recordedPositions;

	void Start () {
        recordedPositions = new Stack<PlayerPosition>();
	}
	
    public void RecordPosition(bool isAccelerated)
    {
        PlayerPosition positionToRecord = new PlayerPosition(player.transform, isAccelerated);
        recordedPositions.Push(positionToRecord);
    }

    public void Rewind()
    {
        bool keepLooking = true;
        float distanceRewinded = 0.0f;

        List<Vector2> rewindVectors = new List<Vector2>();
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
                    player.transform.Translate(rewindVector * Time.deltaTime); //CONSIDER SPEED

                    //Get the landing point for the jump
                    PlayerPosition landingPoint = recordedPositions.Pop();

                    //Jump from player point to landing point with curvilinear trajectory
                    //TODO
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
    }

    struct PlayerPosition
    {
        Vector2 position;
        bool isAccelerated;

        public PlayerPosition(Transform playerTransform, bool accelerated)
        {
            position = playerTransform.position;
            isAccelerated = accelerated;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public bool GetIsAccelerated()
        {
            return isAccelerated;
        }
    }
}
