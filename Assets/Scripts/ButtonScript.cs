using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    private AudioManager audioManager;

    void Awake(){
        audioManager = FindObjectOfType<AudioManager>();
        
        if (audioManager != null)
        {
            audioManager.playMusic();
        }
    }
    public void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void loadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void loadSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void loadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void exitGame() 
    {
        Application.Quit();
    }
}
