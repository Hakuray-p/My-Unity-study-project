using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 操作说明管理器：控制操作说明面板的显示和隐藏
/// </summary>
public class ControlsGuideManager : MonoBehaviour
{
    [Header("UI 引用")]
    [Tooltip("操作说明面板（需要在Unity中创建并拖入）")]
    [SerializeField] private GameObject controlsPanel;

    [Tooltip("打开操作说明的按钮（可选，如果为空则需要在Unity中手动绑定）")]
    [SerializeField] private Button openButton;

    [Tooltip("关闭操作说明的按钮（在面板内部）")]
    [SerializeField] private Button closeButton;

    [Header("操作说明内容")]
    [Tooltip("操作说明文本内容（普通Text组件，如果使用TextMeshPro请留空）")]
    [SerializeField] private Text controlsText;

    [Tooltip("操作说明文本内容（TextMeshPro组件，如果使用普通Text请留空）")]
    [SerializeField] private TextMeshProUGUI controlsTextTMP;

    [Tooltip("是否在Start时自动隐藏面板")]
    [SerializeField] private bool hideOnStart = true;

    private void Start()
    {
        // 初始化：隐藏操作说明面板
        if (controlsPanel != null && hideOnStart)
        {
            controlsPanel.SetActive(false);
        }

        // 绑定按钮事件
        if (openButton != null)
        {
            openButton.onClick.AddListener(OpenControls);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseControls);
        }
    }

    /// <summary>
    /// 打开操作说明面板
    /// </summary>
    public void OpenControls()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("[ControlsGuideManager] 操作说明面板未分配！", this);
        }
    }

    /// <summary>
    /// 关闭操作说明面板
    /// </summary>
    public void CloseControls()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(false);
        }
    }

    /// <summary>
    /// 切换操作说明面板的显示状态
    /// </summary>
    public void ToggleControls()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(!controlsPanel.activeSelf);
        }
    }

}

