using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventSlot : MonoBehaviour, IInspectorSetter
{
    [Header("Parameters")]
    [SerializeField] private Color failColor;

    [SerializeField] private TextMeshPro nameMesh = null;
    [SerializeField] private TextMeshPro daysLeftMesh = null;
    [SerializeField] private TextMeshPro costMesh = null;
    [SerializeField] private TextMeshPro rewardMesh = null;
    [SerializeField] private SpriteRenderer sprite = null;

    private Color baseSpriteColor = Color.white;

    private void Awake()
    {
        baseSpriteColor = sprite.color;
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref nameMesh, "Name");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref daysLeftMesh, "DaysLeft");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref costMesh, "Cost");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref rewardMesh, "Reward");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref sprite, "Sprite");
    }

    public void Construct(EventData data)
    {
        sprite.color = baseSpriteColor;

        nameMesh.text = data.GetName();
        UpdateDaysLeftMesh(data.GetDaysToPrepare());
        costMesh.text = data.GetCost().ToString(ItemStack.Sign.Negative);
        rewardMesh.text = data.GetReward().ToString(ItemStack.Sign.Positive);
    }

    public void UpdateDaysLeftMesh(int days)
    {
        if (days == 0) daysLeftMesh.text = null; 
        else daysLeftMesh.text = $"{ days } Days left";
    }

    public void SetSpriteColorToFail()
    {
        sprite.color = failColor;
    }
}
