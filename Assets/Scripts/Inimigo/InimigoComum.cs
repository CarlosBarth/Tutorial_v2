using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoComum : MonoBehaviour, ILevarDano
{
    private UnityEngine.AI.NavMeshAgent agente;
    private GameObject player;
    private Animator anim;
    public float distanciaDoAtaque = 2.0f;
    public int vida = 50;
    public AudioClip somMorte;
    public AudioClip somPasso;
    public AudioSource audioSrc;

    private FieldOfView fov;
    private PatrulharAleatorio pal;

    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();

        fov = GetComponent<FieldOfView>();
        pal = GetComponent<PatrulharAleatorio>();
    }

    // Update is called once per frame
    void Update()
    {
        OlharParaJogador(); 
        VerificaVida();

        if (fov.podeVerPlayer) 
        {
            VaiAtrasJogador();
        } else 
        {
            anim.SetBool("pararAtaque", true);
            CorrigirRigidSair();
            agente.isStopped = false;
            pal.Andar();
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
            print("ataque");
            CorrigirRigidEntrar();
        }  

        if (distanciaDoPlayer >= (distanciaDoAtaque + 1)) 
        {
            //print("parar-ataque");
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

    private void Morrer() 
    {
        audioSrc.clip = somMorte;
        audioSrc.Play();

        agente.isStopped = true;
        anim.SetBool("podeAndar", false);
        anim.SetBool("pararAtaque", true);
        anim.SetBool("morreu", true);

        this.enabled = false;
    }

    public void DarDano() 
    {
        player.GetComponent<MovimentarPersonagem>().AtualizarVida(-10);
        print("dar dano");
    }

    public void Passo() 
    {
        audioSrc.PlayOneShot(somPasso, 0.5f); // O segundo parâmetro identifica o volume do som
    }
}
