using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FruitPatchController : MonoBehaviour
{
    [SerializeField] private FruitPatchSO fruitPatchSO;
    [SerializeField] private List<GameObject> stageList;
    [SerializeField] private Image timerImage;

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
            SetImageStatus(true);
            StartCoroutine(StartTimerCoroutine());
        }   
    }
    
    private void IncreaseFruitStage()
    {
        Debug.Log(currentStage);

        currentStage += 1;

        if (currentStage < stageList.Count)
        {
            SetImageStatus(true);
        }
        else
        {
            SetImageStatus(false);
            StopCoroutine(startTimerCoroutine);     
        }

        StageReset();
    }
    private IEnumerator StartTimerCoroutine()
    {
        while (currentStage != stageList.Count)
        {
            yield return null;

            currentTimer -= Time.deltaTime;

            SetTimerImageFillAmount();

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

    private void SetImageStatus(bool status)
    {
        timerImage.gameObject.SetActive(status);
    }

    private void SetTimerImageFillAmount()
    {
        if (currentTimer > 0)
        {
            float fillAmount = currentTimer / fruitPatchSO.stageTimer;
            timerImage.fillAmount = fillAmount;
        }
        else
        {
            timerImage.fillAmount = 0f;
        }
    }
}
