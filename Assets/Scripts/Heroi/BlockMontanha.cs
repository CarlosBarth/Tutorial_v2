using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMontanha : MonoBehaviour
{
    public GameObject texto;

    void OnTriggerEnter(Collider other)
    {
        texto.SetActive(true);
    }

    void OnTriggerExit(Collider other) 
    {
        texto.SetActive(false);
    }

}
