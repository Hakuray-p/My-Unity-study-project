using UnityEngine;
using System.Collections;

public class AlchemyFurnace : MonoBehaviour
{
    [Header("材料槽（从项目中分配物品）")]
    public Item slot1;
    public Item slot2;

    [Header("引用")]
    public RecipeDatabase recipeDB;

    [Header("运行时状态")]
    public bool isCooking = false;
    public float progress = 0f;
    private Recipe currentRecipe;

    // 事件/回调（简单实现）
    public System.Action<Item> OnCookFinished; // 返回 result Item

    public void StartCook()
    {
        if (isCooking)
        {
            Debug.Log("正在炼制中");
            return;
        }

        if (slot1 == null || slot2 == null)
        {
            // 提示需要两个材料
            Debug.Log("材料不足 (需要同时拖入两个材料)");
            return;
        }

        currentRecipe = recipeDB.FindMatch(slot1, slot2);

        if (currentRecipe == null)
        {
            Debug.Log("配方不匹配: " + slot1.itemName + " + " + slot2.itemName);

            // 清空槽位，等待玩家重新投料
            slot1 = null;
            slot2 = null;

            // 如果你有 UI 文本引用，可以在这里调用更新方法
            // infoText.text = "配方不匹配，请重新放材料";

            return;
        }

        // *** 核心修复：在此处检查并消耗材料，并进行回滚检查 ***

        // 1. 尝试消耗第一个材料
        if (!InventoryManager.Instance.TrySpend(slot1))
        {
            Debug.Log("材料不足: " + slot1.itemName + " (炼制失败)");
            return;
        }

        // 2. 尝试消耗第二个材料
        if (!InventoryManager.Instance.TrySpend(slot2))
        {
            // 关键回滚：如果第二个材料不足，第一个材料必须退还！
            InventoryManager.Instance.Add(slot1);
            Debug.Log("材料不足: " + slot2.itemName + " (炼制失败，材料已退回)");
            return;
        }

        // 两个材料都成功消耗并从背包中扣除，开始炼制
        StartCoroutine(CookCoroutine(currentRecipe));
    }

    IEnumerator CookCoroutine(Recipe recipe)
    {
        isCooking = true;
        progress = 0f;
        Debug.Log("开始炼制: " + recipe.result.itemName);

        while (progress < recipe.cookTime)
        {
            progress += Time.deltaTime;
            yield return null;
        }

        // 炼制完成
        isCooking = false;
        progress = 0f;
        Item result = recipe.result;

        // 清空材料槽
        slot1 = null;
        slot2 = null;

        Debug.Log("炼制完成: " + result.itemName);

        // 回调
        OnCookFinished?.Invoke(result);
    }

    // 用于 UI 拖拽放入材料
    public void TryAddMaterial(Item item)
    {
        if (slot1 == null)
        {
            slot1 = item;
            Debug.Log("加入到 Slot1: " + item.itemName);
            return;
        }

        if (slot2 == null)
        {
            slot2 = item;
            Debug.Log("加入到 Slot2: " + item.itemName);
            return;
        }

        Debug.Log("两个槽位都满了");
    }
}