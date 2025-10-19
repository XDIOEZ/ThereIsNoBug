using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    public MapPointsSO points;
    public MapPointsSO arrived;
    public float size;
    public GameObject point;
    private Vector2 nowPos;
    private LineRenderer lineRenderer;
    
    public Vector2 NowPos
    {
        get { return nowPos; }
        set { nowPos = value; }
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        arrived.MapPoints = arrived.MapPoints
            .OrderBy(v => v.pos.x)     // 首先按 x 升序排序
            .ThenBy(v => v.pos.y)      // 然后按 y 升序排序
            .ToList();
        DrawMap();
    }

    private void DrawMap()
    {
        List<Vector2> pointsOnWay = new List<Vector2>();
        pointsOnWay.Add(Vector2.zero);
        pointsOnWay.AddRange(DrawWays(new MapPoint(Vector2.zero, 2)));
        lineRenderer.positionCount = pointsOnWay.Count;
        for (int i = 0; i < pointsOnWay.Count; i++)
        {
            lineRenderer.SetPosition(i, pointsOnWay[i] * size);
            GameObject pointGbj = Instantiate(point,this.transform);
            pointGbj.transform.localPosition = pointsOnWay[i] * size;
        }
    }

    private List<Vector2> DrawWays(MapPoint point)
    {
        List<Vector2> pointsOnWay = new List<Vector2>();
        MapPoint _point = new MapPoint(point);
        foreach (var VARIABLE in arrived.MapPoints)
        {
            if (VARIABLE.pos == point.pos)
            {
                _point = VARIABLE;
                break;
            }
        }
        pointsOnWay.Add(point.pos);
        if (_point.dirs.IndexOf(0) != -1)
        {
            List<Vector2> wayPoints = DrawLeftWays(point);
            pointsOnWay.AddRange(wayPoints);
            if (_point.dirs.Count > 1)
            {
                wayPoints.Reverse();
                pointsOnWay.AddRange(wayPoints);
                pointsOnWay.Add(point.pos);
            }
        }
        
        if (_point.dirs.IndexOf(1) != -1)
        {
            List<Vector2> wayPoints = DrawUpWays(point);
            pointsOnWay.AddRange(wayPoints);
            if (_point.dirs.Any(num => num > 1))
            {
                wayPoints.Reverse();
                pointsOnWay.AddRange(wayPoints);
                pointsOnWay.Add(point.pos);
            }
        }
        
        if (_point.dirs.IndexOf(2) != -1)
        {
            List<Vector2> wayPoints = DrawRightWays(point);
            pointsOnWay.AddRange(wayPoints);
            if (_point.dirs.Any(num => num > 2))
            {
                wayPoints.Reverse();
                pointsOnWay.AddRange(wayPoints);
                pointsOnWay.Add(point.pos);
            }
        }
        
        if (_point.dirs.IndexOf(3) != -1)
        {
            List<Vector2> wayPoints = DrawDownWays(point);
            pointsOnWay.AddRange(wayPoints);
        }
        return pointsOnWay;
    }
    
    private List<Vector2> DrawLeftWays(MapPoint point)
    {
        List<Vector2> pointsOnWay = new List<Vector2>();
        MapPoint _point = new MapPoint(point);
        _point.pos.x = point.pos.x-1;
        pointsOnWay = DrawWays(_point);
        return pointsOnWay;
    }
    
    private List<Vector2> DrawRightWays(MapPoint point)
    {
        List<Vector2> pointsOnWay = new List<Vector2>();
        MapPoint _point = new MapPoint(point);
        _point.pos.x = point.pos.x+1;
        pointsOnWay = DrawWays(_point);
        return pointsOnWay;
    }
    
    private List<Vector2> DrawUpWays(MapPoint point)
    {
        List<Vector2> pointsOnWay = new List<Vector2>();
        MapPoint _point = new MapPoint(point);
        _point.pos.y = point.pos.y+1;
        pointsOnWay = DrawWays(_point);
        return pointsOnWay;
    }
    
    private List<Vector2> DrawDownWays(MapPoint point)
    {
        List<Vector2> pointsOnWay = new List<Vector2>();
        MapPoint _point = new MapPoint(point);
        _point.pos.y = point.pos.y-1;
        pointsOnWay = DrawWays(_point);
        return pointsOnWay;
    }
    
    // private List<Vector2> DrawOneway(Vector2 first,int dir)
    // {
    //     List<Vector2> pointsOnWay =  new List<Vector2>();
    //     if (dir != 1)
    //     {
    //         Vector2 thenx = new Vector2(first.x - 1, first.y);
    //         int indexX = arrived.vecter2s.IndexOf(thenx);
    //         if (indexX != -1)
    //         {
    //             pointsOnWay.Add(arrived.vecter2s[indexX]);
    //             pointsOnWay.AddRange(DrawOneway(arrived.vecter2s[indexX],-1));
    //         }
    //
    //     }
    //
    //     if (dir != -1)
    //     {
    //         Vector2 thenz = new Vector2(first.x + 1, first.y);
    //         int indexZ = arrived.vecter2s.IndexOf(thenz);
    //         if (indexZ != -1) 
    //         { 
    //             pointsOnWay.Add(arrived.vecter2s[indexZ]); 
    //             pointsOnWay.AddRange(DrawOneway(arrived.vecter2s[indexZ],1));
    //         } 
    //
    //     }
    //
    //     Vector2 theny = new Vector2(first.x, first.y + 1);
    //     int indexY = arrived.vecter2s.IndexOf(theny);
    //     if (indexY != -1)
    //     {
    //         pointsOnWay.Add(arrived.vecter2s[indexY]);
    //         pointsOnWay.AddRange(DrawOneway(arrived.vecter2s[indexY],0));
    //     }
    //     return pointsOnWay;
    // }
}

[Serializable]
public class MapPoint
{
    public MapPoint(Vector2 _pos , int _dir)
    {
        pos = _pos;
        dirs = new List<int>();
        dirs.Add(_dir);
    }
    public MapPoint(MapPoint _mapPoint)
    {
        pos = _mapPoint.pos;
        dirs = new List<int>();
        foreach (var VARIABLE in dirs)
        {
            dirs.Add(VARIABLE);
        }
    }
    public Vector2 pos;
    public List<int> dirs;
}