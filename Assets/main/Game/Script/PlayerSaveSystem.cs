using System;
using UnityEngine;

/// <summary>
/// 负责序列化/反序列化玩家关键数据，并在场景之间转移保存的数据。
/// </summary>
public static class PlayerSaveSystem
{
    private const string SaveKey = "PLAYER_AUTO_SAVE_V1";

    private static bool _hasPendingData;
    private static PlayerSaveData _pendingData;

    /// <summary>
    /// 是否已经有存档。
    /// </summary>
    public static bool HasSave()
    {
        return PlayerPrefs.HasKey(SaveKey);
    }

    /// <summary>
    /// 将玩家当前状态写入存档。
    /// </summary>
    public static void SavePlayer(Player player)
    {
        if (player == null)
        {
            Debug.LogWarning("PlayerSaveSystem.SavePlayer: player == null");
            return;
        }

        var data = BuildSaveData(player);
        var json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
#if UNITY_EDITOR
        Debug.Log($"[PlayerSaveSystem] 已保存，时间戳: {data.savedAtTicks}");
#endif
    }

    /// <summary>
    /// 读取存档，不存在则返回 false。
    /// </summary>
    public static bool TryGetSavedData(out PlayerSaveData data)
    {
        if (!HasSave())
        {
            data = null;
            return false;
        }

        var json = PlayerPrefs.GetString(SaveKey);
        try
        {
            data = JsonUtility.FromJson<PlayerSaveData>(json);
            return data != null;
        }
        catch (Exception e)
        {
            Debug.LogError($"[PlayerSaveSystem] 解析存档失败: {e.Message}");
            data = null;
            return false;
        }
    }

    /// <summary>
    /// 将存档放入待应用队列，供下一个场景载入时使用。
    /// </summary>
    public static void QueueDataForNextScene(PlayerSaveData data)
    {
        if (data == null)
        {
            _hasPendingData = false;
            _pendingData = null;
            return;
        }

        _pendingData = data;
        _hasPendingData = true;
    }

    /// <summary>
    /// 取出待应用数据（仅能使用一次）。
    /// </summary>
    public static bool TryConsumePendingData(out PlayerSaveData data)
    {
        if (_hasPendingData && _pendingData != null)
        {
            data = _pendingData;
            _pendingData = null;
            _hasPendingData = false;
            return true;
        }

        data = null;
        return false;
    }

    /// <summary>
    /// 删除现有存档。
    /// </summary>
    public static void DeleteSave()
    {
        if (HasSave())
        {
            PlayerPrefs.DeleteKey(SaveKey);
        }
        _hasPendingData = false;
        _pendingData = null;
    }

    private static PlayerSaveData BuildSaveData(Player player)
    {
        var pdm = PlayerDataManager.Instance;
        var data = new PlayerSaveData
        {
            position = player.transform.position,
            rotationEuler = player.transform.rotation.eulerAngles,
            hp = pdm != null ? pdm.hp : player.Hp,
            maxHp = pdm != null ? pdm.maxHp : player.MaxHp,
            attackPower = pdm != null ? pdm.attackPower : player.AttackPower,
            sceneName = "Game",
            savedAtTicks = DateTime.UtcNow.Ticks
        };
        return data;
    }
}

/// <summary>
/// 玩家可序列化存档数据。
/// </summary>
[Serializable]
public class PlayerSaveData
{
    public Vector3 position;
    public Vector3 rotationEuler;
    public float hp;
    public float maxHp;
    public float attackPower;
    public string sceneName;
    public long savedAtTicks;
}



