using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour, IInspectorSetter
{
    public static EventManager Instance = null;

    [SerializeField] private EventSlot eventSlot = null;

    private bool constructedBefore = false;

    private int currentEventDaysLeft = 0;
    private EventData currentEvent = null;
    private EventData[] allowedEvents = null;

    private void Awake()
    {
        Instance = this;
        constructedBefore = false;
    }

    public void Construct(EventData[] allowedEvents)
    {
        if (!constructedBefore)
        {
            DayManager.OnDayEnded += DecrementDaysLeft;
            constructedBefore = true;
        }

        this.allowedEvents = allowedEvents;
        ChooseEvent();
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectFoundByType(transform, ref eventSlot);
    }

    public void SetSlotActive(bool active)
    {
        eventSlot.gameObject.SetActive(active);
    }

    private void ChooseEvent()
    {
        currentEvent = allowedEvents[Random.Range(0, allowedEvents.Length)];
        eventSlot.Construct(currentEvent);

        currentEventDaysLeft = currentEvent.GetDaysToPrepare();

        if (currentEventDaysLeft == 0) ExecuteCurrentEvent();
    }

    private void DecrementDaysLeft()
    {
        currentEventDaysLeft--;
        eventSlot.UpdateDaysLeftMesh(currentEventDaysLeft);

        if (currentEventDaysLeft == 0) ExecuteCurrentEvent();
    }

    private void ExecuteCurrentEvent()
    {
        ItemStack eventCost = currentEvent.GetCost();
        if (!eventCost.IsNone() && !Inventory.Instance.Contains(eventCost))
        {
            MissionManager.Instance.MissionFailed();
            eventSlot.SetSpriteColorToFail();
            return;
        }

        Inventory.Instance.Retrieve(eventCost);
        Inventory.Instance.Store(currentEvent.GetReward());
        ChooseEvent();
    }
}
