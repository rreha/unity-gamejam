using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
     public GameObject pauseMenuUI; 
    private bool isPaused = false;

    void Update()
    {
        // ESC tuşuna basıldığında pause işlemini kontrol et
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Pause menüsünü gizle
        Time.timeScale = 1f; // Oyunun hızını normale döndür
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Pause menüsünü göster
        Time.timeScale = 0f; // Oyunu durdur
        isPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Oyun kapanır (Editörde çalışmaz)
    }
}
