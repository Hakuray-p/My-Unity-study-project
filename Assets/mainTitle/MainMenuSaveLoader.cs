using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 主菜单中用于读取存档并跳转到游戏场景的脚本。
/// </summary>
public class MainMenuSaveLoader : MonoBehaviour
{
    [SerializeField] private string defaultGameSceneName = "Game";
    [SerializeField] private Button continueButton;

    private void Start()
    {
        RefreshContinueButton();
    }

    /// <summary>
    /// 点击“继续游戏”按钮时调用。
    /// </summary>
    public void ContinueGame()
    {
        if (!PlayerSaveSystem.TryGetSavedData(out var data) || data == null)
        {
            Debug.LogWarning("[MainMenuSaveLoader] 当前没有可用存档。");
            RefreshContinueButton();
            return;
        }

        PlayerSaveSystem.QueueDataForNextScene(data);
        var targetScene = string.IsNullOrEmpty(data.sceneName) ? defaultGameSceneName : data.sceneName;
        SceneManager.LoadScene(targetScene);
    }

    /// <summary>
    /// 可选：在 UI 上提供“删除存档”按钮时调用。
    /// </summary>
    public void DeleteSave()
    {
        PlayerSaveSystem.DeleteSave();
        RefreshContinueButton();
    }

    /// <summary>
    /// 根据是否有存档自动决定继续按钮是否可用。
    /// </summary>
    public void RefreshContinueButton()
    {
        if (continueButton != null)
        {
            continueButton.interactable = PlayerSaveSystem.HasSave();
        }
    }
}



