using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Detectamos que el player controler entro a una zona de muerte
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            /*
             * Instansiamos / creamos la variable controller del tipo PlayerController
             * para invocar la muerte del personaje recuperando la componente PlayerController.
             */
            PlayerController controller = collision.GetComponent<PlayerController>();
            
            controller.Die(); // Declaramos la muerte del personaje.
        }
    }
}
