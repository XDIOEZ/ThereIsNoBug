using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayClick()
    {   
        audioSource.Play();
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayClick();
        }
    }
}
