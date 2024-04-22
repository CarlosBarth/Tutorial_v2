using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVida : MonoBehaviour, IPegavel
{

    public int vida = 25;
    protected MensagemTemporaria mensagemTemporaria;

    public void Pegar()
    {
        MovimentarPersonagem player = GameObject.FindWithTag("Player").GetComponent<MovimentarPersonagem>();
        player.AtualizarVida(vida);
        if (player.GetVida() >= 100) {
            mensagemTemporaria.MostrarMensagem("Vida Cheia!");
            return;
        }
        player.AtualizarVida(vida);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        mensagemTemporaria = canvas.GetComponent<MensagemTemporaria>();
        if (mensagemTemporaria == null)
        {
            Debug.LogError("Não foi possível encontrar o componente MensagemTemporaria no Canvas.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
