using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    public VoidEventSO moveUpEvent;
    public VoidEventSO moveDownEvent;
    public VoidEventSO moveLeftEvent;
    public VoidEventSO moveRightEvent;
    public VoidEventSO resetEvent;
    public MapPointsSO arrived;
    public float lineSize;
    public float size;
    public GameObject nowPoint;
    public GameObject point;
    private LineRenderer lineRenderer;
    
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        arrived.MapPoints.Clear();
        moveUpEvent.OnEventRaised += GoUp;
        moveDownEvent.OnEventRaised += GoDown;
        moveLeftEvent.OnEventRaised += GoLeft;
        moveRightEvent.OnEventRaised += GoRight;
        resetEvent.OnEventRaised += ReverseMap;
    }

    public void OpenMap()
    {
        gameObject.SetActive(true);
        DrawMap();
        gameObject.transform.position = Camera.main.ViewportToWorldPoint(
            new Vector3(0.75f, 0.25f, Camera.main.nearClipPlane + 1f));
    }

    public void CloseMap()
    {
        var children = transform.Cast<Transform>().ToList();
        
        foreach (var child in children)
        {
            Destroy(child.gameObject);
        }
        gameObject.SetActive(false);
    }
    
    public void ReverseMap()
    {
        lineRenderer.positionCount = 0;
        arrived.MapPoints.Clear();
        arrived.MapPoints.Add(new MapPoint(new Vector2(3,0), 1));
    }
    
    public void GoUp()
    {
        MapPoint mapPoint = new MapPoint(SceneLoadManager.GetInstance().currentPosition,1);
        MapPoint _mapPoint = arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == SceneLoadManager.GetInstance().currentPosition);
        if (arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == new Vector2(SceneLoadManager.GetInstance().currentPosition.x , SceneLoadManager.GetInstance().currentPosition.y + 1 )) != null)
        {
            return;
        }
        if (_mapPoint != null)
        {
            if (!_mapPoint.dirs.Contains(1))
            {
                _mapPoint.dirs.Add(1);
            }
        }
        else
        {
            arrived.MapPoints.Add(mapPoint);
        }
    }
    public void GoDown()
    {
        MapPoint mapPoint = new MapPoint(SceneLoadManager.GetInstance().currentPosition,3);
        MapPoint _mapPoint = arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == SceneLoadManager.GetInstance().currentPosition);
        if (arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == new Vector2(SceneLoadManager.GetInstance().currentPosition.x , SceneLoadManager.GetInstance().currentPosition.y - 1 )) != null)
        {
            return;
        }
        if (_mapPoint != null)
        {
            if (!_mapPoint.dirs.Contains(3))
            {
                _mapPoint.dirs.Add(3);
            }
        }
        else
        {
            arrived.MapPoints.Add(mapPoint);
        }
    }
    public void GoLeft()
    {
        MapPoint mapPoint = new MapPoint(SceneLoadManager.GetInstance().currentPosition,0);
        MapPoint _mapPoint = arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == SceneLoadManager.GetInstance().currentPosition);
        if (arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == new Vector2(SceneLoadManager.GetInstance().currentPosition.x - 1 , SceneLoadManager.GetInstance().currentPosition.y)) != null)
        {
            return;
        }
        if (_mapPoint != null)
        {
            if (!_mapPoint.dirs.Contains(0))
            {
                _mapPoint.dirs.Add(0);
            }
        }
        else
        {
            arrived.MapPoints.Add(mapPoint);
        }
    }
    public void GoRight()
    {
        MapPoint mapPoint = new MapPoint(SceneLoadManager.GetInstance().currentPosition,2);
        MapPoint _mapPoint = arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == SceneLoadManager.GetInstance().currentPosition);
        if (arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == new Vector2(SceneLoadManager.GetInstance().currentPosition.x + 1, SceneLoadManager.GetInstance().currentPosition.y )) != null)
        {
            return;
        }
        if (_mapPoint != null)
        {
            if (!_mapPoint.dirs.Contains(2))
            {
                _mapPoint.dirs.Add(2);
            }
        }
        else
        {
            arrived.MapPoints.Add(mapPoint);
        }
    }
    
    private void DrawMap()
    {
        if(lineRenderer == null)
            
            lineRenderer = GetComponent<LineRenderer>();
        if (arrived.MapPoints.Count != 0)
        {
            arrived.MapPoints = arrived.MapPoints
                .OrderBy(v => v.pos.x)
                .ThenBy(v => v.pos.y)
                .ToList();
            List<Vector2> pointsOnWay = new List<Vector2>();
            pointsOnWay.AddRange(DrawWays(new MapPoint(new Vector2(3,0), 1)));
            lineRenderer.startWidth = lineSize;
            lineRenderer.endWidth = lineSize;
            lineRenderer.positionCount = pointsOnWay.Count;
            for (int i = 0; i < pointsOnWay.Count; i++)
            {
                lineRenderer.SetPosition(i, pointsOnWay[i] * size);
                if (pointsOnWay[i] == SceneLoadManager.GetInstance().currentPosition)
                {
                    GameObject nowPointGbj = Instantiate(nowPoint,this.transform);
                    nowPointGbj.transform.localPosition = pointsOnWay[i] * size;
                }
                else
                {
                    GameObject pointGbj = Instantiate(point,this.transform);
                    pointGbj.transform.localPosition = pointsOnWay[i] * size;
                }
            }
        }
        else
        {
            GameObject nowPointGbj = Instantiate(nowPoint,this.transform);
            nowPointGbj.transform.localPosition = new Vector2(3 , 0) * size;
            Debug.Log("no point");
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
        _point.dirs[0] = -1;
        pointsOnWay = DrawWays(_point);
        return pointsOnWay;
    }
    
    private List<Vector2> DrawRightWays(MapPoint point)
    {
        List<Vector2> pointsOnWay = new List<Vector2>();
        MapPoint _point = new MapPoint(point);
        _point.pos.x = point.pos.x+1;
        _point.dirs[0] = -1;
        pointsOnWay = DrawWays(_point);
        return pointsOnWay;
    }
    
    private List<Vector2> DrawUpWays(MapPoint point)
    {
        List<Vector2> pointsOnWay = new List<Vector2>();
        MapPoint _point = new MapPoint(point);
        _point.pos.y = point.pos.y+1;
        _point.dirs[0] = -1;
        pointsOnWay = DrawWays(_point);
        return pointsOnWay;
    }
    
    private List<Vector2> DrawDownWays(MapPoint point)
    {
        List<Vector2> pointsOnWay = new List<Vector2>();
        MapPoint _point = new MapPoint(point);
        _point.pos.y = point.pos.y-1;
        _point.dirs[0] = -1;
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
        foreach (var VARIABLE in _mapPoint.dirs)
        {
            dirs.Add(VARIABLE);
        }
    }
    
    public Vector2 pos;
    public List<int> dirs;
}