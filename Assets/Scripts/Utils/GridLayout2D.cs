using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridLayout2D : MonoBehaviour
{
    public Vector2 CellSize = Vector2.one;
    public Vector2Int MaxCells = Vector2Int.one;
    public Vector2 Spacing;

    [SerializeField] private GridCentered Centered = GridCentered.Both;
    [SerializeField] private GridStartCorner StartCorner = GridStartCorner.UpperLeft;
    [SerializeField] private GridStartAxis StartAxis = GridStartAxis.Horizontal;

    private int OldChildrenCount;
    private Vector2 MaxCellsDirectioned;

    private enum GridStartCorner
    {
        UpperLeft,
        UpperRight,
        LowerLeft,
        LowerRight,
    }

    private enum GridCentered
    {
        None,
        Horizontal,
        Vertical,
        Both,
    }

    private enum GridStartAxis
    {
        Horizontal,
        Vertical,
    }

    private void Update()
    {
        int childrenCount = transform.childCount;
        if (OldChildrenCount != childrenCount)
        {
            UpdateChildrenPosition();
            OldChildrenCount = childrenCount;
        }
    }

    private void OnValidate()
    {
        UpdateChildrenPosition();
    }

    [ContextMenu("Update Children Position")]
    public void UpdateChildrenPosition(Vector2Int newMaxCells = new Vector2Int(), bool ignoreMoreItemsThanCellsWarning = false)
    {
        OldChildrenCount = transform.childCount;
        if (newMaxCells != new Vector2Int())
        {
            MaxCells = newMaxCells;
        }
        int childrenIndexReached = 0;
        MaxCellsDirectioned = StartAxis == GridStartAxis.Horizontal ? MaxCells : new Vector2Int(MaxCells.y, MaxCells.x);
        for (int j = 0; j < MaxCellsDirectioned.y; j++)
        {
            for (int i = 0; i < MaxCellsDirectioned.x; i++)
            {
                if (transform.childCount > childrenIndexReached)
                {
                    Vector2Int direction = StartAxis == GridStartAxis.Horizontal ? new Vector2Int(i, j) : new Vector2Int(j, i);
                    Vector2 position = (CellSize + Spacing) * (GetCenteredPosition() + direction) * GetStartMultiplier();
                    transform.GetChild(childrenIndexReached).localPosition = position;
                    transform.GetChild(childrenIndexReached).gameObject.SetActive(true);
                    childrenIndexReached++;
                }
            }
        }
        if (childrenIndexReached < transform.childCount && !ignoreMoreItemsThanCellsWarning)
        {
            Debug.Log("More Items than MaxCells in GridLayout2D in " + gameObject.name);
        }
        for (int i = childrenIndexReached; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private Vector2Int GetStartMultiplier()
    {
        switch (StartCorner)
        {
            case GridStartCorner.UpperLeft:
                return new Vector2Int(1, -1);
            case GridStartCorner.UpperRight:
                return new Vector2Int(-1, -1);
            case GridStartCorner.LowerLeft:
                return new Vector2Int(1, 1);
            case GridStartCorner.LowerRight:
                return new Vector2Int(-1, 1);
            default:
                return Vector2Int.zero;
        }
    }

    private Vector2 GetCenteredPosition()
    {
        switch (Centered)
        {
            case GridCentered.None:
                return Vector2.zero;
            case GridCentered.Horizontal:
                return new Vector2(GetGlobalCenterPosition().x, 0);
            case GridCentered.Vertical:
                return new Vector2(0, GetGlobalCenterPosition().y);
            case GridCentered.Both:
                return GetGlobalCenterPosition();
            default:
                return Vector2.zero;
        }
    }

    private Vector2 GetGlobalCenterPosition()
    {
        if (StartAxis == GridStartAxis.Horizontal)
        {
            Vector2 cellCount = Vector2.Min(new Vector2(transform.childCount, Mathf.Ceil((float)transform.childCount / MaxCells.x)), MaxCells);
            return ((cellCount - Vector2.one) * -0.5f);
        }
        else
        {
            Vector2 cellCount = Vector2.Min(new Vector2(Mathf.Ceil((float)transform.childCount / MaxCells.y), transform.childCount), MaxCells);
            return ((cellCount - Vector2.one) * -0.5f);
        }
    }
}