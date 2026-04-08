using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 设置管理器：用于主菜单，点击设置按钮跳转到设置场景
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("场景设置")]
    [Tooltip("设置场景名称")]
    [SerializeField] private string settingsSceneName = "SettingsMenu";

    [Header("UI 引用")]
    [Tooltip("设置按钮（可选，如果为空则需要在Unity中手动绑定）")]
    [SerializeField] private Button settingsButton;

    private void Start()
    {
        // 如果按钮已分配，自动绑定事件
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OpenSettings);
        }
    }

    /// <summary>
    /// 打开设置场景（由设置按钮调用）
    /// </summary>
    public void OpenSettings()
    {
        if (string.IsNullOrEmpty(settingsSceneName))
        {
            Debug.LogError("[SettingsManager] 设置场景名称为空！请在Inspector中设置。", this);
            return;
        }

        Debug.Log($"[SettingsManager] 正在切换到设置场景: {settingsSceneName}");
        SceneManager.LoadScene(settingsSceneName);
    }
}

