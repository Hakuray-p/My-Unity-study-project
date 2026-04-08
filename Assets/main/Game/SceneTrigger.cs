using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // 需要导入，用于协程

/// <summary>
/// 场景触发器脚本（改进版）
/// 当玩家进入触发器区域后需要停留足够时间才会执行传送。
/// </summary>
public class SceneTrigger : MonoBehaviour
{
    [Header("场景切换设置")]
    [Tooltip("要切换到的目标场景名称")]
    public string targetSceneName = "home";

    [Tooltip("只有带有此标签的物体才能触发场景切换（留空则任何物体都能触发）")]
    public string triggerTag = "Player";

    [Tooltip("切换场景前的延迟时间（秒）")]
    public float delayTime = 5f; // 新增的触发延迟时间

    [Tooltip("是否显示调试信息")]
    public bool showDebugLog = true;

    // 私有变量
    private Coroutine switchCoroutine = null; // 用于存储当前运行的传送协程，以便取消

    // --------------------------- 3D 触发器事件 ---------------------------

    /// <summary>
    /// 3D触发器检测：当物体进入触发器时调用
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // 1. 检查标签是否符合触发条件
        if (!IsTargetTag(other.gameObject))
        {
            return;
        }

        // 2. 只有当没有正在运行的传送协程时，才启动新的协程
        if (switchCoroutine == null)
        {
            if (showDebugLog)
            {
                Debug.Log($"SceneTrigger: {other.name} 进入触发器区域，{delayTime} 秒倒计时开始...");
            }
            switchCoroutine = StartCoroutine(LoadSceneAfterDelay(delayTime));
        }
    }

    /// <summary>
    /// 3D触发器检测：当物体离开触发器时调用
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        // 1. 检查标签
        if (!IsTargetTag(other.gameObject))
        {
            return;
        }

        // 2. 如果当前有正在运行的协程，则取消它
        if (switchCoroutine != null)
        {
            StopCoroutine(switchCoroutine);
            switchCoroutine = null; // 重置引用

            if (showDebugLog)
            {
                Debug.Log($"SceneTrigger: {other.name} 离开触发器区域，传送倒计时已取消");
            }
        }
    }

    // --------------------------- 2D 触发器事件 ---------------------------

    /// <summary>
    /// 2D触发器检测：当物体进入触发器时调用
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 检查标签是否符合触发条件
        if (!IsTargetTag(other.gameObject))
        {
            return;
        }

        // 2. 只有当没有正在运行的传送协程时，才启动新的协程
        if (switchCoroutine == null)
        {
            if (showDebugLog)
            {
                Debug.Log($"SceneTrigger: {other.name} 进入触发器区域，{delayTime} 秒倒计时开始...");
            }
            switchCoroutine = StartCoroutine(LoadSceneAfterDelay(delayTime));
        }
    }

    /// <summary>
    /// 2D触发器检测：当物体离开触发器时调用
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        // 1. 检查标签
        if (!IsTargetTag(other.gameObject))
        {
            return;
        }

        // 2. 如果当前有正在运行的协程，则取消它
        if (switchCoroutine != null)
        {
            StopCoroutine(switchCoroutine);
            switchCoroutine = null; // 重置引用

            if (showDebugLog)
            {
                Debug.Log($"SceneTrigger: {other.name} 离开触发器区域，传送倒计时已取消");
            }
        }
    }

    // --------------------------- 核心逻辑方法 ---------------------------

    /// <summary>
    /// 检查游戏对象是否具有目标标签
    /// </summary>
    private bool IsTargetTag(GameObject obj)
    {
        if (string.IsNullOrEmpty(triggerTag))
        {
            // 如果未设置标签，任何物体都可以
            return true;
        }
        return obj.CompareTag(triggerTag);
    }

    /// <summary>
    /// 协程：等待指定的延迟时间后加载场景
    /// </summary>
    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        // 暂停执行，等待指定的时间
        yield return new WaitForSeconds(delay);

        // 检查目标场景名称是否有效
        if (string.IsNullOrEmpty(targetSceneName))
        {
            if (showDebugLog)
            {
                Debug.LogWarning("SceneTrigger: 目标场景名称为空，无法切换场景！");
            }
            switchCoroutine = null; // 确保协程引用被重置
            yield break; // 退出协程
        }

        // 延迟时间结束后执行传送
        if (showDebugLog)
        {
            Debug.Log($"SceneTrigger: 延迟时间结束，正在切换到场景: {targetSceneName}");
        }

        // 切换场景
        SceneManager.LoadScene(targetSceneName);

        // 注意：SceneManager.LoadScene 执行后当前场景会被卸载，无需手动设置 switchCoroutine = null
    }

    // --------------------------- 编辑器可视化（保持不变） ---------------------------

    /// <summary>
    /// 在编辑器中可视化触发器范围（仅在Scene视图中显示）
    /// </summary>
    private void OnDrawGizmos()
    {
        // 绘制触发器范围的轮廓（黄色半透明）
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);

        // 获取Collider组件
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            if (collider is BoxCollider)
            {
                BoxCollider boxCollider = collider as BoxCollider;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(boxCollider.center, boxCollider.size);
            }
            else if (collider is SphereCollider)
            {
                SphereCollider sphereCollider = collider as SphereCollider;
                Gizmos.DrawSphere(transform.position + sphereCollider.center, sphereCollider.radius);
            }
            else if (collider is CapsuleCollider)
            {
                CapsuleCollider capsuleCollider = collider as CapsuleCollider;
                Gizmos.DrawSphere(transform.position + capsuleCollider.center, capsuleCollider.radius);
            }
        }
    }
}