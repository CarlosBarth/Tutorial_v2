using UnityEngine;
using UnityEngine.UI;  // Importante para acessar componentes de UI como Text
using System.Collections;

public class MensagemTemporaria : MonoBehaviour
{
    public Text textoMsg;  // Atribua este campo no inspector com o seu objeto Text
    private float duracao = 5f;  // Duração que a mensagem ficará visível

    void Start()
    {
        // Inicia escondido
        textoMsg.gameObject.SetActive(false);
    }

    // Método para ativar a mensagem
    public void MostrarMensagem(string mensagem)
    {
        textoMsg.text = mensagem;  // Configura o texto
        textoMsg.gameObject.SetActive(true);  // Mostra o texto
        StartCoroutine(EsconderMensagem());
        print(mensagem);
    }

    // Corrotina para esconder a mensagem após um tempo
    IEnumerator EsconderMensagem()
    {
        yield return new WaitForSeconds(duracao);
        textoMsg.gameObject.SetActive(false);  // Esconde o texto após o tempo especificado
    }
}
