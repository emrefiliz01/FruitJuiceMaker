using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FruitPatchController : MonoBehaviour
{
    [SerializeField] private FruitPatchSO fruitPatchSO;
    [SerializeField] private List<GameObject> stageList;
    [SerializeField] private TextMeshPro timerText;

    private int currentStage;
    private float currentTimer;

    private void Start()
    {
        currentStage = 1;

        StageReset();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
           // StageReset();
            currentStage = 0;
        }


        if (currentTimer > 0 && currentStage != stageList.Count)
        {
            currentTimer -= Time.deltaTime;
        }
        else
        {
            currentTimer = 0;
            IncreaseFruitStage();
        }

        timerText.text = currentTimer.ToString("F0");
    }

    private void IncreaseFruitStage()
    {
        if (currentStage >= stageList.Count)
        {
            currentTimer = 0;
        }
        else
        {
            currentStage++;

            StageReset();
        }

        
    }

    private void StageReset()
    {
        foreach (GameObject stage in stageList)
        {
            stage.SetActive(false);
        }

        stageList[currentStage - 1].SetActive(true);

        currentTimer = fruitPatchSO.stageTimer;
    }
}
