using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMission : MonoBehaviour, IInputReceiver, IInspectorSetter
{
    public static EditMission Instance = null;

    [SerializeField] private Canvas canvas;
    [SerializeField] private ConfigFieldEditor[] configFieldEditors;

    private bool editing;

    private void Awake()
    {
        Instance = this;
        editing = false;
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref canvas, "Canvas");
        InspectorSetter.SetInspectorObjectsInChildByType(canvas.transform, ref configFieldEditors);
    }

    public void OnInputDown()
    {

    }

    public void OnInputSecondaryDown()
    {

    }

    public void OnInputUp()
    {

    }

    public void OnInputUpInside()
    {
        editing = !editing;

        Inventory.Instance.gameObject.SetActive(!editing);
        EventManager.Instance.SetSlotActive(!editing);
        BoardManager.Instance.SetBoardActive(!editing);
        UIManager.Instance.SetUIMeshesActive(!editing);

        canvas.gameObject.SetActive(editing);

        if(editing)
        {
            ConfigMessage.Instance.ClearMessageText();
            foreach (ConfigFieldEditor configFieldEditor in configFieldEditors)
            {
                configFieldEditor.GetJSON();
                configFieldEditor.ApplyJSON();
            }
        }
        else MissionManager.Instance.StartCurrentMission();
    }

    public void GetJSONOnAllConfigFieldEditors()
    {
        foreach (ConfigFieldEditor configFieldEditor in configFieldEditors)
        {
            configFieldEditor.GetJSON();
        }
    }
}
