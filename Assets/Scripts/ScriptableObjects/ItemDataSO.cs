using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDataSO : ScriptableObject
{
    [SerializeField] private string itemName = null;

    public string GetName()
    {
        return itemName;
    }
}
