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
    private Coroutine startTimerCoroutine;

    private void Start()
    {
        currentStage = 1;

        StageReset();

        startTimerCoroutine = StartCoroutine(StartTimerCoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentStage = 1;
            StageReset();
            SetTextStatus(true);
            StartCoroutine(StartTimerCoroutine());
        }

            //  if (currentTimer > 0 && currentStage != stageList.Count)
            // {
            //      currentTimer -= Time.deltaTime;
            //  }
            //  else
            //  {
            //     currentTimer = 0;
            //   IncreaseFruitStage();
            //}

            timerText.text = currentTimer.ToString("F0");
    }
    
    private void IncreaseFruitStage()
    {
        Debug.Log(currentStage);

        currentStage += 1;

        if (currentStage < stageList.Count)
        {
            SetTextStatus(true);
        }
        else
        {
            SetTextStatus(false);
            StopCoroutine(startTimerCoroutine);     
        }

        StageReset();
    }
    private IEnumerator StartTimerCoroutine()
    {
        while (currentStage != stageList.Count)
        {
            yield return new WaitForSeconds(1f);

            currentTimer -= 1;

            if (currentTimer <= 0)
            {
                IncreaseFruitStage();
            }
        }
        
        yield return null;
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

    private void SetTextStatus(bool status)
    {
        timerText.gameObject.SetActive(status);
    }
}
