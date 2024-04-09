using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcoesObjeto : MonoBehaviour
{
    private IdentificarObjetos idObjetos;
    private bool pegou = false;

    // Start is called before the first frame update
    void Start()
    {
        idObjetos = GetComponent<IdentificarObjetos>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && idObjetos.GetObjPegar() != null)
        {
            //print("Update - Pegar");
            Pegar();
        }

    
        if (Input.GetKeyDown(KeyCode.F) && idObjetos.GetObjArrastar() != null)
        {
            if (!pegou)
            {
                Arrastar();
                //print("Arrastar");
            }
            else
            {
                Soltar();
                //print("Soltar");
            }
            pegou = !pegou;
        }
        
    }

    private void Pegar()
    {
        IPegavel obj = idObjetos.GetObjPegar().GetComponent<IPegavel>();
        if (obj != null) {
            obj.Pegar();
        }
        
        Destroy(idObjetos.GetObjPegar());
        idObjetos.EsconderTexto();
    }

    private void Soltar()
    {
        GameObject obj = idObjetos.GetObjArrastar();
        Destroy(obj.GetComponent<DragDrop>());
        idObjetos.enabled = true;
    }

    private void Arrastar()
    {
        GameObject obj = idObjetos.GetObjArrastar();
        obj.AddComponent<DragDrop>();
        obj.GetComponent<DragDrop>().Ativar();
        idObjetos.enabled = false;
    }

    
    
}