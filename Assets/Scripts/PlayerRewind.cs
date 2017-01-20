using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewind : MonoBehaviour {

    public GameObject player;
    public float rewindDistance;
    private Stack<PlayerPosition> recordedPositions;

	// Use this for initialization
	void Start () {
        recordedPositions = new Stack<PlayerPosition>();
	}
	
    public void recordPosition(bool isAccelerated)
    {
        PlayerPosition positionToRecord = new PlayerPosition(player.transform, isAccelerated);
        recordedPositions.Push(positionToRecord);
    }

    public void rewind()
    {
        bool keepLooking = true;

        List<Vector2> rewindVectors = new List<Vector2>();
        while(recordedPositions.Count > 0 && keepLooking)
        {
            PlayerPosition recordedPosition = recordedPositions.Pop();
            if(recordedPosition.getPosition().x < player.transform.position.x - rewindDistance)
            {
                //Nos hemos pasado de distancia de rewind, parar de hacer vectores
                keepLooking = false;
            }
            else
            {

            }
        }
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

        public Vector2 getPosition()
        {
            return position;
        }

        public bool getIsAccelerated()
        {
            return isAccelerated;
        }
    }
}
