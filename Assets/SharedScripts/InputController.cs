using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float SwipeDistanceFilter = 20;
    
    public static Action OnLeft;
    public static Action OnRight;
    public static Action OnUp;
    public static Action OnDown;
    public static Action OnAction;
    
    private Vector2 startingMousePos;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            OnDown?.Invoke();
        if (Input.GetKeyDown(KeyCode.W))
            OnUp?.Invoke();
        if (Input.GetKeyDown(KeyCode.A))
            OnLeft?.Invoke();
        if (Input.GetKeyDown(KeyCode.D))
            OnRight?.Invoke();
        if (Input.GetKeyDown(KeyCode.Space))
            OnAction?.Invoke();
        
        if (Input.touches.Length > 0)
        {
            var touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
                InitSwipe(touch.position);
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                FinishSwipe(touch.position);
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                InitSwipe(Input.mousePosition);
            else if (Input.GetMouseButtonUp(0))
                FinishSwipe(Input.mousePosition);
        }
    }
    
    private void InitSwipe(Vector2 start)
    {
        startingMousePos = start;
    }
    
    private void FinishSwipe(Vector2 end)
    {
        var delta = end - startingMousePos;
        if (delta.magnitude < SwipeDistanceFilter)
        {
            OnAction?.Invoke();
            return;
        }

        if (Math.Abs(delta.x) > Math.Abs(delta.y))
        {
            if (delta.x > 0) 
                OnRight?.Invoke();
            else 
                OnLeft?.Invoke();
        }
        else
        {
            if (delta.y > 0) 
                OnUp?.Invoke();
            else 
                OnDown?.Invoke();
        }
    }
}
