using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using GamePlay;
using UnityEngine;

/// <summary>
/// 负责管理触摸
/// </summary>
public class TouchSystem : MonoBehaviour
{
    private Vector2 _lastPos = Vector2.zero;

    /// <summary>
    /// 仅建议2D场景使用
    /// </summary>
    public bool openTouchPrefab;

    [SerializeField] private GameObject touchPrefab;
    private readonly Dictionary<int, GameObject> _touchObjsDict = new();


    private void Start()
    {
    }

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
            for (int i = 0; i < Input.touchCount; i++)
            {
                HandleTouch(Input.GetTouch(i));
            }
        }

#elif UNITY_WEBGL
    // 在WebGL中可以支持触摸或鼠标输入
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                HandleTouch(Input.GetTouch(i));
            }
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
        var simulatedTouch = new Touch
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
        var mainCamera = CameraManager.Main;
        if (mainCamera == null) return;
        Vector3 touchWorldPos = mainCamera.ScreenToWorldPoint(touch.position);

        touchWorldPos.z = 0;
        int handlePos = -1;
        Ray ray = mainCamera.ScreenPointToRay(touch.position);
        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.collider)
            {
                if (NotesObjManager.Instance.KeyToPos?.TryGetValue(hit.collider, out handlePos) == true)
                    NotesObjManager.Instance.HitBoxes[handlePos].isTouching =
                        touch.phase is not (TouchPhase.Ended or TouchPhase.Canceled);
            }
        }


        switch (touch.phase)
        {
            case TouchPhase.Began:
                SpawnTouch(touch, touchWorldPos);
                GameManager.Instance.ResolveNote(handlePos);
                break;
            case TouchPhase.Moved:
                if (openTouchPrefab)
                    _touchObjsDict[touch.fingerId].transform.position = touchWorldPos;
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                RemoveTouch(touch);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SpawnTouch(Touch touch, Vector2 worldPos)
    {
        if (!openTouchPrefab) return;
        GameObject o = Instantiate(touchPrefab, worldPos, Quaternion.identity);
        o.transform.localScale = Vector3.zero;
        o.transform.DOScale(1.25f, 0.1f);
        _touchObjsDict[touch.fingerId] = o;
    }

    private void RemoveTouch(Touch touch)
    {
        if (!openTouchPrefab) return;
        GameObject curTouchObj = _touchObjsDict[touch.fingerId];
        curTouchObj.GetComponent<SpriteRenderer>().DOFade(0, 0.1f).OnComplete(() => { Destroy(curTouchObj); });
        _touchObjsDict.Remove(touch.fingerId);
    }
}