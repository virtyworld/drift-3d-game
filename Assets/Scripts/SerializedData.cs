using System;
using UnityEngine;
using UnityEngine.UI;

public struct SerializedData { }

[Serializable]
public struct CarData
{
    public CarName name;
    public GameObject prefab;
}
[Serializable]
public class ButtonPanelPair
{
    public Button button;
    public GameObject panel;
}