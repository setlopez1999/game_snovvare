using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaPies : MonoBehaviour
{
    // Start is called before the first frame update

    public Personaje logicaPersonaje;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider collider){
        logicaPersonaje.puedoSaltar = true;

    }

    private void OnTriggerExit(Collider collider){
        logicaPersonaje.puedoSaltar = false;
    }
}
