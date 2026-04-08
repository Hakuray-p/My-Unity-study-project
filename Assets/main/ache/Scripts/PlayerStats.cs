using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int MaxHP
    {
        get { return PlayerDataManager.Instance != null ? PlayerDataManager.Instance.MaxHP_Int : 100; }
        set { if (PlayerDataManager.Instance != null) PlayerDataManager.Instance.maxHp = value; }
    }

    public int Attack
    {
        get { return PlayerDataManager.Instance != null ? PlayerDataManager.Instance.Attack_Int : 10; }
        set { if (PlayerDataManager.Instance != null) PlayerDataManager.Instance.attackPower = value; }
    }

    // 显示数值到 UI
    public void PrintStats()
    {
        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.PrintStats();
        }
        else
        {
            Debug.Log($"PlayerStats - HP: {MaxHP}, ATK: {Attack}");
        }
    }

    // 喝药水功能，修改为使用统一数据管理器
    public void DrinkPotion(Item potion)
    {
        if (potion == null || PlayerDataManager.Instance == null) return;

        string name = potion.itemName.ToLower();

        if (name.Contains("life") || name.Contains("生命药水"))
        {
            float add = 20f;
            PlayerDataManager.Instance.AddMaxHp(add);
            Debug.Log($"{potion.itemName}制作成功，最大生命值 +{add}，当前 maxHP = {PlayerDataManager.Instance.maxHp}");
        }
        else if (name.Contains("attack") || name.Contains("攻击药水"))
        {
            float add = 5f;
            PlayerDataManager.Instance.AddAttackPower(add);
            Debug.Log($"{potion.itemName}制作成功，攻击力 +{add}，当前 attack = {PlayerDataManager.Instance.attackPower}");
        }
        else
        {
            Debug.Log("未知药水类型: " + potion.itemName);
        }

        // 可选在此处刷新 UI 显示
        PrintStats();
    }
}