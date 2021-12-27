using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour, IInspectorSetter
{
    [SerializeField] private Camera mainCamera = null;

    private float startTouchTime = -1;
    private Transform stillClickingTransform = null;

    private void Update()
    {
        Vector3 inputPosition = Input.mousePresent ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;
        Vector3 actualInputPosition = mainCamera.ScreenToWorldPoint(inputPosition);
        RaycastHit2D hit = Physics2D.Raycast(actualInputPosition, Vector2.zero);
        if (hit.collider != null)
        {
            HandleLongTap(hit.collider.transform);
            InsideTransform(hit.collider.transform);
        }
        HandleInputUp(hit.collider != null ? hit.collider.transform : null);
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectFoundByType(transform, ref mainCamera);
    }

    private void HandleLongTap(Transform currentTransform)
    {
        if (startTouchTime != -1 && Time.time - startTouchTime > 0.5f)
        {
            startTouchTime = -1;
            foreach (IInputReceiver receiver in currentTransform.GetComponents<IInputReceiver>())
            {
                receiver.OnInputSecondaryDown();
            }
        }
    }

    private void InsideTransform(Transform newTransform)
    {
        if(Input.mousePresent && Input.GetMouseButtonDown(1))
        {
            foreach (IInputReceiver receiver in newTransform.GetComponents<IInputReceiver>())
            {
                receiver.OnInputSecondaryDown();
            }
        }
        if (Input.mousePresent ? Input.GetMouseButtonDown(0) : Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if(!Input.mousePresent) startTouchTime = Time.time;
            foreach (IInputReceiver receiver in newTransform.GetComponents<IInputReceiver>())
            {
                receiver.OnInputDown();
            }
            stillClickingTransform = newTransform;
        }
    }

    private void HandleInputUp(Transform currentTransform)
    {
        bool inputUp = Input.mousePresent ? Input.GetMouseButtonUp(0) : Input.GetTouch(0).phase == TouchPhase.Ended;
        if (inputUp && stillClickingTransform != null)
        {
            startTouchTime = -1;
            if (currentTransform == stillClickingTransform)
            {
                foreach (IInputReceiver receiver in currentTransform.GetComponents<IInputReceiver>())
                {
                    receiver.OnInputUpInside();
                }
            }

            foreach (IInputReceiver receiver in stillClickingTransform.GetComponents<IInputReceiver>())
            {
                receiver.OnInputUp();
            }
            stillClickingTransform = null;
        }
    }
}
