using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BuildingBehaviour : MonoBehaviour, IInputReceiver, IInspectorSetter
{
    [Header("References")]
    [SerializeField] private TextMeshPro nameMesh = null;
    [SerializeField] private TextMeshPro costMesh = null;
    [SerializeField] private SpriteRenderer sprite = null;
    [SerializeField] private SpriteRenderer outline = null;

    [Header("Parameters")]
    [SerializeField] private Color spriteDisabledColor = Color.white;
    [SerializeField] private Color outlineDisabledColor = Color.white;
    [SerializeField] private Color outlineSelectedColor = Color.white;

    private Vector3 baseLocalScale;
    private Color spriteBaseColor;
    private Color outlineBaseColor;

    private Building building;

    private void Awake()
    {
        baseLocalScale = transform.localScale;
        outlineBaseColor = outline.color;
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref nameMesh, "Name");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref costMesh, "Cost");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref sprite, "Sprite");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref outline, "Outline");
    }

    public void Construct(BuildingData data)
    {
        if(building != null) building.OnStateChanged -= UpdateSpriteColors;

        nameMesh.text = data.GetName();
        costMesh.text = data.GetCost().ToString(ItemStack.Sign.Negative);
        spriteBaseColor = data.GetColor();

        building = new Building(data);
        building.OnStateChanged += UpdateSpriteColors;
        UpdateSpriteColors();
    }

    public void OnInputDown()
    {
        if (!enabled) return;

        if (!building.isEnabled) return;

        transform.localScale = baseLocalScale * 0.75f;
        sprite.color = spriteBaseColor * 0.75f;
    }

    public void OnInputUp()
    {
        if (!building.isEnabled) return;

        transform.localScale = baseLocalScale;
        sprite.color = spriteBaseColor;
    }

    public void OnInputUpInside()
    {
        if (!enabled) return;

        if (!building.isEnabled) return;

        if (building.isSelected) building.Deselect();
        else building.Select();
    }

    public void OnInputSecondaryDown()
    {

    }

    private void UpdateSpriteColors()
    {
        sprite.color = building.isEnabled ? spriteBaseColor : spriteDisabledColor;

        if (building.isEnabled)
        {
            outline.color = building.isSelected ? outlineSelectedColor : outlineBaseColor;
        }
        else outline.color = outlineDisabledColor;
    }
}
