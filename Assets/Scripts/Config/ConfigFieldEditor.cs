using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfigFieldEditor : MonoBehaviour, IInspectorSetter
{
    [Header("Parameters")]
    [SerializeField] private ConfigManager.ConfigType configType = ConfigManager.ConfigType.Buildings;

    [Header("References")]
    [SerializeField] private TextMeshPro titleMesh;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button getJSONButton;
    [SerializeField] private Button applyJSONButton;
    [SerializeField] private Button addElementButton;

    private void Awake()
    {
        titleMesh.text = configType.ToString();

        getJSONButton.OnButtonInputUpInside += GetJSONFromButton;
        applyJSONButton.OnButtonInputUpInside += ApplyJSONFromButton;
        addElementButton.OnButtonInputUpInside += AddElement;
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectInChildByType(transform, ref titleMesh);
        InspectorSetter.SetInspectorObjectInChildByType(transform, ref inputField);

        InspectorSetter.SetInspectorObjectInChildByName(transform, ref getJSONButton, "GetJSONButton");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref applyJSONButton, "ApplyJSONButton");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref addElementButton, "AddElementButton");
    }

    public void GetJSON()
    {
        inputField.text = ConfigManager.Instance.GetJSON(configType);
    }

    public bool ApplyJSON()
    {
        bool excepton = false;
        try
        {
            ConfigManager.Instance.ApplyJSON(configType, inputField.text);
            GetJSON();
        }
        catch (Exception)
        {
            excepton = true;
            ConfigMessage.Instance.TrySetMessageText("JSON Syntax Error", false);
        }
        return !excepton;
    }

    private void GetJSONFromButton()
    {
        ConfigMessage.Instance.ClearMessageText();

        GetJSON();
    }

    private void ApplyJSONFromButton()
    {
        ConfigMessage.Instance.ClearMessageText();

        if (ApplyJSON()) ConfigMessage.Instance.TrySetMessageText("JSON Applied Successfully", true);
    }

    private void AddElement()
    {
        ConfigMessage.Instance.ClearMessageText();

        ConfigManager.Instance.AddElement(configType);
        GetJSON();
    }
}
