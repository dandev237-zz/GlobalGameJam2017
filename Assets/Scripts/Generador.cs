using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generador : MonoBehaviour {
    GameObject obstaculo;
    public int nobjetos = 6;
    private int segundos = 1;
    private List<GameObject> obstaculos, objetos;
    private bool instanciados = false;
    private int iobjeto = 0;
 
	// Use this for initialization
	void Start () {
        Invoke("AparecerObstaculo", segundos);
	}
	
	// Update is called once per frame
	void Update () {
           
	}

    //Carga antes
    private void Awake()
    {
        //Generamos un array que contendrá los obstaculos
       obstaculos = new List<GameObject>();
        objetos = new List<GameObject>();
       for(int i = 0; i < nobjetos; i++)
        {
            obstaculo = (GameObject)Resources.Load("prefabs/rrjobstaculo");
            obstaculos.Add(obstaculo);
            //obstaculos.Add((GameObject)Resources.Load("prefabs/rrjobstaculo"));
        }
    }

    private void AparecerObstaculo() { 
        if (instanciados) {
            //Mover al principio
            //Debug.Log("Todos instanciados");
            //Debug.Log("iobjetos: " + iobjeto);
            //Debug.Log("Obstaculo:" + obstaculos[iobjeto]);
            var obstaculoA = objetos[iobjeto];
            //obstaculoA.transform.position = this.transform.position;
            obstaculoA.GetComponent<rrjscrObstaculo>().Reiniciar(this.transform);
            iobjeto++;
        } else {
            objetos.Add( Instantiate(obstaculos[iobjeto]));
            iobjeto++;
            if (iobjeto == nobjetos) {
                instanciados = true;
            }
        }
        if (iobjeto == nobjetos)
        {
            iobjeto = 0;
        }
        //Cambiamos el flag para hacer aparecer un obstaculo
        Invoke("AparecerObstaculo", segundos);
    }
}