using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrashBinController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GrindedFruitController grindedFruitController;
    [SerializeField] private JuiceMakerController juiceMakerController;
    [SerializeField] private Transform trashBinThrowPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ThrowAllItems();
        }
    }
    private void ThrowAllItems()
    {
        #region Fruits
        foreach (GameObject fruit in playerController.collectedFruitList)
        {
            if (fruit != null)
            {
                fruit.transform.DOMove(trashBinThrowPoint.position, 0.7f).OnComplete(() => Destroy(fruit));
            }
        }

        playerController.collectedFruitList.Clear();
        #endregion

        #region Grinded Fruits
        foreach (GameObject grindedFruit in playerController.collectedGrindedFruitList)
        {
            if (grindedFruit != null)
            {
                grindedFruit.transform.DOMove(trashBinThrowPoint.position, 0.7f).OnComplete(() => Destroy(grindedFruit));
            }
        }

        playerController.collectedGrindedFruitList.Clear();
        #endregion

        #region Juice
        foreach (GameObject juice in playerController.collectedJuiceList)
        {
            if (juice != null)
            {
                juice.transform.DOMove(trashBinThrowPoint.position, 0.7f).OnComplete(() => Destroy(juice));
            }
        }

        playerController.collectedJuiceList.Clear();
        #endregion

        playerController.isHolding = false;
    }
}
