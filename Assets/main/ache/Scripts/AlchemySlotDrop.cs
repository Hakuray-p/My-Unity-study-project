// AlchemySlotDrop.cs (拖拽材料到炼药槽)

using UnityEngine;
using UnityEngine.EventSystems;

public class AlchemySlotDrop : MonoBehaviour, IDropHandler
{
    // 在Inspector中分配AlchemyFurnace
    public AlchemyFurnace furnace;

    public void OnDrop(PointerEventData eventData)
    {
        // 1. 获取拖拽的物品
        var drag = eventData.pointerDrag.GetComponent<ItemDragHandler>();
        if (drag == null) return;

        // 2. 获取物品数据
        Item item = drag.itemData;

        // 3. 添加到炼药炉（炼药炉会检查材料是否有效）
        furnace.TryAddMaterial(item);

        // 调试信息
        Debug.Log("添加材料: " + item.itemName + " 到炼药炉");
    }
}