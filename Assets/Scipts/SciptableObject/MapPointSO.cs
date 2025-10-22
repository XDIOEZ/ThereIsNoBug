using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CollectionSO/MapPointsSO")]
public class MapPointsSO : ScriptableObject
{
    [Header("0左,1上,2右,3下")]
    public List<MapPoint> MapPoints = new List<MapPoint>();

}
