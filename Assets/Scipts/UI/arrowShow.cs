using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowShow : MonoBehaviour
{
    private int[,] left;
    private int[,] right;
    private int[,] up;
    private int[,] down;

    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject upArrow;
    public GameObject downArrow;
    
    public AreaChange areaChange;
    public AreaTransition areaTransition;
    
    public int currentXindex;
    public int currentYindex;
    
    
    private void Start()
    {
        

        currentXindex = areaTransition.currentXAreaIndex;
        currentYindex = areaChange.currentYAreaIndex;
        
        left = new int[5, 4];
        right = new int[5,4];
        up = new int[5,4];
        down = new int[5,4];

        var Left = new (int row, int col)[]
        {
            (1,0),(2,0),(1,1),(2,1),(3,1),(4,1),(1,2),(2,2),(3,2),(4,2),(1,3),(2,3),(4,3)
        };
        foreach (var (row,col) in Left)
        {
            left[row, col] = 1;
        }

        
        
        var Right = new (int row, int col)[]
        {
            (0,0),(0,1),(0,2),(0,3),
            (1,0),(1,1),(1,2),(1,3),
            (2,1),(2,2),
            (3,1),(3,2),(3,3)
        };
        foreach (var (row,col) in Right)
        {
            right[row, col] = 1;
        }
        
        var Up = new (int row, int col)[]
        {
            (0,0), (0,1), (0,2), // 第一行
            (1,0), (1,1), // 第二行
            (2,2), (2,0), // 第三行
            (3,0), (3,1), // 第四行
            (4,1), (4,2) // 第五行
        };
        foreach (var (row, col) in Up)
        {
            up[row, col] = 1;
        }
        
        var Down = new (int row, int col)[]
        {
            (0,1),(0,2), (0,3), // 第一行
            (1,2), (1,1), // 第二行
            (2,1), (2,3), // 第三行
            (3,2), (3,1), // 第四行
            (4,2), (4,3)  // 第五行
        };
        foreach (var (row, col) in Down)
        {
            down[row, col] = 1;
        }
    }

    public void ShowArrow(int x, int y)
    {
        if(left[x,y]==1)
            leftArrow.SetActive(true);
        else
            leftArrow.SetActive(false);
        if(right[x,y]==1)
            rightArrow.SetActive(true);
        else
            rightArrow.SetActive(false);
        if(up[x,y]==1)
            upArrow.SetActive(true);
        else
            upArrow.SetActive(false);
        if(down[x,y]==1)
            downArrow.SetActive(true);
        else
            downArrow.SetActive(false);
        
    }
    
    private void Update()
    {
        currentXindex = areaTransition.currentXAreaIndex;
        currentYindex = areaChange.currentYAreaIndex;
        ShowArrow(currentXindex,currentYindex);
    }
}
