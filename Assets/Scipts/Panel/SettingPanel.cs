using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel:BasePanel
{
    public Slider MusicSlider;

    protected override void Awake()
    {
        base.Awake();
        MusicSlider = GetControl<Slider>("MusicSlider");
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
                print("音量大小为"+value);
                break;
        }
    }
}
