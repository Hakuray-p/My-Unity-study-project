using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// 音量控制脚本：用于调节主音量
/// </summary>
public class VolumeControl : MonoBehaviour
{
    [Header("音频设置")]
    [Tooltip("拖入 MainMixer.mixer 文件")]
    [SerializeField] private AudioMixer mainMixer;
    
    [Tooltip("拖入音量滑块 UI")]
    [SerializeField] private Slider volumeSlider;

    [Header("音量参数名称")]
    [Tooltip("AudioMixer 中音量参数的名称（默认：MasterVol）")]
    [SerializeField] private string volumeParameterName = "MasterVol";

    private void Start()
    {
        InitializeVolume();
    }

    /// <summary>
    /// 初始化音量设置
    /// </summary>
    private void InitializeVolume()
    {
        if (volumeSlider == null)
        {
            Debug.LogWarning("[VolumeControl] 音量滑块未分配！", this);
            return;
        }

        if (mainMixer == null)
        {
            Debug.LogWarning("[VolumeControl] AudioMixer 未分配！", this);
            return;
        }

        // 读取保存的音量值，默认 0.75f (75%)
        float savedVol = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        
        // 设置滑块值
        volumeSlider.value = savedVol;
        
        // 应用音量
        SetVolume(savedVol);

        // 绑定滑块事件（当用户拖动滑块时自动调用）
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    /// <summary>
    /// 设置音量（由滑块调用）
    /// </summary>
    /// <param name="sliderValue">滑块值（0-1）</param>
    public void SetVolume(float sliderValue)
    {
        if (mainMixer == null)
        {
            Debug.LogError("[VolumeControl] AudioMixer 未分配，无法设置音量！", this);
            return;
        }

        // 将线性值转换为对数 dB 值
        // 公式：dB = log10(value) * 20
        // 当 value = 0 时，使用 0.0001f 避免 log(0) 错误
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20;

        // 设置 AudioMixer 中的音量参数
        bool success = mainMixer.SetFloat(volumeParameterName, dB);
        
        if (!success)
        {
            Debug.LogError($"[VolumeControl] 无法设置音量参数 '{volumeParameterName}'！请检查：\n" +
                          $"1. AudioMixer中是否存在该参数\n" +
                          $"2. 参数名是否正确（当前：{volumeParameterName}）\n" +
                          $"3. 所有AudioSource是否已连接到AudioMixer", this);
        }
        else
        {
            // 验证设置是否成功
            float currentValue;
            if (mainMixer.GetFloat(volumeParameterName, out currentValue))
            {
                Debug.Log($"[VolumeControl] 音量已设置为: {sliderValue * 100:F0}% (dB: {currentValue:F2})", this);
            }
        }

        // 保存设置
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 获取当前音量值（0-1）
    /// </summary>
    public float GetVolume()
    {
        return PlayerPrefs.GetFloat("MasterVolume", 0.75f);
    }
}