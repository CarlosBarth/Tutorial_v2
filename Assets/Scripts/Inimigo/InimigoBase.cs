using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InimigoBase : MonoBehaviour, ILevarDano
{
    protected NavMeshAgent agente;
    protected GameObject player;
    protected Animator anim;
    public float distanciaDoAtaque = 2.0f;
    public int vida = 50;
    public AudioClip somMorte;
    public AudioClip somPasso;
    public AudioClip somPunch;
    public AudioClip somKick;
    public AudioClip somRugido;
    public AudioSource audioSrc;

    protected FieldOfView fov;
    protected PatrulharAleatorio pal;

    public bool jahGrunhiu = false;
    

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

    // Update is called once per framessssss
    void Update()
    {       
        anim.SetBool("andando", !agente.isStopped);
        if (!agente.enabled || anim.GetCurrentAnimatorStateInfo(0).IsName("roar")) 
        {
            anim.ResetTrigger("roar");
            return;
        }

        if (vida <= 0) 
        {
            Morrer();
        }         
                
        // StartCoroutine(WaitForAnimation("hit"));
        if (fov.podeVerPlayer) 
        {
            VaiAtrasJogador();
            // OlharParaJogador();
            // anim.SetTrigger("roar");
            
            // if (!anim.GetBool("jahRosnou")) 
            // {
            //     agente.isStopped = true;
            //     anim.Play("Mutant Roaring");
            //     audioSrc.clip = somRugido;
            //     audioSrc.Play();
            //     anim.SetBool("jahRosnou", true);
            //     StartCoroutine(WaitForAnimation("Mutant Roaring"));
            // } else
            // {
                
        } else 
        {
        //     anim.ResetTrigger("roar");
            anim.SetBool("pararAtaque", true);
            CorrigirRigidSair();
            // agente.isStopped = false;
            pal.Andar(anim);
        }
    }

    IEnumerator WaitForAnimation(string animationName, int adcTime = 0)
    {
        // Aguarda até que a animação esteja sendo reproduzida
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null;
        }
        
        // Agora, espera a animação terminar
        float animationLength = anim.GetCurrentAnimatorStateInfo(0).length;
        float length = (animationLength + adcTime);
        print(animationName);
        print(length);
        print(Time.deltaTime);
        
        yield return new WaitForSeconds(length);
        
        // Aqui a animação terminou, mover o NPC em direção ao jogador
        // print("foi atras");
        // VaiAtrasJogador();
    }

    private void VaiAtrasJogador() 
    {
        if (!jahGrunhiu) 
        {
            agente.isStopped = true;
                anim.Play("roar");
                audioSrc.clip = somRugido;
                audioSrc.Play();
                anim.SetBool("jahRosnou", true);
                StartCoroutine(WaitForAnimation("roar"));
                anim.SetTrigger("roar");
                jahGrunhiu = true;
            print("roar");
        }

        float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);
        // print(distanciaDoPlayer);

        if (distanciaDoPlayer < distanciaDoAtaque) 
        {
            agente.isStopped = true;
            anim.SetTrigger("ataque");
            anim.SetBool("podeAndar", false);
            anim.SetBool("pararAtaque", false);
            // StartCoroutine(WaitForAnimation("atack"));
            CorrigirRigidEntrar();
        }  else 
        {
            agente.isStopped = false;
            anim.SetBool("podeAndar", true);
            anim.SetBool("pararAtaque", true);
            agente.SetDestination(player.transform.position);
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
        print("olhar");
        if (this.enabled) 
        {
            Vector3 direcaoOlhar = player.transform.position - transform.position;
            Quaternion rotacao = Quaternion.LookRotation(direcaoOlhar);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300);
        }
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
        anim.SetBool("podeAndar", false);
        StartCoroutine(WaitForAnimation("hit"));
        if (this.enabled) 
        {
            VaiAtrasJogador();
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
        anim.SetBool("dead", true);
        this.enabled = false;
        fov.enabled = false;
        GameObject.Destroy(agente, 10);
    }

    public void DarDano() 
    {
        player.GetComponent<MovimentarPersonagem>().AtualizarVida(-10);
        
    }

    public void Passo() 
    {
        audioSrc.PlayOneShot(somPasso, 0.5f); // O segundo parâmetro identifica o volume do som
    }
    
    public void Soco() 
    {
        audioSrc.PlayOneShot(somPunch, 0.5f); // O segundo parâmetro identifica o volume do som
    }

    public void Chute() 
    {
        audioSrc.PlayOneShot(somKick, 0.5f); // O segundo parâmetro identifica o volume do som
    }
}
