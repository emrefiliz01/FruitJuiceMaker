using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPatchController : MonoBehaviour
{
    [SerializeField] private FruitPatchSO fruitPatchSO;
    [SerializeField] private List<GameObject> stageList;

    int currentStage;
    int currentTimer;

    private void Start()
    {
        currentStage = 1;

        foreach (GameObject stage in stageList)
        {
            stage.SetActive(false);
        }

         stageList[currentStage - 1].SetActive(true);   
    }
}
