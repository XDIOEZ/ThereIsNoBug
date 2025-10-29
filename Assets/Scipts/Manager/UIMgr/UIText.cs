using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIMgr.Instance().ShowPanel<GamePanel>("GamePanel", E_UI_Layer.Mid, (thisPanel) =>
        {
            thisPanel.PropBoxIn();
            print("显示道具栏");
            thisPanel.ToggleSelectByUI(0);
        });
    }

}
