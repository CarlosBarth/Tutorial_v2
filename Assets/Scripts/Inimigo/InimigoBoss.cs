using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoBoss : InimigoBase, ILevarDano
{
    public override bool IsBoss() 
    {
        return true;
    } 

    // Update is called once per frame
    void Update()
    {
        OlharParaJogador(); 
        VerificaVida();

        if (fov.podeVerPlayer) 
        {
            anim.SetTrigger("roar");
            VaiAtrasJogador();
        } else 
        {
            anim.SetBool("pararAtaque", true);
            CorrigirRigidSair();
            agente.isStopped = false;
            pal.Andar(anim);
        }
    }

    private void VaiAtrasJogador() 
    {
        float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanciaDoPlayer < distanciaDoAtaque) 
        {
            agente.isStopped = true;
            Debug.Log("Boss - Ataque");

            anim.SetTrigger("ataque");
            anim.SetBool("podeAndar", false);
            anim.SetBool("pararAtaque", false);
            CorrigirRigidEntrar();
        }  

        if (distanciaDoPlayer >= (distanciaDoAtaque + 1)) 
        {
            print("Boss - parar-ataque");
            anim.SetBool("pararAtaque", true);
            CorrigirRigidSair();
        }

        if (anim.GetBool("podeAndar"))
        {
            agente.isStopped = false;
            agente.SetDestination(player.transform.position);
            anim.ResetTrigger("ataque");
        }
    }

    private void OlharParaJogador()
    {
        Vector3 direcaoOlhar = player.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlhar);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300);
    }

    private void CorrigirRigidEntrar() 
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void CorrigirRigidSair() 
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void LevarDano(int dano) 
    {
        vida-= dano;
        agente.isStopped = true;
        anim.SetTrigger("levouTiro");
        anim.SetBool("podeAndar", true);
    }

    private void VerificaVida() 
    {
        if (vida <= 0) 
        {
            Morrer();
        }
    }

    protected override void Morrer() 
    {
        base.Morrer();
        AtualizaPontuacao(30);
        print(this.name);
        if (this.name == "TempleBoss") {
            AbrirPortas();
        }
    }

    public void DarDano() 
    {
        player.GetComponent<MovimentarPersonagem>().AtualizarVida(-20);
    }

    public void Passo() 
    {
        audioSrc.PlayOneShot(somPasso, 0.5f); // O segundo parâmetro identifica o volume do som
    }
}
