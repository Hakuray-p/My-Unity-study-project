using UnityEngine;

/// <summary>
/// 统一玩家数据管理器，负责生命值和攻击力。
/// 单例模式 + DontDestroyOnLoad 确保场景切换时数据不丢失。
/// </summary>
public class PlayerDataManager : MonoBehaviour
{
    // 单例实例
    public static PlayerDataManager Instance { get; private set; }

    [Header("当前数值")]
    [Tooltip("当前生命值")]
    public float hp = 100f;

    [Tooltip("最大生命值")]
    public float maxHp = 100f;

    [Tooltip("攻击力")]
    public float attackPower = 30f;

    [Header("初始数值设置")]
    [Tooltip("初始最大生命值")]
    public float initialMaxHp = 100f;

    [Tooltip("初始攻击力")]
    public float initialAttackPower = 30f;

    private void Awake()
    {
        // 单例模式：确保只有一个实例存在
        if (Instance == null)
        {
            Instance = this;
            // 关键逻辑：场景切换时不销毁此对象，保留数据
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 如果已存在实例，销毁重复项
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// 增加生命值（不超过上限）
    /// </summary>
    public void AddHp(float amount)
    {
        hp = Mathf.Min(hp + amount, maxHp);
    }

    /// <summary>
    /// 减少生命值（不低于0）
    /// </summary>
    public void ReduceHp(float amount)
    {
        hp = Mathf.Max(0, hp - amount);
    }

    /// <summary>
    /// 增加最大生命值
    /// </summary>
    public void AddMaxHp(float amount)
    {
        maxHp += amount;
        hp = Mathf.Min(hp, maxHp); // 确保当前HP不超过新的最大值
    }

    /// <summary>
    /// 增加攻击力
    /// </summary>
    public void AddAttackPower(float amount)
    {
        attackPower += amount;
    }

    /// <summary>
    /// 设置攻击力
    /// </summary>
    public void SetAttackPower(float value)
    {
        attackPower = value;
    }

    /// <summary>
    /// 获取HP百分比（0-1）
    /// </summary>
    public float GetHpPercentage()
    {
        if (maxHp <= 0) return 0;
        return hp / maxHp;
    }

    /// <summary>
    /// 打印当前数值（用于调试）
    /// </summary>
    public void PrintStats()
    {
        Debug.Log($"PlayerDataManager - HP: {hp}/{maxHp}, Attack: {attackPower}");
    }

    // 为了兼容PlayerStats的int类型，提供转换逻辑
    public int MaxHP_Int => Mathf.RoundToInt(maxHp);
    public int Attack_Int => Mathf.RoundToInt(attackPower);
}