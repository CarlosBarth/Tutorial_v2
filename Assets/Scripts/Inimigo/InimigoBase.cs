using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class InimigoBase : MonoBehaviour, ILevarDano
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
    public Text textoPontuacao;

    protected float tempoAcumulado = 0f;
    protected float intervalo = 20f;
    protected int countEnemyTemple = 0;
    protected MensagemTemporaria mensagemTemporaria;
    
    public abstract bool IsBoss();

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            mensagemTemporaria = canvas.GetComponent<MensagemTemporaria>();
            if (mensagemTemporaria == null)
            {
                Debug.LogError("Não foi possível encontrar o componente MensagemTemporaria no Canvas.");
            }
        }
        else
        {
            Debug.LogError("Não foi possível encontrar um objeto Canvas com o nome especificado.");
        }
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
        if (agente.enabled != true) {
            return;
        }
        vida-= dano;
        agente.isStopped = true;
        anim.SetTrigger("levouTiro");
        anim.SetBool("podeAndar", false);
        StartCoroutine(WaitForAnimation("hit"));
        if (this.enabled) 
        {
            VaiAtrasJogador();
            print(this.name);
        }
        
    }

    protected virtual void Morrer() 
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
        GameObject.Destroy(this, 5);
        GameObject.Destroy(agente, 5);
               
    }

    protected void AtualizaPontuacao(int valor)
    {
        int pontuacaoAtual = int.Parse(textoPontuacao.text);
        int pontuacao = Mathf.CeilToInt(Mathf.Clamp(pontuacaoAtual + valor, 0,9999));
        textoPontuacao.text = pontuacao.ToString();
    }

    public void AbrirPortaBoss() 
{
    GameObject porta = GameObject.FindWithTag("PortaBoss");
    if (porta != null) {
        // Destrancar a porta
        porta.GetComponent<SunTemple.Door>().Destrancar();
        
        // Adicionar um outline à porta
        Outline outline = porta.GetComponent<Outline>();
        if (outline != null) {
            outline.OutlineWidth = 5f;
        }

        // Tocar o som do AudioSource(destrancar)
        AudioSource audioSource = porta.GetComponent<AudioSource>();
        if (audioSource != null) {
            audioSource.Play();
        }
    }
}


    public void AbrirPortas() 
    {
        bool abriuPortas = false;
        GameObject[] portas = GameObject.FindGameObjectsWithTag("Porta");
        foreach (GameObject porta in portas)
        {
            if (porta != null) {
                abriuPortas = true;
                porta.GetComponent<SunTemple.Door>().Destrancar();
                if (porta.GetComponent<Outline>()) 
                {
                    porta.GetComponent<Outline>().OutlineWidth = 5f;
                }
            }
        }

        if (abriuPortas) 
        {
            mensagemTemporaria.MostrarMensagem("As portas da Catedral foram destrancadas!");
        }
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
