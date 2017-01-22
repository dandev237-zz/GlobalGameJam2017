using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rrjscrObstaculo : MonoBehaviour
{
    public int velocidad = 6;
    // Use this for initialization
    void Start()
    {
        Invoke("Delete", 10);
    }

    public void Delete()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * velocidad * Time.deltaTime);
    }

    public void Reiniciar(Transform transform)
    {
        this.transform.position = transform.position;
        if (this.tag.Equals("disco"))
        {
            this.transform.position = new Vector3(this.transform.position.x, -0.2f, this.transform.position.z);
        }
    }
}
