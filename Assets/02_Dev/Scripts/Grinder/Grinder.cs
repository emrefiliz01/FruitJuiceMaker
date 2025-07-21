using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grinder : MonoBehaviour
{
    [SerializeField] GrinderSO grinderSO;

    [SerializeField] private PlayerController playerController;

    private bool isGrinding;

    public void StartGrinder()
    {
        isGrinding = true;

        int fruitCount = playerController.collectedFruits.Count;

        foreach (var fruit in playerController.collectedFruits)
        {
            Destroy(fruit);
        }

        playerController.collectedFruits.Clear();

        Debug.Log(fruitCount + " fruitss removed");
    }

    private IEnumerator GrinderTimerCoroutine()
    {
        float currentGrinderTimer = grinderSO.grindingTime;

        while (currentGrinderTimer > 0)
        {
            currentGrinderTimer-= Time.deltaTime;
            yield return null;

            Debug.Log("Fruitss are grinded");

            isGrinding = false;
        }
    }
}
