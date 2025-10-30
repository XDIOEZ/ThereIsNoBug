using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel:BasePanel
{
    public Slider MusicSlider;
    public Slider SoundSlider;
    private bool isActive = false;
    public bool IsActive
    {
        get { return isActive; }
    }

    protected override void Awake()
    {
        base.Awake();
        MusicSlider = GetControl<Slider>("MusicSlider");
        SoundSlider = GetControl<Slider>("SoundSlider");
        //TODO:设置界面打开
        isActive = true;
    }

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName)
        {
            case "ReloadBtn":
                UIMgr.Instance().HidePanel("SettingPanel");
                break;
            case"ExitBtn":
                Application.Quit();
                break;
        }
    }

    protected override void OnValueChanged(string toggleName, bool value)
    {
        base.OnValueChanged(toggleName, value);
        switch (toggleName)
        {
            case "MusicToggle":
                break;
            case "SoundToggle":
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

    public override void HideMe()
    {
        base.HideMe();
        //TODO:设置界面关闭
        isActive = false;
    }
}
