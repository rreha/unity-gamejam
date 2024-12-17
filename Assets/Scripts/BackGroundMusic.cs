using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    // Start is called before the first frame update
    private static BackGroundMusic instance;

    void Awake()
    {
        // Eğer başka bir BackgroundMusic nesnesi varsa, bu nesneyi yok et
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Bu nesneyi sahne değişiminde yok etme
        }
    }
}
