using System.Collections;
using UnityEngine;

public class Egg : MonoBehaviour, ILevarDano
{
    public GameObject chickPrefab; // Referência ao prefab do filhote
    private Coroutine hatchCoroutine; // Guarda a referência da coroutine
    public AudioSource audioSrc;
    public AudioClip somEclodir;
    public int vida = 25;

    void Start()
    {
        
        audioSrc = GetComponent<AudioSource>();
        ResetHatchTimer(); // Inicializa o timer
    }

    void Update ()
    {
        VerificaVida(); 
    }

    private IEnumerator HatchEgg()
    {
        yield return new WaitForSeconds(10); // Espera 10 segundos
        Hatch();
    }

    private void Hatch()
    {
        print(chickPrefab);
        if (chickPrefab != null)
        {
            audioSrc.clip = somEclodir;
            audioSrc.Play();
            StartCoroutine(WaitForSound());
            // Instancia o filhote na posição do ovo
            Instantiate(chickPrefab, transform.position, Quaternion.identity);
        }

        // Destrói o ovo após a eclosão
        Destroy(gameObject);
    }

    public void ResetHatchTimer()
    {
        if (hatchCoroutine != null)
        {
            print("reset");
            StopCoroutine(hatchCoroutine); // Para a coroutine atual se estiver rodando
        }
        hatchCoroutine = StartCoroutine(HatchEgg()); // Inicia uma nova coroutine
    }

    void OnCollisionEnter(Collision collision)
    {
        ResetHatchTimer(); // Reinicia o timer
    }

    public void LevarDano(int dano) 
    {
        vida-= dano;
    }

    protected void VerificaVida() 
    {
        if (vida <= 0) 
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator WaitForSound()
    {
        yield return new WaitForSeconds(5);
    }
}
