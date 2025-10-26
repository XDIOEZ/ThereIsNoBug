using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel:BasePanel
{
    public Slider MusicSlider;
    public Slider SoundSlider;

    protected override void Awake()
    {
        base.Awake();
        MusicSlider = GetControl<Slider>("MusicSlider");
        SoundSlider = GetControl<Slider>("SoundSlider");
    }

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName)
        {
            case "CloseBtn":
                UIMgr.Instance().HidePanel("SettingPanel");
                break;
            case "ReloadBtn":
                break;
            case"ExitBtn":
                break;
        }
    }

    protected override void SliderOnValueChanged(string sliderName, float value)
    {
        base.SliderOnValueChanged(sliderName, value);
        switch (sliderName)
        {
            case "MusicSlider":
                AudioManager.Instance.SetBGMVolume(value);
                print("音量大小为"+value);
                break;
            case "SoundSlider":
                print("音效大小为"+value);
                break;
        }
    }
}
