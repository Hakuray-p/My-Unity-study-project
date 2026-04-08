using UnityEngine;
using System.Collections;

public class InventoryUIAnimation : MonoBehaviour
{
    // *** 库存UI动画控制器 ***
    public InventoryManager inventoryManager;
    public GameObject inventoryPanelObject; // 包含 InventoryPanel 的 GameObject

    // private 变量，动画相关
    private CanvasGroup canvasGroup;
    private RectTransform rect;

    public float animationDuration = 0.25f;
    public Vector3 hiddenScale = new Vector3(0.8f, 0.8f, 1);
    public Vector3 shownScale = Vector3.one;

    private bool isOpen = false;

    void Awake()
    {
        // *** 步骤1：安全检查与组件获取 ***
        if (inventoryPanelObject == null)
        {
            Debug.LogError("InventoryUIAnimation: inventoryPanelObject 未分配！");
            return;
        }

        canvasGroup = inventoryPanelObject.GetComponent<CanvasGroup>();
        rect = inventoryPanelObject.GetComponent<RectTransform>();

        if (canvasGroup == null)
        {
            Debug.LogError("InventoryUIAnimation: 找不到 CanvasGroup 组件！");
            return;
        }

        // 初始状态
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        rect.localScale = hiddenScale;

        // 确保 UI GameObject 处于激活状态，方便 GetComponent 调用
        inventoryPanelObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        // 根据当前状态播放动画
        if (isOpen)
            StartCoroutine(CloseAnim());
        else
            StartCoroutine(OpenAnim());

        isOpen = !isOpen;
    }

    IEnumerator OpenAnim()
    {
        // 安全检查
        if (canvasGroup == null) yield break;

        canvasGroup.blocksRaycasts = true;

        float t = 0;
        while (t < animationDuration)
        {
            t += Time.deltaTime;
            float p = t / animationDuration;

            canvasGroup.alpha = Mathf.Lerp(0, 1, p);
            rect.localScale = Vector3.Lerp(hiddenScale, shownScale, p);

            yield return null;
        }

        canvasGroup.alpha = 1;
        rect.localScale = shownScale;

        // *** 步骤2：刷新库存 UI ***
        if (inventoryManager != null)
        {
            inventoryManager.RefreshUI();
        }

        yield break; // 明确结束协程
    }

    IEnumerator CloseAnim()
    {
        // 安全检查
        if (canvasGroup == null) yield break;

        canvasGroup.blocksRaycasts = false;

        float t = 0;
        while (t < animationDuration)
        {
            t += Time.deltaTime;
            float p = t / animationDuration;

            canvasGroup.alpha = Mathf.Lerp(1, 0, p);
            rect.localScale = Vector3.Lerp(shownScale, hiddenScale, p);

            yield return null;
        }

        canvasGroup.alpha = 0;
        rect.localScale = hiddenScale;

        yield break; // 明确结束协程
    }
}