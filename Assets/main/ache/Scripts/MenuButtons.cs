using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 菜单按钮场景切换脚本
/// 包含两个按钮功能：返回主菜单和进入游戏
/// </summary>
public class MenuButtons : MonoBehaviour
{
    [Header("场景设置")]
    [Tooltip("主菜单场景名称")]
    public string mainMenuSceneName = "main menu";

    [Tooltip("游戏场景名称")]
    public string gameSceneName = "Game";

    [Tooltip("是否显示调试信息")]
    public bool showDebugLog = true;

    /// <summary>
    /// 按钮1：返回主菜单
    /// 在Unity编辑器中，将这个方法拖到按钮的OnClick事件中
    /// </summary>
    public void ReturnToMainMenu()
    {
        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            if (showDebugLog)
            {
                Debug.LogError("MenuButtons: 主菜单场景名称为空！请在Inspector中设置。");
            }
            return;
        }

        if (showDebugLog)
        {
            Debug.Log($"MenuButtons: 正在切换到主菜单场景: {mainMenuSceneName}");
        }

        // 恢复时间缩放（如果之前暂停了游戏）
        Time.timeScale = 1f;

        // 切换场景
        SceneManager.LoadScene(mainMenuSceneName);
    }

    /// <summary>
    /// 按钮2：进入游戏场景
    /// 在Unity编辑器中，将这个方法拖到按钮的OnClick事件中
    /// </summary>
    public void StartGame()
    {
        if (string.IsNullOrEmpty(gameSceneName))
        {
            if (showDebugLog)
            {
                Debug.LogError("MenuButtons: 游戏场景名称为空！请在Inspector中设置。");
            }
            return;
        }

        if (showDebugLog)
        {
            Debug.Log($"MenuButtons: 正在切换到游戏场景: {gameSceneName}");
        }

        // 恢复时间缩放（如果之前暂停了游戏）
        Time.timeScale = 1f;

        // 切换场景
        SceneManager.LoadScene(gameSceneName);
    }
}