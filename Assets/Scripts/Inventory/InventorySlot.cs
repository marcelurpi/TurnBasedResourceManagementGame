using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class InventorySlot : MonoBehaviour, IInspectorSetter, IInputReceiver
{
    [SerializeField] private TextMeshPro itemStackMesh = null;

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref itemStackMesh, "ItemStack");
    }

    public void OnInputDown()
    {

    }

    public void OnInputSecondaryDown()
    {
        Inventory.Instance.RemoveItemInSlot(transform.GetSiblingIndex());
    }

    public void OnInputUp()
    {

    }

    public void OnInputUpInside()
    {

    }

    public void SetItem(ItemStack item)
    {
        itemStackMesh.text = item.IsNone() ? "Empty" : item.ToString();
    }
}
