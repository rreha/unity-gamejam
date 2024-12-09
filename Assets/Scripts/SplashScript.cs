using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScript : MonoBehaviour
{
    float wait = 3f;
    private void Start()
    {
        StartCoroutine(Splash());
    }

    private IEnumerator Splash()
    {
        yield return new WaitForSeconds(wait);
        SceneManager.LoadScene("MainMenu");
    }
}
