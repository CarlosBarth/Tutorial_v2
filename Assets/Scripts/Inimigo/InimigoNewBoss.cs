using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoNewBoss : InimigoBase, ILevarDano
{
    public override bool IsBoss() 
    {
        return true;
    }

    void Update()
    {       
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle")) 
        {
            agente.isStopped = true;
            anim.SetBool("podeAndar", false);
            StartCoroutine(WaitForAnimation("idle"));
        }
        
        VerificaVida();        

        if (fov.podeVerPlayer) 
        {
            VaiAtrasJogador();                
        } else 
        {
            anim.SetBool("pararAtaque", true);
            anim.SetBool("podeAndar", true);
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
            anim.SetTrigger("ataque");
            anim.SetBool("podeAndar", false);
            anim.SetBool("pararAtaque", false);       
            CorrigirRigidEntrar();
        }  

        if (distanciaDoPlayer >= (distanciaDoAtaque + 1)) 
        {
            anim.SetBool("podeAndar", true);
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


    protected override void Morrer() 
    {
        base.Morrer();
        AtualizaPontuacao(30);
        this.enabled = false;
        GameObject.Destroy(this, 5);
        if (this.name == "TempleBoss") {
            AbrirPortas();
        }
    }

    public void DarDano() 
    {
        player.GetComponent<MovimentarPersonagem>().AtualizarVida(-20);
    }

    public void LevarDano(int dano) 
    {
        if (agente.enabled != true) {
            return;
        }
        vida-= dano;
        agente.isStopped = true;
        anim.SetTrigger("levouTiro");
        anim.SetBool("podeAndar", false);
        // StartCoroutine(WaitForAnimation("hit"));
        if (this.enabled) 
        {
            VaiAtrasJogador();
            print(this.name);
        }
        
    }
}
