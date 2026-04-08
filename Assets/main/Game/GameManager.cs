using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject GameOverPanel;
    public bool isFail = false;
    public GameObject goodEndingPanel;   // 好结局图片面板
    public GameObject badEndingPanel;    // 坏结局图片面板
    public string mainMenuSceneName = "main menu"; // 主菜单场景名（需在 Build Settings 中存在）
    public float endingDelay = 10f;      // 停留展示时间（秒）
    private bool endingTriggered = false;
    //玩家游戏信息
    public Slider _healthSlider;

    private void Awake()
    {
        Instance = this;
        GameOverPanel = GameObject.Find("GameOverPanel");

        // 初始化血条UI
        if (_healthSlider == null)
        {
            GameObject sliderObj = GameObject.Find("HealthSlider");
            if (sliderObj != null)
            {
                _healthSlider = sliderObj.GetComponent<Slider>();
            }
        }

        //初始化ui 
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        // 添加空检查，优先使用PlayerDataManager
        if (_healthSlider != null)
        {
            if (PlayerDataManager.Instance != null)
            {
                // 使用统一数据管理器
                float hpPercentage = PlayerDataManager.Instance.GetHpPercentage();
                _healthSlider.value = hpPercentage;
            }
            else if (Player.Instance != null)
            {
                // 备用方案：使用Player实例
                _healthSlider.value = Player.Instance.Hp / Player.Instance.MaxHp;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    //游戏失败
    public void GameOver()
    {
        if (GameOverPanel != null)
        {
            CanvasGroup cg = GameOverPanel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1;
                cg.blocksRaycasts = true;
                cg.interactable = true;
            }
        }

        isFail = true;

        StartCoroutine(WaitGoMainMenu());
    }
    public void TryCheckEnding()
    {
        if (endingTriggered) return;
        if (Spawner.Instance != null && Spawner.Instance.remainEnemies > 0) return;

        var data = PlayerDataManager.Instance;
        if (data == null) return;

        bool good = data.maxHp < 500f && data.attackPower < 100f;
        bool bad = data.maxHp > 500f && data.attackPower > 100f;

        if (good) ShowEnding(goodEndingPanel);
        else if (bad) ShowEnding(badEndingPanel);
        // 不满足两者则不触发（可按需要补默认结局）
    }

    // 展示结局并安排返回主菜单
    private void ShowEnding(GameObject panel)
    {
        endingTriggered = true;

        // 先清空存档
        PlayerSaveSystem.DeleteSave();

        // 显示图片面板
        if (panel != null)
        {
            var cg = panel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1;
                cg.blocksRaycasts = true;
                cg.interactable = true;
            }
            else
            {
                panel.SetActive(true);
            }
        }

        // 确保时间正常流逝
        Time.timeScale = 1f;

        // 10 秒后回主菜单
        StartCoroutine(ReturnToMenuAfterDelay());
    }

    private IEnumerator ReturnToMenuAfterDelay()
    {
        float t = 0f;
        while (t < endingDelay)
        {
            t += Time.deltaTime;
            yield return null;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
    }
    //回主菜单
    public void GoMainMenu()
    {
        SceneManager.LoadScene("Main menu");
    }

    //退出游戏
    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator WaitGoMainMenu()
    {
        float timer = 0;
        while (timer < 4)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        GoMainMenu();
    }
}