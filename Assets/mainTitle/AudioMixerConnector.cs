using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// AudioMixer连接器：自动将所有AudioSource连接到AudioMixer
/// 这个脚本可以帮助修复音量控制不生效的问题
/// </summary>
public class AudioMixerConnector : MonoBehaviour
{
    [Header("AudioMixer设置")]
    [Tooltip("拖入 MainMixer.mixer 文件")]
    [SerializeField] private AudioMixer mainMixer;

    [Tooltip("AudioMixer组（通常是Master组）")]
    [SerializeField] private AudioMixerGroup masterGroup;

    [Header("自动连接设置")]
    [Tooltip("是否在Start时自动连接所有AudioSource")]
    [SerializeField] private bool autoConnectOnStart = true;

    [Tooltip("是否只连接场景中的AudioSource（不包括DontDestroyOnLoad的）")]
    [SerializeField] private bool onlyConnectInScene = false;

    private void Start()
    {
        if (autoConnectOnStart)
        {
            ConnectAllAudioSources();
        }
    }

    /// <summary>
    /// 连接所有AudioSource到AudioMixer
    /// </summary>
    [ContextMenu("连接所有AudioSource")]
    public void ConnectAllAudioSources()
    {
        if (mainMixer == null)
        {
            Debug.LogError("[AudioMixerConnector] AudioMixer未分配！", this);
            return;
        }

        // 如果没有手动指定Master组，尝试从AudioMixer中获取
        if (masterGroup == null)
        {
            AudioMixerGroup[] groups = mainMixer.FindMatchingGroups("Master");
            if (groups != null && groups.Length > 0)
            {
                masterGroup = groups[0];
                Debug.Log($"[AudioMixerConnector] 自动找到Master组: {masterGroup.name}", this);
            }
            else
            {
                Debug.LogError("[AudioMixerConnector] 无法找到Master组！请手动指定。", this);
                return;
            }
        }

        // 查找所有AudioSource
        AudioSource[] allAudioSources;
        if (onlyConnectInScene)
        {
            allAudioSources = FindObjectsOfType<AudioSource>();
        }
        else
        {
            allAudioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
        }

        int connectedCount = 0;
        int alreadyConnectedCount = 0;

        foreach (AudioSource audioSource in allAudioSources)
        {
            // 跳过未激活的
            if (!audioSource.gameObject.activeInHierarchy && onlyConnectInScene)
            {
                continue;
            }

            // 如果已经连接到AudioMixer，跳过
            if (audioSource.outputAudioMixerGroup != null)
            {
                alreadyConnectedCount++;
                continue;
            }

            // 连接到Master组
            audioSource.outputAudioMixerGroup = masterGroup;
            connectedCount++;

            Debug.Log($"[AudioMixerConnector] 已连接: {audioSource.gameObject.name} -> {masterGroup.name}", this);
        }

        Debug.Log($"[AudioMixerConnector] 完成！新连接: {connectedCount} 个，已连接: {alreadyConnectedCount} 个", this);
    }

    /// <summary>
    /// 手动连接指定的AudioSource
    /// </summary>
    public void ConnectAudioSource(AudioSource audioSource)
    {
        if (audioSource == null)
        {
            Debug.LogWarning("[AudioMixerConnector] AudioSource为空！", this);
            return;
        }

        if (masterGroup == null)
        {
            Debug.LogError("[AudioMixerConnector] Master组未分配！", this);
            return;
        }

        audioSource.outputAudioMixerGroup = masterGroup;
        Debug.Log($"[AudioMixerConnector] 已连接: {audioSource.gameObject.name} -> {masterGroup.name}", this);
    }
}

