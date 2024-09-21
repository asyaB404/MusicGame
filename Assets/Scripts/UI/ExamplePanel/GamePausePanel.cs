using UnityEngine.UI;

namespace UI.ExamplePanel
{
    /// <summary>
    /// 示例面板，暂停的面板，注意做好游戏暂停时状态的管理
    /// </summary>
    public class GamePausePanel : BasePanel<GamePausePanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Button>("return").onClick.AddListener(HideMe);
            GetControl<Button>("exit").onClick.AddListener(() =>
            {
                GamePanel.Instance.HideMe();
            });
        }
    }
}