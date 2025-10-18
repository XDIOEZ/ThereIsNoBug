using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIDrag : MonoBehaviour
{
    private bool isDrag = false;
    private Vector2 startPos;
    void Update()
    {
        if (isDrag)
        {
            this.transform.position = Input.mousePosition;
        }
    }

    public void StartDrag()
    {
        isDrag = true;
        startPos = this.transform.position;
    }

    public void ExitDrag()
    {
        isDrag = false;
        this.transform.position = startPos;
    }
}



