using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
public class TileBehaviour : MonoBehaviour, IInputReceiver, IInspectorSetter
{
    [SerializeField] private TextMeshPro nameMesh = null;
    [SerializeField] private TextMeshPro actionNameMesh = null;
    [SerializeField] private TextMeshPro actionPrimaryCostMesh = null;
    [SerializeField] private TextMeshPro actionSecondaryCostMesh = null;
    [SerializeField] private TextMeshPro actionRewardMesh = null;
    [SerializeField] private SpriteRenderer sprite = null;
    [SerializeField] private SpriteRenderer outline = null;
    [SerializeField] private float centeredActionRewardMeshPositionY = 0;
    [SerializeField] private Color spriteDisabledColor = Color.white; 
    [SerializeField] private Color outlineDisabledColor = Color.white;

    private Vector3 baseLocalScale;
    private Color spriteBaseColor;
    private Color outlineBaseColor;
    private float baseRewardPositionY;
    private float basePrimaryCostPositionX;

    private Tile tile;

    private void Awake()
    {
        baseLocalScale = transform.localScale;
        outlineBaseColor = outline.color;
        baseRewardPositionY = actionRewardMesh.transform.localPosition.y;
        basePrimaryCostPositionX = actionPrimaryCostMesh.transform.localPosition.x;
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref nameMesh, "Name");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref actionNameMesh, "Action");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref actionPrimaryCostMesh, "PrimaryCost");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref actionSecondaryCostMesh, "SecondaryCost");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref actionRewardMesh, "Reward");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref sprite, "Sprite");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref outline, "Outline");
    }

    public void Construct(TileData data, int daysToConstruct)
    {
        Building.OnBuildingSelectionChanged -= UpdateBuildingSpriteColor;

        if (tile != null) tile.OnConstructionFinished -= ShowTileActionInfo;

        tile = new Tile(data, daysToConstruct);
        tile.OnConstructionFinished += ShowTileActionInfo;

        spriteBaseColor = data.GetColor();

        sprite.color = data.GetColor();
        nameMesh.text = data.GetName();

        if (daysToConstruct > 0)
        {
            actionNameMesh.text = "Building";
            HideCostMeshes();
        }
        else ShowTileActionInfo(data.GetActionName(), data.GetActionPrimaryCost(), data.GetActionSecondaryCost(), data.GetActionReward());

        Building.OnBuildingSelectionChanged += UpdateBuildingSpriteColor;
    }

    public void OnInputDown()
    {
        if (!enabled) return;

        if (!tile.CanExecuteTileAction()) return;

        transform.localScale = baseLocalScale * 0.75f;
        sprite.color = spriteBaseColor * 0.75f;
    }

    public void OnInputUp()
    {
        transform.localScale = baseLocalScale;

        if (!tile.CanExecuteTileAction()) return;

        sprite.color = spriteBaseColor;
    }

    public void OnInputUpInside()
    {
        if (!enabled) return;

        tile.ExecuteTileAction(this);
    }

    public void OnInputSecondaryDown()
    {
        tile.DestroyBuilding(this);
    }

    private void ShowTileActionInfo(string actionTitle, ItemStack primaryCost, ItemStack secondaryCost, ItemStack reward)
    {
        actionNameMesh.text = actionTitle;

        actionPrimaryCostMesh.text = primaryCost.ToString(ItemStack.Sign.Negative);
        actionSecondaryCostMesh.text = secondaryCost.ToString(ItemStack.Sign.Negative);
        actionRewardMesh.text = reward.ToString(ItemStack.Sign.Positive);

        actionNameMesh.gameObject.SetActive(true);
        actionPrimaryCostMesh.gameObject.SetActive(true);
        actionSecondaryCostMesh.gameObject.SetActive(true);
        actionRewardMesh.gameObject.SetActive(true);

        RearrangeTextMeshesIfCostsNone(primaryCost, secondaryCost);
    }

    private void HideCostMeshes()
    {
        actionPrimaryCostMesh.gameObject.SetActive(false);
        actionSecondaryCostMesh.gameObject.SetActive(false);
        actionRewardMesh.gameObject.SetActive(false);
    }

    private void RearrangeTextMeshesIfCostsNone(ItemStack primaryCost, ItemStack secondaryCost)
    {
        Vector3 rewardMeshLocalPosition = actionRewardMesh.transform.localPosition;
        rewardMeshLocalPosition.y = primaryCost.IsNone() ? centeredActionRewardMeshPositionY : baseRewardPositionY;
        actionRewardMesh.transform.localPosition = rewardMeshLocalPosition;

        Vector3 primaryCostMeshLocalPosition = actionPrimaryCostMesh.transform.localPosition;
        primaryCostMeshLocalPosition.x = secondaryCost.IsNone() ? 0 : basePrimaryCostPositionX;
        actionPrimaryCostMesh.transform.localPosition = primaryCostMeshLocalPosition;
    }

    private void UpdateBuildingSpriteColor()
    {
        sprite.color = tile.SelectedBuildingUnconstructable() ? spriteDisabledColor : spriteBaseColor;
        outline.color = tile.SelectedBuildingUnconstructable() ? outlineDisabledColor : outlineBaseColor;
    }
}
