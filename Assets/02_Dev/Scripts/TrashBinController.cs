using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBinController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GrindedFruitController grindedFruitController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ClearAndDestroyLists();
        }
    }

    private void ClearAndDestroyLists()
    {
        foreach (var fruit in playerController.collectedFruitList)
        {
            Destroy(fruit);
        }

        playerController.collectedFruitList.Clear();

        /*foreach (var grindedFruit in playerController.coll)
        {
            Destroy(grindedFruit);
        }

        grindedFruitController.grindedFruitBowlList.Clear();

        playerController.isHolding = false;*/
    }
}
