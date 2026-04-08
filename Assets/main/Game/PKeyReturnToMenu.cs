using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// P键返回主菜单功能
/// 独立脚本，可以在任意场景中使用
/// </summary>
public class PKeyReturnToMenu : MonoBehaviour
{
    [Header("主菜单设置")]
    [Tooltip("主菜单场景名称（必须在Build Settings中）")]
    public string mainMenuSceneName = "main menu";

    [Tooltip("是否启用P键返回主菜单功能")]
    public bool enablePKeyReturn = true;

    [Tooltip("是否显示调试信息")]
    public bool showDebugLog = true;

    // Update is called once per frame
    void Update()
    {
        // 检查是否启用功能
        if (!enablePKeyReturn)
        {
            return;
        }

        // 检测P键按下
        if (Input.GetKeyDown(KeyCode.P))
        {
            ReturnToMainMenu();
        }
    }

    /// <summary>
    /// 返回主菜单
    /// </summary>
    private void ReturnToMainMenu()
    {
        // 检查场景名称是否有效
        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            if (showDebugLog)
            {
                Debug.LogWarning("PKeyReturnToMenu: 主菜单场景名称为空，无法切换场景！");
            }
            return;
        }

        if (showDebugLog)
        {
            Debug.Log($"PKeyReturnToMenu: 按P键返回主菜单，正在切换到场景: {mainMenuSceneName}");
        }

        // 恢复时间缩放（如果之前暂停了）
        Time.timeScale = 1f;

        // 切换场景
        SceneManager.LoadScene(mainMenuSceneName);
    }

    /// <summary>
    /// 公共方法：可以从其他脚本调用
    /// </summary>
    public void GoToMainMenu()
    {
        ReturnToMainMenu();
    }
}