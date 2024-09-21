using UnityEngine.UI;

namespace UI.ExamplePanel
{
    /// <summary>
    /// 示例面板，游戏中的面板，可以在这添加类似血条等GamingUI
    /// </summary>
    public class GamePanel : BasePanel<GamePanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Button>("menu").onClick.AddListener(() => { GamePausePanel.Instance.ShowMe(); });
            GetControl<Button>("lost").onClick.AddListener(() => { GameOverPanel.Instance.ShowMe(); });
        }

        public override void OnPressedEsc()
        {
            GamePausePanel.Instance.ShowMe();
        }
    }
}