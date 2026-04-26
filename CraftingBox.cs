using System.Collections.Generic;
using UnityEngine;

public class CraftingBox : MonoBehaviour
{
    public List<MaterialItem> storedMaterials = new List<MaterialItem>();

    public Transform outputPoint;

    public GameObject swordPrefab;
    public GameObject spearPrefab;
    public GameObject daggerPrefab;

    void OnTriggerEnter2D(Collider2D col)
    {
        MaterialItem mat = col.GetComponent<MaterialItem>();

        if (mat != null && !storedMaterials.Contains(mat))
        {
            storedMaterials.Add(mat);
            Debug.Log("Added: " + mat.type);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        MaterialItem mat = col.GetComponent<MaterialItem>();

        if (mat != null && storedMaterials.Contains(mat))
        {
            storedMaterials.Remove(mat);
            Debug.Log("Removed: " + mat.type);
        }
    }

    public void Craft()
    {
        if (MatchRecipe(1, 2))
        {
            SpawnItem(swordPrefab);
            ConsumeMaterials(1, 2);
            Debug.Log("Crafted Sword!");
        }
        else if (MatchRecipe(2, 1))
        {
            SpawnItem(spearPrefab);
            ConsumeMaterials(2, 1);
            Debug.Log("Crafted Spear!");
        }
        else if (MatchRecipe(1, 1))
        {
            SpawnItem(daggerPrefab);
            ConsumeMaterials(1, 1);
            Debug.Log("Crafted Dagger!");
        }
        else
        {
            Debug.Log("Invalid recipe!");
        }
    }

    bool MatchRecipe(int woodRequired, int ironRequired)
    {
        int wood = 0;
        int iron = 0;

        foreach (MaterialItem mat in storedMaterials)
        {
            if (mat.type == MaterialType.Wood) wood++;
            if (mat.type == MaterialType.Iron) iron++;
        }

        return wood >= woodRequired && iron >= ironRequired;
    }

    void ConsumeMaterials(int woodRequired, int ironRequired)
    {
        int woodUsed = 0;
        int ironUsed = 0;

        List<MaterialItem> toRemove = new List<MaterialItem>();

        foreach (MaterialItem mat in storedMaterials)
        {
            if (mat.type == MaterialType.Wood && woodUsed < woodRequired)
            {
                woodUsed++;
                toRemove.Add(mat);
            }
            else if (mat.type == MaterialType.Iron && ironUsed < ironRequired)
            {
                ironUsed++;
                toRemove.Add(mat);
            }
        }

        foreach (MaterialItem mat in toRemove)
        {
            storedMaterials.Remove(mat);
            Destroy(mat.gameObject);
        }
    }

    void SpawnItem(GameObject prefab)
    {
        Instantiate(prefab, outputPoint.position, Quaternion.identity);
    }
}