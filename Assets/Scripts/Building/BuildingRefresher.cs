using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BuildingRefresher : MonoBehaviour, IInputReceiver, IInspectorSetter
{
    [SerializeField] private SpriteRenderer sprite = null;

    private Vector3 baseLocalScale;
    private Color baseSpriteColor;

    private void Start()
    {
        baseLocalScale = transform.localScale;
        baseSpriteColor = sprite.color;
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectInChildByName(transform, ref sprite, "Sprite");
    }

    public void OnInputDown()
    {
        if (!enabled) return;

        transform.localScale = baseLocalScale * 0.75f;
        sprite.color = baseSpriteColor * 0.75f;
    }

    public void OnInputUp()
    {
        transform.localScale = baseLocalScale;
        sprite.color = baseSpriteColor;
    }

    public void OnInputUpInside()
    {
        if (!enabled) return;

        BoardManager.Instance.AssignRandomBuildings();
        PeopleManager.Instance.EmployPerson();
    }

    public void OnInputSecondaryDown()
    {

    }
}
