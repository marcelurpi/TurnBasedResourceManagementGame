using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputReceiver
{
    void OnInputDown();

    void OnInputUp();

    void OnInputUpInside();

    void OnInputSecondaryDown();
}
