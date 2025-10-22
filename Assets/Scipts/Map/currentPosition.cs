using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currentPosition
{

    public int X_currentindex;
    public int Y_currentindex;

    private static currentPosition instance;

    public static currentPosition Instance
    {
        get
        {
            if(instance == null)
                instance = new currentPosition();
            return instance;
        }
        
        
    }
    

}
