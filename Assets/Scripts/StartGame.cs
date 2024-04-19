using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public Text campoDePontuacao;

    public void ReiniciarJogo() 
   {
          SceneManager.LoadScene(1);
   }

   public void SairJogo() 
   {
        Application.Quit();
   }
}
