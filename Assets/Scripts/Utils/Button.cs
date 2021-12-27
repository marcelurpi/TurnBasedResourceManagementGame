using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Button : MonoBehaviour, IInputReceiver, IInspectorSetter
{
    public event System.Action OnButtonInputUpInside;

    [SerializeField] private SpriteRenderer sprite;

    private Vector3 baseLocalScale;
    private Color spriteBaseColor;

    private void Awake()
    {
        baseLocalScale = transform.localScale;
        spriteBaseColor = sprite.color;
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectInChildByType(transform, ref sprite);
    }

    public void OnInputDown()
    {
        transform.localScale = baseLocalScale * 0.75f;
        sprite.color = spriteBaseColor * 0.75f;
    }

    public void OnInputSecondaryDown()
    {

    }

    public void OnInputUp()
    {
        transform.localScale = baseLocalScale;
        sprite.color = spriteBaseColor;
    }

    public void OnInputUpInside()
    {
        OnButtonInputUpInside?.Invoke();
    }
}
