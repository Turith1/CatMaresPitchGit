using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour

{
    
    
    public string gameSceneName = "GameScene";

    private void OnEnable()
    {
        DOTween.KillAll();
    }

    private void Awake()
    {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }


    // Função para o botão "Play".
    public void IniciaGame()
    {
        // Usa o SceneManager para carregar a cena do jogo.
        SceneManager.LoadScene("SceneMainGame");
    }

    public void FazerTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
   
    // Função para o botão "Sair".
    public void SairGame()
    {
        // Encerra a aplicação.
        
       
        Application.Quit();

        
        Debug.Log("Saindo do jogo...");
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}