using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building
{
    public event System.Action OnStateChanged;

    public static event System.Action OnBuildingSelectionChanged;

    public static Building selected = null;

    public bool isEnabled { get; private set; }
    public bool isSelected { get; private set; }

    public readonly BuildingData data;

    public Building(BuildingData data)
    {
        this.data = data;

        isEnabled = false;
        isSelected = false;

        Inventory.Instance.OnInventoryContentsUpdated += UpdateState;
        UpdateState();
    }

    public void Enable()
    {
        if (!isEnabled)
        {
            isEnabled = true;
            OnStateChanged?.Invoke();
        }
    }

    public void Disable()
    {
        if (isEnabled)
        {
            isEnabled = false;
            isSelected = false;
            selected = null;
            OnStateChanged?.Invoke();
        }
    }

    public void Select()
    {
        if (!isSelected)
        {
            if(selected != null) selected.Deselect();

            isSelected = true;
            selected = this;
            OnStateChanged?.Invoke();
            OnBuildingSelectionChanged?.Invoke();
        }
    }

    public void Deselect()
    {
        if (isSelected)
        {
            isSelected = false;
            selected = null;
            OnStateChanged?.Invoke();
            OnBuildingSelectionChanged?.Invoke();
        }
    }

    private void UpdateState()
    {
        bool shouldBeEnabled = data.GetCost().IsNone() || Inventory.Instance.Contains(data.GetCost());
        if (isEnabled && !shouldBeEnabled) Disable();
        if (!isEnabled && shouldBeEnabled) Enable();
    }
}
