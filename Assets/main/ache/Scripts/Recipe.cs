using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Alchemy/Recipe")]
public class Recipe : ScriptableObject
{
    public Item ingredientA;
    public Item ingredientB;
    public Item result;
    public float cookTime = 2f; // 炼制药水所需时间（秒）
}