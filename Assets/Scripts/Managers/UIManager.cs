using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour, IInspectorSetter
{
    public static UIManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private TextMeshPro unemployedPeopleMesh = null;
    [SerializeField] private TextMeshPro maxPeopleMesh = null;
    [SerializeField] private TextMeshPro missionTitleMesh = null;
    [SerializeField] private TextMeshPro missionResultMesh = null;
    [SerializeField] private TextMeshPro missionProgressMesh = null;
    [SerializeField] private TextMeshPro missionGoalMesh = null;
    [SerializeField] private TextMeshPro currentDayMesh = null;
    [SerializeField] private TextMeshPro missionMaxDayMesh = null;

    [Header("Parameters")]
    [SerializeField] private float PMaxOver9UnemployedPosX = 0;
    [SerializeField] private float progressOver99GoalPosX = 0;
    [SerializeField] private float progressOver9GoalPosX = 0;
    [SerializeField] private Color failColor = Color.white; 
    [SerializeField] private Color successColor = Color.white;

    private float baseUnemployedPeopleMeshX;
    private float baseMissionGoalMeshPosX;
    private Color baseUnemployedPeopleMeshColor;
    private Color baseMaxPeopleMeshColor;
    private Color baseMissionProgressMeshColor;
    private Color baseMissionGoalMeshColor;
    private Color baseCurrentDayMeshColor;
    private Color baseMissionMaxDayMeshColor;

    private void Awake()
    {
        Instance = this;

        baseUnemployedPeopleMeshX = unemployedPeopleMesh.transform.position.x;
        baseMissionGoalMeshPosX = missionGoalMesh.transform.position.x;

        baseUnemployedPeopleMeshColor = unemployedPeopleMesh.color;
        baseMaxPeopleMeshColor = maxPeopleMesh.color;
        baseMissionProgressMeshColor = missionProgressMesh.color;
        baseMissionGoalMeshColor = missionGoalMesh.color;
        baseCurrentDayMeshColor = currentDayMesh.color;
        baseMissionMaxDayMeshColor = missionMaxDayMesh.color;
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref unemployedPeopleMesh, "UnemployedPeople");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref maxPeopleMesh, "MaxPeople");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref missionTitleMesh, "MissionTitle");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref missionResultMesh, "MissionResult");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref missionProgressMesh, "MissionProgress");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref missionGoalMesh, "MissionGoal");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref currentDayMesh, "CurrentDay");
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref missionMaxDayMesh, "MissionMaxDay");
    }

    public void ResetColorMeshesHideResult()
    {
        unemployedPeopleMesh.color = baseUnemployedPeopleMeshColor;
        maxPeopleMesh.color = baseMaxPeopleMeshColor;
        missionProgressMesh.color = baseMissionProgressMeshColor;
        missionGoalMesh.color = baseMissionGoalMeshColor;
        currentDayMesh.color = baseCurrentDayMeshColor;
        missionMaxDayMesh.color = baseMissionMaxDayMeshColor;

        missionResultMesh.gameObject.SetActive(false);
    }

    public void UpdateUnemployedPeopleMesh(int unemployedPeople)
    {
        unemployedPeopleMesh.text = unemployedPeople.ToString();
    }

    public void UpdateMaxPeopleMesh(int maxPeople)
    {
        maxPeopleMesh.text = $"{ maxPeople } People";

        Vector3 unemployedPeopleMeshLocalPosition = unemployedPeopleMesh.transform.localPosition;
        if (maxPeople >= 10) unemployedPeopleMeshLocalPosition.x = PMaxOver9UnemployedPosX;
        else unemployedPeopleMeshLocalPosition.x = baseUnemployedPeopleMeshX;
        unemployedPeopleMesh.transform.localPosition = unemployedPeopleMeshLocalPosition;
    }

    public void UpdateMissionNameMesh(string missionTitle)
    {
        missionTitleMesh.text = missionTitle;
    }

    public void ShowAndUpdateMissionResultMesh(Color missionResultColor, string missionResult)
    {
        missionResultMesh.gameObject.SetActive(true);
        missionResultMesh.color = missionResultColor;
        missionResultMesh.text = missionResult;
    }

    public void UpdateMissionProgressMesh(int missionProgress)
    {
        missionProgressMesh.text = missionProgress.ToString();

        Vector3 missionGoalMeshLocalPosition = missionGoalMesh.transform.localPosition;
        if (missionProgress >= 100) missionGoalMeshLocalPosition.x = progressOver99GoalPosX;
        else if (missionProgress >= 10) missionGoalMeshLocalPosition.x = progressOver9GoalPosX;
        else missionGoalMeshLocalPosition.x = baseMissionGoalMeshPosX;
        missionGoalMesh.transform.localPosition = missionGoalMeshLocalPosition;
    }

    public void UpdateMissionGoalMesh(ItemStack missionGoal)
    {
        missionGoalMesh.text = missionGoal.ToString();
    }

    public void UpdateCurrentDayMesh(int currentDay)
    {
        currentDayMesh.text = currentDay.ToString();
    }

    public void UpdateMissionMaxDayMesh(int missionMaxDay)
    {
        missionMaxDayMesh.text = $"{ missionMaxDay } Days";
    }

    public void ChangeDayMeshesColorToFail()
    {
        currentDayMesh.color = failColor;
        missionMaxDayMesh.color = failColor;
    }

    public void ChangePeopleMeshesColorToFail()
    {
        unemployedPeopleMesh.color = failColor;
        maxPeopleMesh.color = failColor;
    }

    public void ChangeMissionMeshesColorToSuccess()
    {
        missionProgressMesh.color = successColor;
        missionGoalMesh.color = successColor;
    }

    public void SetUIMeshesActive(bool active)
    {
        unemployedPeopleMesh.gameObject.SetActive(active);
        maxPeopleMesh.gameObject.SetActive(active);
        missionTitleMesh.gameObject.SetActive(active);
        missionResultMesh.gameObject.SetActive(active);
        missionProgressMesh.gameObject.SetActive(active);
        missionGoalMesh.gameObject.SetActive(active);
        currentDayMesh.gameObject.SetActive(active);
        missionMaxDayMesh.gameObject.SetActive(active);
    }
}
