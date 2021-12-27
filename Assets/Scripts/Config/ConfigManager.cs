using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ConfigManager : MonoBehaviour
{
    public enum ConfigType
    {
        Buildings,
        Events,
        Items,
        Missions,
        Tiles,
        CurrentMission,
    }

    public static ConfigManager Instance = null;

    [SerializeField] private ConfigSO currentConfig = null;
    [SerializeField] private ConfigSO backupConfig = null;

    private void Awake()
    {
        Instance = this;
    }

    public void Construct()
    {
        currentConfig.OnValidate();
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Custom Tools/Load Config From ScriptableObjects")]
    private static void LoadFromScriptableObjectsStatic()
    {
        ConfigManager configManager = FindObjectOfType<ConfigManager>();
        configManager.currentConfig.LoadFromScriptableObjects();
        configManager.backupConfig.LoadConfigContentsFrom(configManager.currentConfig);
    }
#endif

    public string GetJSON(ConfigType configType)
    {
        string json = null;
        switch (configType)
        {
            case ConfigType.Buildings:      json = GetJSONFrom(currentConfig.GetDatas<BuildingData>()); break;
            case ConfigType.Events:         json = GetJSONFrom(currentConfig.GetDatas<EventData>());    break;
            case ConfigType.Items:          json = GetJSONFrom(currentConfig.GetDatas<ItemData>());     break;
            case ConfigType.Missions:       json = GetJSONFrom(currentConfig.GetDatas<MissionData>());  break;
            case ConfigType.Tiles:          json = GetJSONFrom(currentConfig.GetDatas<TileData>());     break;
            case ConfigType.CurrentMission: json = currentConfig.GetCurrentMission()?.GetName();        break;
            default: break;
        }
        return json;
    }

    public void ApplyJSON(ConfigType configType, string json)
    {
        switch (configType)
        {
            case ConfigType.Buildings:      currentConfig.SetDatas(ApplyJSONTo<BuildingData>(json));    break;
            case ConfigType.Events:         currentConfig.SetDatas(ApplyJSONTo<EventData>(json));       break;
            case ConfigType.Items:          currentConfig.SetDatas(ApplyJSONTo<ItemData>(json));        break;
            case ConfigType.Missions:       currentConfig.SetDatas(ApplyJSONTo<MissionData>(json));     break;
            case ConfigType.Tiles:          currentConfig.SetDatas(ApplyJSONTo<TileData>(json));        break;
            case ConfigType.CurrentMission: currentConfig.SetCurrentMission(json);                      break;
            default: break;
        }
    }

    public void AddElement(ConfigType configType)
    {
        switch (configType)
        {
            case ConfigType.Buildings: currentConfig.AddElementToData<BuildingData>();  break;
            case ConfigType.Events:    currentConfig.AddElementToData<EventData>();     break;
            case ConfigType.Items:     currentConfig.AddElementToData<ItemData>();      break;
            case ConfigType.Missions:  currentConfig.AddElementToData<MissionData>();   break;
            case ConfigType.Tiles:     currentConfig.AddElementToData<TileData>();      break;
            default: break;
        }
    }

    public void ResetToDefault()
    {
        ConfigMessage.Instance.ClearMessageText();

        currentConfig.LoadConfigContentsFrom(backupConfig);
        EditMission.Instance.GetJSONOnAllConfigFieldEditors();

        ConfigMessage.Instance.TrySetMessageText("Resetted To Default", true);
    }

    private string GetJSONFrom<T>(T[] elements)
    {
        StringBuilder builder = new StringBuilder("[");
        for (int i = 0; i < elements.Length; i++)
        {
            if (i != 0) builder.Append(",");
            builder.Append(JsonUtility.ToJson(elements[i]));
        }
        return builder.Append("]").ToString();
    }

    private T[] ApplyJSONTo<T>(string json) where T : IData
    {
        string[] splitted = SplitJSONArray(json);
        T[] datas = new T[splitted.Length];
        bool[] toDelete = new bool[splitted.Length];
        for (int i = 0; i < splitted.Length; i++)
        {
            datas[i] = JsonUtility.FromJson<T>(splitted[i]);
            toDelete[i] = datas[i].GetName().ToLower() == "delete";
        }
        datas = DeleteAtIndexes(datas, toDelete);
        return datas;
    }

    private string[] SplitJSONArray(string json)
    {
        string substr = json.Substring(1, json.Length - 2);
        int openedSymbols = 0;
        StringBuilder builder = new StringBuilder();
        List<string> splitted = new List<string>();
        for (int i = 0; i < substr.Length; i++)
        {
            if (openedSymbols == 0 && substr[i] == ',')
            {
                splitted.Add(builder.ToString());
                builder.Clear();
            }
            else
            {
                if (substr[i] == '{' || substr[i] == '[') openedSymbols++;
                else if (substr[i] == '}' || substr[i] == ']') openedSymbols--;

                builder.Append(substr[i]);
            }
        }
        if (builder.Length != 0) splitted.Add(builder.ToString());
        return splitted.ToArray();
    }

    private T[] DeleteAtIndexes<T>(T[] array, bool[] toDelete)
    {
        List<T> toKeep = new List<T>();
        for (int i = 0; i < toDelete.Length; i++)
        {
            if (!toDelete[i]) toKeep.Add(array[i]);
        }
        return toKeep.ToArray();
    }
}
