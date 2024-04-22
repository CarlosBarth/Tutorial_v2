using UnityEngine;
using UnityEngine.UI;  // Importante para acessar componentes de UI como Text
using System.Collections;

public class MensagemTemporaria : MonoBehaviour
{
    public Text textoMsg;  // Atribua este campo no inspector com o seu objeto Text

    void Start()
    {
        // Inicia escondido
        textoMsg.gameObject.SetActive(false);
    }

    // Método para ativar a mensagem
    public void MostrarMensagem(string mensagem, int duracao = 5)
    {
        textoMsg.text = mensagem;  // Configura o texto
        textoMsg.gameObject.SetActive(true);  // Mostra o texto
        StartCoroutine(EsconderMensagem(duracao));
    }

    // Corrotina para esconder a mensagem após um tempo
    IEnumerator EsconderMensagem(float duracao)
    {
        yield return new WaitForSeconds(duracao);
        textoMsg.gameObject.SetActive(false);  // Esconde o texto após o tempo especificado
    }
}
