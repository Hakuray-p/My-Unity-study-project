using UnityEngine;
using UnityEngine.UI;

public class AlchemyTesterUI : MonoBehaviour
{
    public AlchemyFurnace furnace;
    public PlayerStats player;
    public Button startButton;
    public Text infoText;

    void Start()
    {
        if (furnace == null || player == null)
            Debug.LogError("请检查 AlchemyFurnace 或 PlayerStats 是否已分配");

        if (startButton != null)
            startButton.onClick.AddListener(OnStartButton);

        furnace.OnCookFinished += OnCookFinished;
    }

    void OnStartButton()
    {
        furnace.StartCook();
        if (infoText != null) infoText.text = "开始炼制...";
    }

    void OnCookFinished(Item result)
    {
        if (infoText != null) infoText.text = "制作成功: " + result.itemName;

        player.DrinkPotion(result);
        player.PrintStats();
    }
}