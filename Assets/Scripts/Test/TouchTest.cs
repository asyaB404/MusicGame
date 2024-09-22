using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Touch touch = new Touch
            {
                fingerId = 0,
                position = Input.mousePosition,
                phase = Input.GetMouseButtonDown(0)
                    ? TouchPhase.Began
                    : (Input.GetMouseButtonUp(0) ? TouchPhase.Ended : TouchPhase.Moved),
                tapCount = 1,
            };

            // 处理你的触摸逻辑
            HandleTouch(touch);
        }
    }

    void HandleTouch(Touch touch)
    {
        // 处理触摸事件
        if (touch.phase == TouchPhase.Began)
        {
            Debug.Log("模拟触摸开始: " + touch.position);
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            Debug.Log("模拟触摸移动: " + touch.position);
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            Debug.Log("模拟触摸结束");
        }
    }
}