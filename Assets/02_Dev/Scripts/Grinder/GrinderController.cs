using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrinderController : MonoBehaviour
{
    [SerializeField] GrinderSO grinderSO;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Image grinderTimerImage;

    private bool isGrinding;
    private float currentGrinderTimer;

    private void Start()
    {
        isGrinding = false;
    }
    public void StartGrinder()
    {
        isGrinding = true;

        SetImageStatus(true);
            
        int fruitCount = playerController.collectedFruits.Count;

        foreach (var fruit in playerController.collectedFruits)
        {
            Destroy(fruit);
        }

        playerController.collectedFruits.Clear();

        Debug.Log(fruitCount + " fruitss removed");

        StartCoroutine(GrinderTimerCoroutine());
    }

    private IEnumerator GrinderTimerCoroutine()
    {
        float currentGrinderTimer = grinderSO.grindingTime;

        grinderTimerImage.fillAmount = 1f;

        while (currentGrinderTimer > 0)
        {
            currentGrinderTimer -= Time.deltaTime;

            float fillAmount = currentGrinderTimer / grinderSO.grindingTime;

            grinderTimerImage.fillAmount = fillAmount;

            yield return null;
        }

        ResetGrinder();
    }

    public bool IsGrinding()
    {
        return isGrinding;
    }

    public GrinderSO GetGrinderSO()
    {
        return grinderSO;
    }

    private void SetImageStatus(bool status)
    {
        grinderTimerImage.gameObject.SetActive(status);
    }

    private void ResetGrinder()
    {
        grinderTimerImage.fillAmount = 1f;

        isGrinding = false;
    }
}
