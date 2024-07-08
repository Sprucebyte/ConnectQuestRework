using log4net.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;






[CustomEditor(typeof(int))]
public class CustPropertyDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        for (int x = 0; x < 5; x++)
        {
            
            for (int y = 0; y < 5; y++)
            {

            }
        }
    }
}