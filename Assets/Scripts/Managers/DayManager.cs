using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    public static event System.Action OnDayEnded = null;

    private int currentDay;
    private int missionMaxDay;

    private void Awake()
    {
        Instance = this;
    }

    public void Construct(int missionMaxDay)
    {
        this.missionMaxDay = missionMaxDay;
        UIManager.Instance.UpdateMissionMaxDayMesh(missionMaxDay);

        currentDay = 1;
        UIManager.Instance.UpdateCurrentDayMesh(currentDay);
    }

    public void PassDay()
    {
        if(currentDay == missionMaxDay)
        {
            UIManager.Instance.ChangeDayMeshesColorToFail();

            MissionManager.Instance.MissionFailed();

            return;
        }

        currentDay++;
        OnDayEnded?.Invoke();

        UIManager.Instance.UpdateCurrentDayMesh(currentDay);
    }
}
