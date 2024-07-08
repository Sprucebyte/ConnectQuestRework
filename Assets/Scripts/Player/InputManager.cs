using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2 mousePositionScreenSpace { get; private set; }
    public Vector2 mousePositionWorldSpace { get; private set; }
    public Vector2 mouseSpeed { get; private set; }
    public Vector2 lastMousePosition { get; private set; }

    private void Start()
    {
        mousePositionScreenSpace = Input.mousePosition;
        mousePositionWorldSpace = Camera.main.ScreenToWorldPoint(mousePositionScreenSpace);
        lastMousePosition = mousePositionScreenSpace;
        mouseSpeed = mousePositionScreenSpace - lastMousePosition;
    }

    private void Update()
    {
        lastMousePosition = mousePositionScreenSpace;
        mousePositionScreenSpace = Input.mousePosition;
        mousePositionWorldSpace = Camera.main.ScreenToWorldPoint(mousePositionScreenSpace);
        mouseSpeed = mousePositionScreenSpace - lastMousePosition;
    }

    

}
