using UnityEngine;
using UnityEngine.UI;

namespace UI.ExamplePanel
{
    /// <summary>
    /// 示例面板，游戏结束面板
    /// </summary>
    public class GameOverPanel : BasePanel<GameOverPanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Button>("restart").onClick.AddListener(() =>
            {
                Debug.Log("发送重置游戏事件~");
                HideMe();
            });
            GetControl<Button>("exit").onClick.AddListener(Exit);
        }

        private void Exit()
        {
            UIManager.Instance.ClearPanels();
            GameStartPanel.Instance.ShowMe();
        }

        public override void OnPressedEsc()
        {
            Exit();
        }
    }
}