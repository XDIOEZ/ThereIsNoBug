using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAudio : MonoBehaviour
{
    public void PlayBGM(string name)
    {
        AudioManager.Instance.PlayBGM(name);
    }
    public void PlaySFX(string name)
    {
        AudioManager.Instance.PlaySFX(name);
    }
    public void SwitchBGM(string name)
    {
        AudioManager.Instance.SwitchBGM(name);
    }
    public void StopBGM(string name)
    {
        AudioManager.Instance.StopBGM(name);
    }
}
