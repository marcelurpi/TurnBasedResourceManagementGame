using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public static class InspectorSetter
{
#if UNITY_EDITOR
    [MenuItem("Custom Tools/Set All Inspector Objects")]
    private static void SetAllInspectorObjects()
    {
        MonoBehaviour[] components = Object.FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour component in components)
        {
            if(component is IInspectorSetter)
            {
                ((IInspectorSetter)component).SetInspectorComponents();
                EditorUtility.SetDirty(component);
            }
        }
    }
#endif

    public static void SetInspectorObjectInGameObjectByType<T>(Transform transform, ref T component) where T : Component
    {
#if UNITY_EDITOR
        component = transform.GetComponent<T>();
        if (component == null) Debug.LogError($"{ typeof(T).Name } not found at { transform.name }");
#endif
    }

    public static void SetInspectorObjectInChildByName<T>(Transform transform, ref T component, string toFind) where T : Component
    {
#if UNITY_EDITOR
        component = transform.Find(toFind)?.GetComponent<T>();
        if (component == null) Debug.LogError($"{ toFind } not found at { transform.name }");
#endif
    }

    public static void SetInspectorObjectInChildByType<T>(Transform transform, ref T component) where T : Component
    {
#if UNITY_EDITOR
        component = transform.GetComponentInChildren<T>();
        if (component == null) Debug.LogError($"{ typeof(T).Name } not found at { transform.name }");
#endif
    }

    public static void SetInspectorObjectsInChildByType<T>(Transform transform, ref T[] components) where T : Component
    {
#if UNITY_EDITOR
        components = transform.GetComponentsInChildren<T>();
        if (components.Length == 0) Debug.LogError($"{ typeof(T).Name } not found at { transform.name }");
#endif
    }

    public static void SetInspectorObjectFoundByType<T>(Transform transform, ref T component) where T : Component
    {
#if UNITY_EDITOR
        component = Object.FindObjectOfType<T>();
        if (component == null) Debug.LogError($"{ typeof(T).Name } not found at { transform.name }");
#endif
    }

    public static void SetInspectorObjectsFoundByType<T>(Transform transform, ref T[] components) where T : Component
    {
#if UNITY_EDITOR
        components = Object.FindObjectsOfType<T>();
        if (components.Length == 0) Debug.LogError($"{ typeof(T).Name } not found at { transform.name }");
#endif
    }
}


