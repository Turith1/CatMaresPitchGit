using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour

{
    
    
    public string gameSceneName = "GameScene";

    private void Awake()
    {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }


    // Função para o botão "Play".
    public void IniciaGame()
    {
        // Usa o SceneManager para carregar a cena do jogo.
        DOTween.KillAll();
        SceneManager.LoadScene("SceneMainGame");
    }

    public void FazerTutorial()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("Tutorial");
    }

    public void MainMenu()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("Menu");
    }
   
    // Função para o botão "Sair".
    public void SairGame()
    {
        // Encerra a aplicação.

        DOTween.KillAll();
        Application.Quit();

        
        Debug.Log("Saindo do jogo...");
    }
}