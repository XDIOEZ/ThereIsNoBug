using UnityEngine;

public class BellAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public canInteract canInteract;
    
    public void Update()
    {
        if(currentPosition.Instance.Y_currentindex == 3 && currentPosition.Instance.X_currentindex == 1)
        {
            // TODO 在播放的时候不覆盖
            // 修改为：只有在音频未播放时才开始播放
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }

        if(canInteract.enabled == false)
        {
            audioSource.Stop();
        }
    }
}