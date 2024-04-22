using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class InimigoFilhote : InimigoBase, ILevarDano
{
        
    public GameObject EvolucaoFinal;
    public AudioClip somEvolucao;

    public override bool IsBoss() 
    {
        return false;
    }

    // Update is called once per frame
    protected override void FixedUpDate()
    {
        base.FixedUpDate();
    }


    
    protected override void Morrer() 
    {
        base.Morrer();
        print("filhoteDied");
        countEnemyTemple++;
        
        if (countEnemyTemple >= 10) 
        {
            AbrirPortaBoss();
            mensagemTemporaria.MostrarMensagem("Uma porta foi destrancada!");
        } 
        AtualizaPontuacao(5);
    }

    protected override void BeginEvolve() 
    {
        StartCoroutine(Evolve());
    }

    private IEnumerator Evolve()
    {
        yield return new WaitForSeconds(30); // Espera 10 segundos
        DoEvolve();
    }

    private void DoEvolve()
    {
        Instantiate(EvolucaoFinal, transform.position, Quaternion.identity);
        audioSrc.clip = somEvolucao;
        audioSrc.Play();
        // Destrói o ovo após a eclosão
        Destroy(gameObject);
    }

}
