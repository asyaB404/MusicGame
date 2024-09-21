using UnityEngine;
using UnityEngine.UI;

namespace UI.ExamplePanel
{
    /// <summary>
    /// 示例面板，选关
    /// </summary>
    public class SelectLevelPanel : BasePanel<SelectLevelPanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Button>("level1").onClick.AddListener(() =>
            {
                GamePanel.Instance.ShowMe();
                Debug.Log("进入了第1关");
            });
            GetControl<Button>("level2").onClick.AddListener(() =>
            {
                GamePanel.Instance.ShowMe();
                Debug.Log("进入了第2关");
            });
            GetControl<Button>("level3").onClick.AddListener(() =>
            {
                GamePanel.Instance.ShowMe();
                Debug.Log("进入了第3关");
            });
        }
    }
}