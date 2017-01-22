using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generador : MonoBehaviour
{
    private int nobjetos;
    private int segundos = 1;
    public List<GameObject> obstaculos/*, objetos*/;
    public bool ground = false;
    private bool instanciados = false, finish = false;
    private int iobjeto = 0;
    private GameObject disco;

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
    }

    private void AparecerObstaculo()
    {
        if (Random.Range(0, 101) >= 30)
        {
            //if (instanciados)
            //{
            //    int rnd = Random.Range(0, obstaculos.Count);
            //    var obstaculoA = objetos[rnd];
            //    obstaculoA.SetActive(true);
            //    obstaculoA.GetComponent<rrjscrObstaculo>().Reiniciar(this.transform);
            //    iobjeto++;
            //}
            //else
            //{
            if (iobjeto < 5)
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
            //GameObject objeto = Instantiate(obstaculos[iobjeto]);
            //objeto.GetComponent<rrjscrObstaculo>().Reiniciar(this.transform);
            //objetos.Add(objeto);
            // iobjeto++;
            //if (iobjeto - 1 == nobjetos)
            //{
            //    instanciados = true;
            //}
            // }
        }
        //if (iobjeto == nobjetos)
        //{
        //    iobjeto = 0;
        //}
        //Cambiamos el flag para hacer aparecer un obstaculo
        Invoke("AparecerObstaculo", segundos);
    }
}