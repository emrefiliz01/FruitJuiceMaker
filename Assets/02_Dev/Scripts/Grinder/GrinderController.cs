using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GrinderController : MonoBehaviour
{
    [SerializeField] GrinderSO grinderSO;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Image grinderTimerImage;
    [SerializeField] private TextMesh grinderFruitCountText;

    private bool isGrinding;
    private Coroutine grinderCoroutine;
    private Coroutine grinderProcessCoroutine;

    private PlayerInteracton playerInteracton;
    public GrindedFruitController grindedFruitController;
    public List<GameObject> grinderFruitsList;
    

    private void Start()
    {
        isGrinding = false;
        GrinderFruitCountText();
    }

    private void Update()
    {
        
    }
    public void StartGrinder()
    {
        if (grindedFruitController.grindedFruitBowlList.Count == grinderSO.grindedFruitBowlCapacity)
        {
            ResetGrinder();
            return;
        }
        
        if (isGrinding)
        {
            return;
        }
        isGrinding = true;
        SetImageStatus(true);
        
        grinderCoroutine = StartCoroutine(GrinderTimerCoroutine());
    }

    private IEnumerator GrinderTimerCoroutine()
    {
        float currentGrinderTimer = grinderSO.grindingTime;

        grinderTimerImage.fillAmount = 1f;

        while (grinderFruitsList.Count > 0)
        {
            if (currentGrinderTimer > 0)
            {
                currentGrinderTimer -= Time.deltaTime;

                float fillAmount = currentGrinderTimer / grinderSO.grindingTime;

                grinderTimerImage.fillAmount = fillAmount;

                GrinderFruitCountText();

                yield return null;
            }
            else
            {
                grinderFruitsList.RemoveAt(0);

                CanCreateGrindedFruit();

                currentGrinderTimer = grinderSO.grindingTime;

                
            }
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

    public void ResetGrinder()
    {
        grinderTimerImage.fillAmount = 1f;

        isGrinding = false;

        GrinderFruitCountText();

        StopCoroutine(grinderCoroutine);
    }

    public bool CanAddFruit()
    {
        return grinderFruitsList.Count < grinderSO.grinderCapacity;
    }

    public void GrinderFruitCountText()
    {
        grinderFruitCountText.text = grinderFruitsList.Count.ToString();
    }

    public void AddFruit(GameObject fruit)
    {
        if (CanAddFruit())
        {
            grinderFruitsList.Add(fruit);

            GrinderFruitCountText();
        }
    }

    public bool CanCreateGrindedFruit()
    {
        if (grindedFruitController != null && grindedFruitController.CreateGrindedFruit())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
