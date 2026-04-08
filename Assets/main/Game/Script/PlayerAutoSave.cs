using System.Collections;
using UnityEngine;

/// <summary>
/// 挂在玩家身上的自动存档组件，会定时保存并在进入关卡时尝试应用读取到的数据。
/// </summary>
[RequireComponent(typeof(Player))]
public class PlayerAutoSave : MonoBehaviour
{
    [Tooltip("自动存档的时间间隔(秒)")]
    [SerializeField] private float autosaveInterval = 60f;

    [Tooltip("切换场景/销毁时是否立即再存一次")]
    [SerializeField] private bool saveOnDisable = true;

    [Tooltip("应用读取数据后是否立刻保存一次，避免重复加载")]
    [SerializeField] private bool resaveAfterApply = true;

    private Player _player;
    private Coroutine _autosaveRoutine;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        TryApplyPendingData();
        _autosaveRoutine = StartCoroutine(AutosaveLoop());
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveNow();
        }
    }

    private void OnApplicationQuit()
    {
        SaveNow();
    }

    private void OnDisable()
    {
        if (saveOnDisable)
        {
            SaveNow();
        }

        if (_autosaveRoutine != null)
        {
            StopCoroutine(_autosaveRoutine);
            _autosaveRoutine = null;
        }
    }

    /// <summary>
    /// 立刻执行一次存档。
    /// </summary>
    public void SaveNow()
    {
        if (_player == null)
        {
            _player = GetComponent<Player>();
        }

        PlayerSaveSystem.SavePlayer(_player);
    }

    private IEnumerator AutosaveLoop()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(autosaveInterval);
            SaveNow();
        }
    }

    private void TryApplyPendingData()
    {
        if (PlayerSaveSystem.TryConsumePendingData(out var data))
        {
            ApplyData(data);
            if (resaveAfterApply)
            {
                SaveNow();
            }
            return;
        }

        // 如果没有从主菜单触发的待应用数据，但场景内存在旧存档，也可以主动加载。
        if (PlayerSaveSystem.TryGetSavedData(out var fallbackData) &&
            fallbackData != null &&
            fallbackData.sceneName == "Game")
        {
            ApplyData(fallbackData);
        }
    }

    private void ApplyData(PlayerSaveData data)
    {
        transform.position = data.position;
        transform.rotation = Quaternion.Euler(data.rotationEuler);

        var pdm = PlayerDataManager.Instance;
        if (pdm != null)
        {
            pdm.maxHp = data.maxHp;
            pdm.hp = Mathf.Clamp(data.hp, 0, data.maxHp);
            pdm.attackPower = data.attackPower;
        }
        else
        {
            _player.MaxHp = data.maxHp;
            _player.Hp = Mathf.Clamp(data.hp, 0, data.maxHp);
            _player.AttackPower = data.attackPower;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHealth();
        }
    }
}



