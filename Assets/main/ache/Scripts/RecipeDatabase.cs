using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDB", menuName = "Alchemy/RecipeDatabase")]
public class RecipeDatabase : ScriptableObject
{
    public Recipe[] recipes;

    // 查找匹配的配方（不考虑顺序）
    public Recipe FindMatch(Item a, Item b)
    {
        if (a == null || b == null) return null;

        foreach (var r in recipes)
        {
            if (r == null) continue;
            bool m1 = (r.ingredientA == a && r.ingredientB == b);
            bool m2 = (r.ingredientA == b && r.ingredientB == a);
            if (m1 || m2) return r;
        }
        return null;
    }
}