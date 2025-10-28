using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class CG1 : MonoBehaviour
{
    
    public VideoPlayer videoPlayer;
    public VideoClip[] videoClip;
    public GameObject videoCanvas;
    public Image fadeImage;
    public GameSceneSO nextScene;
    
    public VoidEventSO loadMainSeneEvent;
    
    
    public void PlayVideo(int index)
    {
        videoPlayer.clip = videoClip[index];
        videoPlayer.Play();
    }

    private void Start()
    {
        PlayVideo(0);
    }


    private void Update()
    {
        videoPlayer.loopPointReached += EndReached;
    }
    
    
    
    void EndReached(VideoPlayer vp)
    {
        //videoCanvas.SetActive(false);
        fadeImage.DOBlendableColor(new Color(0, 0, 0, 1), 2f);
        SceneLoadManager.GetInstance().LoadScene(nextScene,new Vector3(0,0,0),true);
        //loadMainSeneEvent.RaiseEvent();
    }
    
}
