using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    [Header("Parameters")]
    [SerializeField] private ConfigSO currentConfig = null;
    [SerializeField] private Color missionResultColorSucceed = Color.white;
    [SerializeField] private Color missionResultColorFail = Color.white;

    private int missionProgress;
    private ItemStack missionGoal;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        StartCurrentMission();
    }

    public void StartCurrentMission()
    {
        ConfigManager.Instance.Construct();

        StartGame();

        MissionData currentMission = currentConfig.GetCurrentMission();

        UIManager.Instance.ResetColorMeshesHideResult();

        Inventory.Instance.Construct();

        BoardManager.Instance.Construct(currentConfig.GetEmptyTile(), currentMission.GetStartingTiles(),
            currentMission.GetAlreadyPlacedBuildings(), currentMission.GetAllowedBuildings());

        PeopleManager.Instance.Construct(BoardManager.Instance.GetStartingMaxPeople());
        DayManager.Instance.Construct(currentMission.GetMaxDaysToComplete());
        EventManager.Instance.Construct(currentMission.GetAllowedEvents());

        UIManager.Instance.UpdateMissionNameMesh(currentMission.GetName());

        missionGoal = currentMission.GetGoal();
        UIManager.Instance.UpdateMissionGoalMesh(missionGoal);

        Inventory.Instance.OnInventoryContentsUpdated += UpdateMissionProgress;
        UpdateMissionProgress();
    }

    public void MissionFailed()
    {
        StopGame();
        UIManager.Instance.ShowAndUpdateMissionResultMesh(missionResultColorFail, "Mission Failed");
    }

    private void UpdateMissionProgress()
    {
        missionProgress = Inventory.Instance.GetItemAmountStored(missionGoal.GetName());
        UIManager.Instance.UpdateMissionProgressMesh(missionProgress);

        if (missionProgress >= missionGoal.GetAmount())
        {
            UIManager.Instance.ChangeMissionMeshesColorToSuccess();

            MissionSucceeded();
        }
    }

    private void MissionSucceeded()
    {
        StopGame();
        UIManager.Instance.ShowAndUpdateMissionResultMesh(missionResultColorSucceed, "Mission Succeeded");
    }

    private void StartGame()
    {
        MonoBehaviour[] components = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour component in components)
        {
            if (component is IInputReceiver)
            {
                component.enabled = true;
            }
        }
    }

    private void StopGame()
    {
        MonoBehaviour[] components = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour component in components)
        {
            if (component is IInputReceiver)
            {
                component.enabled = false;
                ((IInputReceiver)component).OnInputUp();
            }
        }
    }
}
