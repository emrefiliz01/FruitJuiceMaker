using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBinController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GrindedFruitController grindedFruitController;
    [SerializeField] private JuiceMakerController juiceMakerController;

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

        foreach (var grindedFruit in playerController.collectedGrindedFruitList)
        {
            Destroy(grindedFruit);
        }

        playerController.collectedGrindedFruitList.Clear();

        foreach (var juice in playerController.collectedJuiceList)
        {
            Destroy(juice);
        }

        playerController.collectedJuiceList.Clear();


        playerController.isHolding = false;
    }
}
