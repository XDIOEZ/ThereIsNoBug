using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGamePanel : BasePanel
{
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName)
        {
            case"CloseBtn":
                UIMgr.Instance().HidePanel("LittleGamePanel");
                break;
            
        }
    }
}
