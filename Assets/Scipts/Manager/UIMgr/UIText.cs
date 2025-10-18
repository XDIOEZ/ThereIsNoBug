using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
            UIMgr.Instance().ShowPanel<BasePanel>("StartPanel", E_UI_Layer.Mid);
    }

}
