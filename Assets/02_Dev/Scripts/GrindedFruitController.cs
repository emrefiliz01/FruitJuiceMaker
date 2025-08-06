using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindedFruitController : MonoBehaviour
{
    [SerializeField] private GameObject grindedFruitController;
    [SerializeField] private GameObject grindedFruitBowlSpawnPoint;
    [SerializeField] private GrindedFruitSO grindedFruitSO;
    [SerializeField] private GrinderController grinderController;

    public List<GameObject> grindedFruitBowlList = new List<GameObject>();

    public bool CreateGrindedFruit()
    {
        if (grindedFruitBowlList.Count < grindedFruitSO.grindedFruitCapacity)
        {
            GameObject grindedFruit = Instantiate(grindedFruitSO.grindedFruitPrefab, grindedFruitBowlSpawnPoint.transform.position + new Vector3(0, 0, 1 * grindedFruitBowlList.Count), Quaternion.identity);

            grindedFruit.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);

            grindedFruit.transform.SetParent(grindedFruitBowlSpawnPoint.transform);

            grindedFruitBowlList.Add(grindedFruit);

            IsTableFull();

            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsTableFull()
    {
        if (grindedFruitBowlList.Count == grindedFruitSO.grindedFruitCapacity)
        {
            grinderController.ResetGrinder();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanAddGrindedFruit()
    {
        return grindedFruitBowlList.Count < grindedFruitSO.grindedFruitCapacity;
    }
}
