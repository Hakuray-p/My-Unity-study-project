using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// 设置场景背景视频控制器：在SettingsMenu场景中循环播放背景视频
/// </summary>
public class SettingsVideoBackground : MonoBehaviour
{
    [Header("视频设置")]
    [Tooltip("视频播放器组件（会自动查找，也可以手动拖入）")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Tooltip("背景视频文件（MP4）")]
    [SerializeField] private VideoClip backgroundVideoClip;

    [Tooltip("视频文件路径（如果使用URL方式）")]
    [SerializeField] private string videoPath = "";

    [Header("渲染设置")]
    [Tooltip("渲染目标纹理（用于在UI上显示视频）")]
    [SerializeField] private RenderTexture renderTexture;

    [Tooltip("目标摄像机（如果要在摄像机上显示）")]
    [SerializeField] private Camera targetCamera;

    [Header("UI显示设置")]
    [Tooltip("用于显示视频的RawImage（会自动查找）")]
    [SerializeField] private UnityEngine.UI.RawImage videoRawImage;

    [Tooltip("是否自动调整视频大小以填满屏幕")]
    [SerializeField] private bool autoResizeToFillScreen = true;

    private void Awake()
    {
        // 自动查找 VideoPlayer 组件
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
            if (videoPlayer == null)
            {
                // 如果没有找到，创建一个新的 VideoPlayer
                videoPlayer = gameObject.AddComponent<VideoPlayer>();
            }
        }
    }

    private void Start()
    {
        SetupVideoPlayer();
    }

    /// <summary>
    /// 设置视频播放器
    /// </summary>
    private void SetupVideoPlayer()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("[SettingsVideoBackground] VideoPlayer 组件未找到！", this);
            return;
        }

        // 设置视频源
        if (backgroundVideoClip != null)
        {
            videoPlayer.clip = backgroundVideoClip;
            videoPlayer.source = VideoSource.VideoClip;
        }
        else if (!string.IsNullOrEmpty(videoPath))
        {
            videoPlayer.url = videoPath;
            videoPlayer.source = VideoSource.Url;
        }
        else
        {
            Debug.LogWarning("[SettingsVideoBackground] 未指定视频文件！请在Inspector中设置 Background Video Clip 或 Video Path。", this);
            return;
        }

        // 优先使用RenderTexture模式（推荐，不会遮挡UI）
        if (renderTexture != null)
        {
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = renderTexture;
            
            // 设置视频宽高比
            if (backgroundVideoClip != null)
            {
                videoPlayer.aspectRatio = VideoAspectRatio.FitVertically; // 或 FitHorizontally, NoScaling
            }
            
            // 自动调整RawImage大小
            if (autoResizeToFillScreen)
            {
                SetupVideoRawImage();
            }
            
            Debug.Log("[SettingsVideoBackground] 使用 RenderTexture 模式渲染视频。", this);
        }
        else if (targetCamera != null)
        {
            // 使用摄像机模式时，设置Alpha值，确保不遮挡UI
            videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
            videoPlayer.targetCamera = targetCamera;
            videoPlayer.targetCameraAlpha = 1.0f; // 确保视频不透明
            
            // 调整摄像机的Clear Flags，确保UI能正常显示
            if (targetCamera != null)
            {
                // 如果使用Camera模式，建议使用Solid Color或Skybox，不要使用Depth
                if (targetCamera.clearFlags == CameraClearFlags.Depth)
                {
                    targetCamera.clearFlags = CameraClearFlags.SolidColor;
                    Debug.LogWarning("[SettingsVideoBackground] 检测到摄像机使用Depth清除模式，已自动改为SolidColor以确保UI正常显示。", this);
                }
            }
            
            Debug.Log("[SettingsVideoBackground] 使用 Camera Far Plane 模式渲染视频。", this);
        }
        else
        {
            // 默认使用摄像机渲染
            if (Camera.main != null)
            {
                videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
                videoPlayer.targetCamera = Camera.main;
                videoPlayer.targetCameraAlpha = 1.0f;
                
                // 调整主摄像机的Clear Flags
                if (Camera.main.clearFlags == CameraClearFlags.Depth)
                {
                    Camera.main.clearFlags = CameraClearFlags.SolidColor;
                    Debug.LogWarning("[SettingsVideoBackground] 检测到主摄像机使用Depth清除模式，已自动改为SolidColor以确保UI正常显示。", this);
                }
                
                Debug.Log("[SettingsVideoBackground] 使用主摄像机渲染视频（Camera Far Plane 模式）。", this);
            }
            else
            {
                Debug.LogWarning("[SettingsVideoBackground] 未找到主摄像机，且未指定渲染目标！建议使用RenderTexture模式。", this);
            }
        }

        // 设置视频播放属性
        videoPlayer.isLooping = true;  // 循环播放
        videoPlayer.playOnAwake = true; // 自动播放
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.skipOnDrop = true;

        // 开始播放
        videoPlayer.Play();

        Debug.Log("[SettingsVideoBackground] 背景视频已开始播放。", this);
    }

    /// <summary>
    /// 手动播放视频（如果需要）
    /// </summary>
    public void PlayVideo()
    {
        if (videoPlayer != null && !videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }

    /// <summary>
    /// 暂停视频（如果需要）
    /// </summary>
    public void PauseVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }

    /// <summary>
    /// 停止视频（如果需要）
    /// </summary>
    public void StopVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
    }

    /// <summary>
    /// 设置视频RawImage，确保填满屏幕
    /// </summary>
    private void SetupVideoRawImage()
    {
        // 自动查找RawImage
        if (videoRawImage == null)
        {
            videoRawImage = FindObjectOfType<UnityEngine.UI.RawImage>();
            if (videoRawImage == null)
            {
                Debug.LogWarning("[SettingsVideoBackground] 未找到RawImage组件！视频可能无法正确显示。", this);
                return;
            }
        }

        // 获取RectTransform
        RectTransform rectTransform = videoRawImage.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogWarning("[SettingsVideoBackground] RawImage没有RectTransform组件！", this);
            return;
        }

        // 设置锚点：左下角到右上角（填满整个屏幕）
        rectTransform.anchorMin = Vector2.zero;  // (0, 0) 左下角
        rectTransform.anchorMax = Vector2.one;  // (1, 1) 右上角
        
        // 设置偏移为0，这样就会填满整个父容器
        rectTransform.offsetMin = Vector2.zero;  // Left, Bottom
        rectTransform.offsetMax = Vector2.zero;   // Right, Top
        
        // 确保RawImage在Canvas的最底层
        rectTransform.SetAsFirstSibling();

        // 设置RawImage的纹理（如果还没有设置）
        if (videoRawImage.texture == null && renderTexture != null)
        {
            videoRawImage.texture = renderTexture;
        }

        Debug.Log("[SettingsVideoBackground] 视频RawImage已设置为填满屏幕。", this);
    }

    private void OnDestroy()
    {
        // 清理资源
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
    }
}

