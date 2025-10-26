using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canInteract : MonoBehaviour
{
    public int whereCanInteract;

    private void Update()
    {
        if(currentPosition.Instance.Y_currentindex!= whereCanInteract)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
