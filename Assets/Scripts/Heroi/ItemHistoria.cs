using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHistoria : MonoBehaviour, IPegavel
{

    public string texto;
    public string NomeProximaPista;
    protected MensagemTemporaria mensagemTemporaria;

    public void Pegar()
    {
        mensagemTemporaria.MostrarMensagem(texto, 12);
        UnlockNextKey();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        mensagemTemporaria = canvas.GetComponent<MensagemTemporaria>();
    }

    void UnlockNextKey()
    {
        GameObject MyObject = FindInActiveObjectByName(GameObject.Find("ItensChave").transform, NomeProximaPista);
        if(MyObject != null)
        {
            MyObject.SetActive(true);
        }
    }

    public GameObject FindInActiveObjectByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.name == name) return child.gameObject;
            GameObject result = FindInActiveObjectByName(child, name);
            if (result != null) return result;
        }
        return null;
    }

}
