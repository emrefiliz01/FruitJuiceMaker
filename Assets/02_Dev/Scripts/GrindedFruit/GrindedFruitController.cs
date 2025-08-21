using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindedFruitController : MonoBehaviour
{
    [SerializeField] private GameObject grindedFruitBowlSpawnPoint;
    [SerializeField] private GrindedFruitSO grindedFruitSO;
    [SerializeField] private GrinderController grinderController;
    private JuiceMakerSO juiceMakerSO;

    public List<GameObject> grindedFruitBowlList = new List<GameObject>();

    public bool CreateGrindedFruit()
    {
        if (grindedFruitBowlList.Count < grindedFruitSO.grindedFruitCapacity)
        {
            GameObject grindedFruit = Instantiate(grindedFruitSO.grindedFruitPrefab, grindedFruitBowlSpawnPoint.transform.position + new Vector3(0, 0, 1 * grindedFruitBowlList.Count), Quaternion.identity);

            grindedFruit.transform.SetParent(grindedFruitBowlSpawnPoint.transform);

            grindedFruitBowlList.Add(grindedFruit);

            if (IsTableFull())
            {
                grinderController.ResetGrinder();
            }

            CheckAndStartGrinder();

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
            return true;
        }
        else
        {
            return false;
        }
    } 

    public void CheckAndStartGrinder()
    {
        if (grinderController != null && grinderController.CanAddFruit() && grinderController.grinderFruitsList.Count > 0)
        {
            grinderController.StartGrinder();
        }
    }
}
