using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class MapAnimator : MonoBehaviour
{
    [HorizontalLine(2f, EColor.Red)]
    [CurveRange(0, 0, 5, 5, EColor.Red)]
    [BoxGroup("Curves")]
    public AnimationCurve sizeCurve;
    //[BoxGroup("Info")]
    bool open = false;
    [Range(0, 5)]
    [BoxGroup("Curves")]
    public float pos = 0;


    Vector3 fullScale;
    [Button]
    public void Open()
    {
        open = true;
    }
    [Button]
    public void Close()
    {
        open = false;
    }

    public void Start()
    {
        fullScale = transform.localScale;
    }
    public void Update()
    {

        transform.localScale = fullScale * sizeCurve.Evaluate(pos);
        
    }

}

