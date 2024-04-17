using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVida : MonoBehaviour, IPegavel
{

    public int vida = 25;

    public void Pegar()
    {
        MovimentarPersonagem player = GameObject.FindWithTag("Player").GetComponent<MovimentarPersonagem>();
        player.AtualizarVida(vida);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
