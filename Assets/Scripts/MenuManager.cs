using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public bool _isPaused;
    [SerializeField]
    private GameObject _pauseCanvas;

    public void Play()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _isPaused = false;
        _pauseCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
