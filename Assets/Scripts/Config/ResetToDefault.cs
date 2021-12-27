using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetToDefault : MonoBehaviour, IInspectorSetter
{
    [SerializeField] private Button button;

    private void Awake()
    {
        button.OnButtonInputUpInside += ConfigManager.Instance.ResetToDefault;
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectInGameObjectByType(transform, ref button);
    }
}
