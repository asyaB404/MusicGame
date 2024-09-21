using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        private readonly Stack<IBasePanel> _panelStack = new();
        public IReadOnlyCollection<IBasePanel> Panels => _panelStack;
        public static UIManager Instance { get; private set; }

        private void Start()
        {
            //GameStartPanel.Instance.ShowMe();
        }

        /// <summary>
        /// 为所有的面板统一执行Init初始化方法
        /// </summary>
        private void Awake()
        {
            Instance = this;
            var basePanels = GetComponentsInChildren<IBasePanel>(true);
            foreach (var panel in basePanels) panel.Init();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                if (Peek() != null)
                    Peek().OnPressedEsc();
        }

        /// <summary>
        /// 清理面板栈，会将栈内的全部面板执行HideMe的同时执行gameObject.SetActive(false)
        /// </summary>
        public void ClearPanels()
        {
            int count = _panelStack.Count;
            for (int i = 0; i < count; i++)
            {
                var curPanel = Peek();
                curPanel.HideMe();
                (curPanel as MonoBehaviour)?.gameObject.SetActive(false);
            }

            _panelStack.Clear();
        }


        /// <summary>
        ///     存入栈中，没有特殊情况建议不要使用。
        ///     因为面板基类的ShowMe和HideMe的默认实现已经对栈元素的进出进行了管理，一般直接调用ShowMe和HideMe即可。
        /// </summary>
        /// <param name="basePanel">要存入栈的面板</param>
        /// <param name="callback">是否先执行栈顶的渐渐隐藏</param>
        /// /// <example>例如[1]当2被存入后,变成了[1,2],此时callback参数决定1会不会调用CallBack(false)</example>
        public void PushPanel(IBasePanel basePanel, bool callback = true)
        {
            if (basePanel.IsInStack)
            {
                Debug.LogWarning("已经存在于栈内，无法再将其存入栈内");
                return;
            }

            if (callback && Peek() != null) Peek().CallBack(false);

            _panelStack.Push(basePanel);
            basePanel.CallBack(true);
        }

        /// <summary>
        ///     弹出栈顶元素，没有特殊情况建议不要使用。
        ///     因为面板基类的ShowMe和HideMe的默认实现已经对栈元素的进出进行了管理，一般直接调用ShowMe和HideMe即可。
        /// </summary>
        /// <param name="callback">弹出后，是否执行新的栈顶的渐渐显示</param>
        /// <example>例如[1,2]当2被弹出后,变成了[1],此时callback参数决定1会不会调用CallBack(true)</example>
        public IBasePanel PopPanel(bool callback = true)
        {
            if (_panelStack.Count <= 0)
            {
                Debug.LogError("栈为空,不能弹出");
                return null;
            }

            if (Peek() != null) Peek().CallBack(false);

            var res = _panelStack.Pop();
            if (callback && Peek() != null) Peek().CallBack(true);

            return res;
        }

        /// <summary>
        /// 得到栈顶元素，没有时返回null
        /// </summary>
        /// <returns></returns>
        public IBasePanel Peek()
        {
            return _panelStack.Count <= 0 ? null : _panelStack.Peek();
        }

        #region Debug

        [ContextMenu("PrintStack")]
        private void PrintStack()
        {
            Debug.Log("_panelStack还剩: " + _panelStack.Count + "个,分别是");
            foreach (var item in _panelStack)
            {
                Debug.Log(item);
            }
        }

        #endregion
    }
}