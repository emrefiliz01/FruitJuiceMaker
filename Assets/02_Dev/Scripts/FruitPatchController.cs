using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class FruitPatchController : MonoBehaviour
{
    [SerializeField] private FruitPatchSO fruitPatchSO;
    [SerializeField] private List<GameObject> stageList;
    [SerializeField] private Image timerImage;
    [SerializeField] private Image fruitIcon;

    private int currentStage;
    private float currentTimer;
    private Coroutine timerCoroutine;
    private bool isReady;


    private void Start()
    {
        currentStage = 1;

        ChangeStage();
        ResetStage();

        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentStage = 1;
            ChangeStage();
            SetImageStatus(true);
            timerCoroutine = StartCoroutine(TimerCoroutine());
            SetFruitIconStatus(false);
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
            StopCoroutine(timerCoroutine);   
            SetFruitIconStatus(true);
            isReady = true;
        }

        ChangeStage();
    }
    private IEnumerator TimerCoroutine()
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

    private void ChangeStage()
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
    private void SetFruitIconStatus(bool status)
    {
        fruitIcon.gameObject.SetActive(status);

        fruitIcon.sprite = fruitPatchSO.fruitImage;
    }

    public void CollectFruit()
    {
        ResetStage();
        SetImageStatus(true);
        SetFruitIconStatus(false);
        StartCoroutine(TimerCoroutine());
    }

    private void ResetStage()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        currentStage = 1;
        currentTimer = fruitPatchSO.stageTimer;

        isReady = false;
        SetFruitIconStatus(false);
        ChangeStage();
    }

    public bool IsReady()
    {
        return isReady;
    }

    public FruitPatchSO GetFruitPatchSO()
    {
        return fruitPatchSO;
    }
}
