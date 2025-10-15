using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable 
{
    
    DataDefination GetDataID();
    
    void RegisterSaveData() => DataManager.instance.RegisterSaveable(this);
    void UnregisterSaveData() => DataManager.instance.UnregisterSaveable(this);

    void GetSaveData(Data data);
    void LoadData(Data data);
    
    
}
