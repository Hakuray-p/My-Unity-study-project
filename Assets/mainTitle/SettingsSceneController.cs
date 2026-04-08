using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 设置场景控制器：用于 SettingsMenu 场景
/// 管理音量控制和返回主菜单功能
/// </summary>
public class SettingsSceneController : MonoBehaviour
{
    [Header("场景设置")]
    [Tooltip("主菜单场景名称")]
    [SerializeField] private string mainMenuSceneName = "main menu";

    [Header("UI 引用")]
    [Tooltip("返回主菜单按钮")]
    [SerializeField] private Button backButton;

    [Header("音量控制")]
    [Tooltip("音量控制脚本（会自动查找，也可以手动拖入）")]
    [SerializeField] private VolumeControl volumeControl;

    private void Start()
    {
        // 自动查找 VolumeControl 组件（如果未手动分配）
        if (volumeControl == null)
        {
            volumeControl = FindObjectOfType<VolumeControl>();
            if (volumeControl == null)
            {
                Debug.LogWarning("[SettingsSceneController] 未找到 VolumeControl 组件！请确保场景中有音量控制脚本。", this);
            }
        }

        // 绑定返回按钮事件
        if (backButton != null)
        {
            backButton.onClick.AddListener(ReturnToMainMenu);
        }
        else
        {
            Debug.LogWarning("[SettingsSceneController] 返回按钮未分配！", this);
        }
    }

    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void ReturnToMainMenu()
    {
        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.LogError("[SettingsSceneController] 主菜单场景名称为空！请在Inspector中设置。", this);
            return;
        }

        Debug.Log($"[SettingsSceneController] 正在返回主菜单: {mainMenuSceneName}");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}

