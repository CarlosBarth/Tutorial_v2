using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovimentarPersonagem : MonoBehaviour
{
    public CharacterController controle;
    public float velocidade = 6f;
    public float alturaPulo = 6f;
    public float gravidade = -20f;
    public Transform checaChao;
    public float raioEsfera = 0.4f;
    public LayerMask chaoMask;
    public bool estahNoChao;

    Vector3 velocidadeCai;
    private Transform cameraTransform;
    private bool estahAbaixado = false;
    private bool levantarBloqueado;
    public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado, velocidadeAbaixar;

    public AudioClip somPulo;
    public AudioClip somPassosGrass;
    public AudioSource audioSrc;
    
    private int vida = 100;
    public Slider sliderVida;
    public Text textoVida;
    public GameObject telaFimJogo;
    
    public Text textoPontuacaoFinal;

    
    // Start is called before the first frame update
    void Start()
    {
        controle = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vida <= 0) 
        {
            FimDeJogo();
            return;
        }

        Mover();
        Pular();
        Agachar();       
    }

    void Agachar() {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            AgacharLevantar();
        }
        ChecarBloqueioAbaixado();
    }

    void Mover() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 mover = transform.right * x + transform.forward * z;

        controle.Move(mover * velocidade * Time.deltaTime);
    
        SomPassos(x, z, estahNoChao);
    }

    private void SomPassos(float eixoX, float eixoY, bool estahNoChao) 
    {
//        print(controle.velocity.magnitude);
         // Verifica se o personagem está se movendo
        if (controle.velocity.magnitude > 0.1f) // Ajuste 0.1f conforme necessário
        {
            if (!audioSrc.isPlaying)
            {
                audioSrc.clip = somPassosGrass; //Passos Na grama
                audioSrc.Play();
            }
            
            // Ajusta o pitch baseado na velocidade do personagem
            // Aqui você pode ajustar os valores multiplicadores para obter o resultado desejado
            audioSrc.pitch = 1.0f + controle.velocity.magnitude / 10.0f;
        }
        else
        {
            // Para o som se o personagem não está se movendo
            if (audioSrc.isPlaying)
            {
                audioSrc.Stop();
            }
        }
    }

    void Pular() 
    {
        estahNoChao = Physics.CheckSphere(checaChao.position, raioEsfera,chaoMask);
        
        if (estahNoChao && Input.GetButtonDown("Jump")) 
        {
            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            audioSrc.clip = somPulo;
            audioSrc.Play();
        }

        if (!estahNoChao)
        {
            velocidadeCai.y += gravidade * Time.deltaTime;
        }

        controle.Move(velocidadeCai * Time.deltaTime);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(checaChao.position, raioEsfera);
    }

    private void AgacharLevantar()
    {
        if (levantarBloqueado || !estahNoChao)
        {
            return;
        }

        estahAbaixado = !estahAbaixado;
        if (estahAbaixado)
        {
            controle.height = alturaAbaixado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraAbaixado, 0);
        }
        else
        {
            controle.height = alturaLevantado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraEmPe, 0);
        }

    }

    private void ChecarBloqueioAbaixado()
    {
        //Debug.DrawRay(cameraTransform.position, Vector3.up * 1.1f, Color.red);
        RaycastHit hit;
        levantarBloqueado = Physics.Raycast(cameraTransform.position, Vector3.up, out hit, 1.1f);
    }
   
   public void AtualizarVida(int valor)
   {
        // print("vida - " + vida);
        // print("valor - " + valor);
        vida = Mathf.CeilToInt(Mathf.Clamp(vida + valor, 0, 100));
        sliderVida.value = vida;
        textoVida.text = $"{vida}";
   }

   private void FimDeJogo ()
   {
        // Vai de 0 a 1
        // 1 velocidade normal
        // entre 0 e 1 possivel configurar camera lenta
        // Time.timeScale = 0;
        // Camera.main.GetComponent<AudioListener>().enabled = false;
        // GetComponentInChildren<Glock>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(2);
    }

}
