using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ResetMission : MonoBehaviour, IInputReceiver
{
    public void OnInputDown()
    {

    }

    public void OnInputSecondaryDown()
    {

    }

    public void OnInputUp()
    {

    }

    public void OnInputUpInside()
    {
        MissionManager.Instance.StartCurrentMission();
    }
}
