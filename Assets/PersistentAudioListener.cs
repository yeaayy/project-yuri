using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentAudioListener : MonoBehaviour
{
    private static PersistentAudioListener instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();
        foreach (AudioListener listener in audioListeners)
        {
            if (listener != this.GetComponent<AudioListener>())
            {
                listener.enabled = false;
            }
        }
    }
}