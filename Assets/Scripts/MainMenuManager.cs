using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    [Header("Pixel Animations")]
    [SerializeField]
    private Animator _quitAnim;
    [SerializeField]
    private GameObject _tutorialAnim;
    [SerializeField]
    private GameObject _mainGameAnim;
    
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
        //SceneManager.LoadScene("SceneMainGame");
        StartCoroutine(SceneChangeDelay("SceneMainGame", 1.6f));
        if(_mainGameAnim != null)
            _mainGameAnim.SetActive(true);
    }

    public void FazerTutorial()
    {
        //SceneManager.LoadScene("Tutorial");
        StartCoroutine(SceneChangeDelay("Tutorial", 1.9f));
        if(_tutorialAnim != null)
            _tutorialAnim.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
   
    // Função para o botão "Sair".
    public void SairGame()
    {
        if (_quitAnim != null)
        {
            _quitAnim.SetTrigger("CastleTrigger");
            Invoke(nameof(QuitGame), 1.8f);
        }
        else
            Application.Quit();
    }

    private IEnumerator SceneChangeDelay(string SceneName, float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneName);
    }

    private void QuitGame()
    {
        Debug.Log("Quiting Game");
        Application.Quit();
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}