using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField]
    private RecipeData[] availableRecipes;
    [SerializeField]
    private GameObject recipeUiPrefab;
    [SerializeField]
    private Transform recipesParent;
    [SerializeField]
    private KeyCode openCraftPanelInput;
    [SerializeField]
    private GameObject craftingPanel;

    private List<RecipeData> unlockedRecipes = new List<RecipeData>();
    void Start()
    {
        UpdateDisplayedRecipes();

    }

    private void Update()
    {
        if (Input.GetKeyDown(openCraftPanelInput))
        {
            craftingPanel.SetActive(!craftingPanel.activeSelf);
            UpdateDisplayedRecipes();
        }
    }

    public RecipeData[] GetAvailableRecipes()
    {
        return availableRecipes;
    }

    public void UnlockRecipe(RecipeData recipe)
    {
        if (!unlockedRecipes.Contains(recipe))
        {
            unlockedRecipes.Add(recipe);
            UpdateDisplayedRecipes();
        }
    }

    public bool IsRecipeUnlocked(RecipeData recipe)
    {
        return unlockedRecipes.Contains(recipe);
    }
    
    public void UpdateDisplayedRecipes()
    {
        foreach (Transform child in recipesParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipeData in unlockedRecipes)
        {
            GameObject recipe = Instantiate(recipeUiPrefab, recipesParent);
            recipe.GetComponent<Recipe>().Configure(recipeData);
        }
        // for (int i = 0; i< availableRecipes.Length; i++)
        // {
        //     GameObject recipe =Instantiate(recipeUiPrefab, recipesParent);
        //     recipe.GetComponent<Recipe>().Configure(availableRecipes[i]);
        // }
    }

    // Update is called once per frame
}
