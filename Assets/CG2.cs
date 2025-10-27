using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
public class CG2 : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip videoClip;
    public GameObject videoCanvas;

    private void Start()
    {
        videoPlayer.clip = videoClip;
        videoPlayer.Play();
    }


    private void Update()
    {
        videoPlayer.loopPointReached += EndReached;
    }

    private void EndReached(VideoPlayer vp)
    {
        videoCanvas.SetActive(false);
    }
    
    
    
}
