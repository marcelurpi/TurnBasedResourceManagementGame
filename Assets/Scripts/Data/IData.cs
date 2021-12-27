using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IData
{
    string GetName();

    void OnValidate(ConfigSO configSaver);
}
