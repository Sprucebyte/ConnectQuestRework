using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Background
{
    public Sprite background;
    public Sprite foreground;
}


[System.Serializable]
public class BackgroundLayered
{
    public List<Sprite> layers;
}
