using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/// <summary>
/// 启动动画控制器：播放一次视频，支持任意键跳过，结束后切换到主菜单。
/// </summary>
public class StartupVideoController : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Tooltip("视频播放结束后加载的场景")]
    [SerializeField] private string nextSceneName = "main menu";

    [Header("交互")]
    [Tooltip("最短观看时间（秒），避免刚进场景就被误触跳过）")]
    [SerializeField] private float minWatchTime = 1.5f;

    [Tooltip("是否允许按任意键跳过")]
    [SerializeField] private bool allowSkip = true;

    private bool _transitionTriggered;
    private float _enterTime;

    private void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
    }

    private void OnEnable()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("[StartupVideoController] 缺少 VideoPlayer 引用，无法播放启动动画。", this);
            return;
        }

        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += HandleVideoFinished;
        videoPlayer.Play();
        _enterTime = Time.time;
    }

    private void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= HandleVideoFinished;
        }
    }

    private void Update()
    {
        if (!allowSkip || _transitionTriggered)
        {
            return;
        }

        if (Time.time - _enterTime < minWatchTime)
        {
            return;
        }

        if (Input.anyKeyDown)
        {
            SkipVideo();
        }
    }

    private void HandleVideoFinished(VideoPlayer _)
    {
        if (_transitionTriggered)
        {
            return;
        }

        _transitionTriggered = true;
        LoadNextScene();
    }

    private void SkipVideo()
    {
        if (_transitionTriggered)
        {
            return;
        }

        _transitionTriggered = true;
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("[StartupVideoController] 未配置下一场景名称。");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}


