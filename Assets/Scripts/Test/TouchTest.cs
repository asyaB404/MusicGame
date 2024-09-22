using System;
using UnityEngine;

public class TouchTest : MonoBehaviour
{
    private Vector2 _lastPos = Vector2.zero;

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // 在PC上使用鼠标模拟触摸输入
        Vector2 mousePosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            SimulateTouch(mousePosition, TouchPhase.Began);
            _lastPos = mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            SimulateTouch(mousePosition, _lastPos != mousePosition ? TouchPhase.Moved : TouchPhase.Stationary);
            _lastPos = mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
            SimulateTouch(mousePosition, TouchPhase.Ended);


#elif UNITY_ANDROID || UNITY_IOS
    // 在移动设备上使用真实的触摸输入
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        HandleTouch(touch);
    }

#elif UNITY_WEBGL
    // 在WebGL中可以支持触摸或鼠标输入
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        HandleTouch(touch);
    }
    else if (Input.GetMouseButton(0)) 
    {
        Vector2 mousePosition = Input.mousePosition;
        SimulateTouch(mousePosition);
    }

#endif
    }

    private void SimulateTouch(Vector2 position, TouchPhase phase)
    {
        // 模拟触摸输入处理逻辑
        Touch simulatedTouch = new Touch
        {
            fingerId = 0,
            position = position,
            phase = phase,
            tapCount = 1,
        };

        HandleTouch(simulatedTouch);
    }

    private void HandleTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                Debug.Log("模拟触摸开始: " + touch.position);
                break;
            case TouchPhase.Moved:
                Debug.Log("模拟触摸移动: " + touch.position);
                break;
            case TouchPhase.Ended:
                Debug.Log("模拟触摸结束");
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Canceled:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}