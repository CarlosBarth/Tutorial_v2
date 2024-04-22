using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InimigoComum : InimigoBase, ILevarDano
{
    public bool colocaOvos = false;

    public override bool IsBoss() 
    {
        return false;
    } 

    // Update is called once per frame
    protected override void FixedUpDate()
    {
        WaitForAnimation("PutTheDamnEgg");
        tempoAcumulado += Time.deltaTime;
        if (tempoAcumulado >= intervalo && colocaOvos) {
            ColocarOvo(); // isso mesmo, um ovo!
            tempoAcumulado = 0f; // Resetar o contador
        }
        base.FixedUpDate();
        // OlharParaJogador(); 
        // VerificaVida();

        // if (fov.podeVerPlayer) 
        // {
        //     VaiAtrasJogador();
                        
        // } else 
        // {
        //     anim.SetBool("pararAtaque", true);
        //     CorrigirRigidSair();
        //     agente.isStopped = false;
        //     pal.Andar(anim);
            
        // }
    }
    
    private void ColocarOvo() 
    {
        bool jahEstaColocandoOvo = anim.GetCurrentAnimatorStateInfo(0).IsName("PutTheDamnEgg"); 
        if (!jahEstaColocandoOvo) 
        {
            anim.Play("PutTheDamnEgg");
            agente.isStopped = true;
            anim.SetBool("podeAndar", false);
           
            // Instancia o ovo na posição do NPC
            Instantiate(eggPrefab, transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
           
            agente.isStopped = false;
            anim.SetBool("podeAndar", true);
        }
    }

    IEnumerator WaitForAnimation(string animationName)
    {
        // print("iterator");
        // Aguarda até que a animação esteja sendo reproduzida
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null;
        }

        // Agora, espera a animação terminar
        float animationLength = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);
        
        // Aqui a animação terminou, mover o NPC em direção ao jogador
        // print("foi atras");
        VaiAtrasJogador();
    }

    private void VaiAtrasJogador() 
    {
        float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);
        // print(distanciaDoPlayer);
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
            // print("parar-ataque");
            anim.SetBool("pararAtaque", true);
            CorrigirRigidSair();
        }

        if (anim.GetBool("podeAndar"))
        {
            // print("get destination");
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
        print("dead");
        countEnemyTemple++;
        
        if (countEnemyTemple >= 1) 
        {
            AbrirPortaBoss();
            mensagemTemporaria.MostrarMensagem("Uma porta foi destrancada!");
        } 
        AtualizaPontuacao(10);
    }

}
