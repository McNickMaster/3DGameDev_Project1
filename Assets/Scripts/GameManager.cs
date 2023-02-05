using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameOverUI, pauseUI;

    public bool paused = false;
    private bool gameOver = false;

    void Awake()
    {
        instance = this;
        
        Time.timeScale = 1;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            if(paused)
            {
                Unpause();
            } else 
            {
                Pause();
            }
        }


        
    }

    public void Unpause()
    {
        paused = false;
        pauseUI.SetActive(false);
        ApplyGameMouse();
        Time.timeScale = paused ? 0 : 1;
    }

    void Pause()
    {
        paused = true;
        pauseUI.SetActive(true);
        
        ApplyUIMouse();
        Time.timeScale = paused ? 0 : 1;
    }

    void EndGame()
    {
        gameOverUI.SetActive(true);
        gameOver = true;
        ApplyUIMouse();
    }

    private void ApplyUIMouse()
    {
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void ApplyGameMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PlayerCaught()
    {
        Invoke("EndGame", 0.075f);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
