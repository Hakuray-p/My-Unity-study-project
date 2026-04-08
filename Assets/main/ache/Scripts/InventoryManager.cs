using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // 假设你的初始默认数量是 3
    private const int DEFAULT_START_COUNT = 3;

    void Awake()
    {
        // 允许场景切换时覆盖旧引用，防止把挂载同一物体的其它组件一起销毁
        Instance = this;
        LoadData(); // <--- 关键修改 1：场景加载时，从 PlayerPrefs 读取数据
    }

    [Header("Scriptable Objects")]
    public Item soulGrass;
    public Item monsterHeart;
    public Item monsterCore;

    [Header("Counts")]
    // 移除默认初始化，改由 LoadData() 设定
    public int soulGrassCount;
    public int monsterHeartCount;
    public int monsterCoreCount;

    [Header("UI")]
    public TextMeshProUGUI soulGrassText;
    public TextMeshProUGUI heartText;
    public TextMeshProUGUI coreText;

    //void Start()
    // {
    //  RefreshUI();
    // }

    // ==========================================
    // 核心修改 2：新增 GetItemCount (解决编译错误)
    // ==========================================
    // 供 ItemCountDisplay.cs 脚本查询特定物品的数量
    public int GetItemCount(Item item)
    {
        if (item == soulGrass) return soulGrassCount;
        if (item == monsterHeart) return monsterHeartCount;
        if (item == monsterCore) return monsterCoreCount;
        return 0;
    }

    // ==========================================
    // 核心修改 3：修改 RefreshUI
    // ==========================================
    public void RefreshUI()
    {
        // 旧的 UI 刷新逻辑 (兼容旧的 UI 字段，增加空检查防止 Home 场景报错)
        if (soulGrassText != null) soulGrassText.text = soulGrassCount.ToString();
        if (heartText != null) heartText.text = monsterHeartCount.ToString();
        if (coreText != null) coreText.text = monsterCoreCount.ToString();

        // 新增：通知所有 ItemCountDisplay 脚本更新 (用于跨场景解耦)
        ItemCountDisplay[] displays = FindObjectsOfType<ItemCountDisplay>();
        foreach (var display in displays)
        {
            display.UpdateDisplay();
        }
    }

    // ==========================================
    // 核心修改 4：新增 Add 材料 (用于打怪掉落)
    // ==========================================
    public void Add(Item item)
    {
        if (item == soulGrass) soulGrassCount++;
        else if (item == monsterHeart) monsterHeartCount++;
        else if (item == monsterCore) monsterCoreCount++;

        SaveData(); // 数据变化时，立即保存
        RefreshUI(); // 刷新 UI
    }

    public bool TrySpend(Item item)
    {
        bool success = false;
        if (item == soulGrass)
        {
            if (soulGrassCount > 0)
            {
                soulGrassCount--;
                success = true;
            }
        }
        else if (item == monsterHeart)
        {
            if (monsterHeartCount > 0)
            {
                monsterHeartCount--;
                success = true;
            }
        }
        else if (item == monsterCore)
        {
            if (monsterCoreCount > 0)
            {
                monsterCoreCount--;
                success = true;
            }
        }

        if (success)
        {
            SaveData(); // <--- 关键修改 5：数据变化时，立即保存
            RefreshUI();
            return true;
        }
        return false;
    }

    // ==========================================
    // 核心修改 6：存档与读档方法
    // ==========================================

    void SaveData()
    {
        PlayerPrefs.SetInt("SoulGrass_Count", soulGrassCount);
        PlayerPrefs.SetInt("MonsterHeart_Count", monsterHeartCount);
        PlayerPrefs.SetInt("MonsterCore_Count", monsterCoreCount);
        PlayerPrefs.Save(); // 强制写入磁盘
        Debug.Log("背包数据已保存！");
    }

    void LoadData()
    {
        // 首次运行时，如果找不到数据，会返回默认值 3
        soulGrassCount = PlayerPrefs.GetInt("SoulGrass_Count", DEFAULT_START_COUNT);
        monsterHeartCount = PlayerPrefs.GetInt("MonsterHeart_Count", DEFAULT_START_COUNT);
        monsterCoreCount = PlayerPrefs.GetInt("MonsterCore_Count", DEFAULT_START_COUNT);
        Debug.Log("背包数据已读取");
    }
}