// ItemCountDisplay.cs (Legacy Text 兼容版)

using UnityEngine;
using UnityEngine.UI; // 必须导入 UnityEngine.UI 命名空间

public class ItemCountDisplay : MonoBehaviour
{
    [Tooltip("拖入这个UI元素要显示的对应物品 Scriptable Object (例如：soulGrass)")]
    public Item itemToDisplay;

    // *** 关键修改：使用 UnityEngine.UI.Text 组件 ***
    [Tooltip("拖入显示数量的 Text 组件 (Legacy UI)")]
    public Text textDisplay;

    void Start()
    {
        // 确保在 Start 时就显示初始数量
        UpdateDisplay();
    }

    // 核心方法：被 InventoryManager 调用的更新方法
    public void UpdateDisplay()
    {
        // 检查所有引用是否都已设置
        if (itemToDisplay == null || textDisplay == null || InventoryManager.Instance == null)
        {
            if (textDisplay == null)
            {
                Debug.LogError($"ItemCountDisplay 脚本 ({gameObject.name}) 缺少 Text Display 引用！请在 Inspector 中拖入 UnityEngine.UI.Text 组件。", this);
            }
            return;
        }

        // 1. 从 InventoryManager 获取数量
        // 确保 InventoryManager.cs 中 GetItemCount 方法已添加（我们前一步已解决编译问题）
        int count = InventoryManager.Instance.GetItemCount(itemToDisplay);

        // 2. 更新 Text
        textDisplay.text = count.ToString();
    }
}