using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataManager : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventSO SaveEvent;
    
    public static DataManager instance;
    
    public List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
        saveData = new Data();
        
    }
    
    
    private void OnEnable()
    {
        SaveEvent.OnEventRaised += Save;
    }

    private void OnDisable()
    {
        SaveEvent.OnEventRaised -= Save;
    }
    
    
    private void Update()
    {
        if(Keyboard.current.rKey.wasPressedThisFrame)
        {
            Load();
        }
    }
    
    public void RegisterSaveable(ISaveable saveable)
    {
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }


    public void UnregisterSaveable(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }
    
    
    public void Save()
    {
        foreach (var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }

        foreach (var saveable in saveData.characterPositionDict)
        {
            Debug.Log(saveable.Key + " " + saveable.Value);
        }
    }

    public void Load()
    {
        foreach (var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }
    
    
    
    
    
    
}
