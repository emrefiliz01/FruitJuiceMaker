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

        if (currentStage <= stageList.Count && currentStage != stageList.Count)
        {
            StageReset();
        }
        else
        {
            StageReset();

            StopCoroutine(startTimerCoroutine);

            timerText.gameObject.SetActive(false);
        }
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
        timerText.gameObject.SetActive(true);
    }
}
