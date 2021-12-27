using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfigMessage : MonoBehaviour, IInspectorSetter
{
    public static ConfigMessage Instance = null;

    [SerializeField] private TextMeshPro messageMesh = null;
    [SerializeField] private Color failColor = Color.white;
    [SerializeField] private Color successColor = Color.white;

    private void Awake()
    {
        Instance = this;
        ClearMessageText();
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectInGameObjectByType(transform, ref messageMesh);
    }

    public void ClearMessageText()
    {
        messageMesh.text = null;
    }

    public void TrySetMessageText(string messageText, bool isSuccessful)
    {
        if (messageMesh.text == null)
        {
            messageMesh.text = messageText;
            messageMesh.color = isSuccessful ? successColor : failColor;
        }
    }
}
