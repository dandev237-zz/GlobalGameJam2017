using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generador : MonoBehaviour
{
    private int nobjetos;
    private float segundos = 1;
    public List<GameObject> obstaculos/*, objetos*/;
    public bool ground = false;
    private bool instanciados = false, finish = false;
    private int iobjeto = 0;
    private GameObject disco;
    private float totalLength;

    // Use this for initialization
    void Start()
    {
        Invoke("AparecerObstaculo", segundos);
        nobjetos = obstaculos.Count - 1;
    }

    //Carga antes
    private void Awake()
    {
        //objetos = new List<GameObject>();
        disco = (GameObject)Resources.Load("prefabs/Disco");
        totalLength = GameObject.Find("MusicLoader").GetComponent<MusicLoader>().Length;
    }

    void Update()
    {
        segundos += Time.deltaTime;
    }

    private void AparecerObstaculo()
    {
        if (Random.Range(0, 101) > 1)
        {
            if (totalLength - segundos > 5)
            {
                int rnd = Random.Range(0, obstaculos.Count);
                var obj = Instantiate(obstaculos[rnd]);
                if (ground)
                {
                    obj.GetComponent<rrjscrObstaculo>().Reiniciar(this.transform);
                }
                else
                {
                    obj.GetComponent<Objects>().Shoot(this.transform);
                }
                iobjeto++;
            }
            else
            {
                if (ground && !finish)
                {
                    var discoObj = Instantiate(disco);
                    discoObj.GetComponent<rrjscrObstaculo>().Reiniciar(this.transform);
                    finish = true;
                }
            }
        }
        //Cambiamos el flag para hacer aparecer un obstaculo
        Invoke("AparecerObstaculo", segundos);
    }
}