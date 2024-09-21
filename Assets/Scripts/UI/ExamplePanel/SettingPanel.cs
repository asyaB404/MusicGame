using UnityEngine.UI;

namespace UI.ExamplePanel
{
    /// <summary>
    /// 示例面板设置
    /// </summary>
    public class SettingPanel : BasePanel<SettingPanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Button>("return").onClick.AddListener(HideMe);
        }
    }
}