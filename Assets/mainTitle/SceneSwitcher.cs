using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

 
    public void GoToAlchemistHome()
    {
        SceneManager.LoadScene("home");
    }


    public void QuitGame()
    {
       
        Application.Quit();

       
#if UNITY_EDITOR
        Debug.Log("Unity");
#endif
    }

    
    public void GoBackToGame()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// 跳转到设置场景
    /// </summary>
    public void GoToSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("main menu");
    }
}