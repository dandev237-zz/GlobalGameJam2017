using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rrjscrObstaculo : MonoBehaviour {
    public int velocidad = 6;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
            transform.Translate(Vector3.left * velocidad * Time.deltaTime);
	}

    public void Reiniciar(Transform transform) {
        Debug.Log("Entra en reiniciar.");
        this.transform.position = transform.position;
    }
}
