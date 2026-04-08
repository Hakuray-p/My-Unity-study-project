using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// 设置场景视频设置助手：帮助快速设置视频背景（使用RenderTexture方式，确保UI正常显示）
/// 这个脚本可以添加到场景中，运行时会自动创建必要的组件
/// </summary>
public class SettingsVideoSetupHelper : MonoBehaviour
{
    [Header("自动设置（运行时）")]
    [Tooltip("是否在运行时自动设置视频背景")]
    [SerializeField] private bool autoSetupOnStart = true;

    [Header("视频设置")]
    [Tooltip("背景视频文件")]
    [SerializeField] private VideoClip backgroundVideoClip;

    [Header("UI设置")]
    [Tooltip("Canvas（会自动查找）")]
    [SerializeField] private Canvas targetCanvas;

    private void Start()
    {
        if (autoSetupOnStart)
        {
            SetupVideoBackground();
        }
    }

    /// <summary>
    /// 设置视频背景（使用RenderTexture方式）
    /// </summary>
    [ContextMenu("设置视频背景")]
    public void SetupVideoBackground()
    {
        if (backgroundVideoClip == null)
        {
            Debug.LogError("[SettingsVideoSetupHelper] 未指定视频文件！", this);
            return;
        }

        // 查找或创建Canvas
        if (targetCanvas == null)
        {
            targetCanvas = FindObjectOfType<Canvas>();
            if (targetCanvas == null)
            {
                Debug.LogError("[SettingsVideoSetupHelper] 未找到Canvas！请确保场景中有Canvas。", this);
                return;
            }
        }

        // 创建RenderTexture
        RenderTexture rt = new RenderTexture(1920, 1080, 0);
        rt.name = "SettingsVideoRT";

        // 创建VideoPlayer
        GameObject videoPlayerObj = new GameObject("BackgroundVideoPlayer");
        VideoPlayer videoPlayer = videoPlayerObj.AddComponent<VideoPlayer>();
        videoPlayer.clip = backgroundVideoClip;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = rt;
        videoPlayer.isLooping = true;
        videoPlayer.playOnAwake = true;

        // 创建RawImage显示视频
        GameObject rawImageObj = new GameObject("BackgroundVideoImage");
        rawImageObj.transform.SetParent(targetCanvas.transform, false);
        
        RectTransform rectTransform = rawImageObj.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;

        // 设置RawImage在最底层
        rawImageObj.transform.SetAsFirstSibling();

        RawImage rawImage = rawImageObj.AddComponent<RawImage>();
        rawImage.texture = rt;

        // 添加SettingsVideoBackground脚本
        SettingsVideoBackground videoController = videoPlayerObj.AddComponent<SettingsVideoBackground>();

        Debug.Log("[SettingsVideoSetupHelper] 视频背景已自动设置完成！", this);
    }
}

