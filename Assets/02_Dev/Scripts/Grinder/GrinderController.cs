using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrinderController : MonoBehaviour
{
    [SerializeField] GrinderSO grinderSO;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Image grinderTimerImage;
    [SerializeField] private GameObject grindedFruitBowlPrefab;
    [SerializeField] private GameObject grindedFruitBowlSpawnPoint;
    [SerializeField] private GameObject grindedFruitSpawnerContainer;
    [SerializeField] private TextMesh grinderFruitCountText;

    private bool isGrinding;
    private Coroutine grinderCoroutine;

    public List<GameObject> grinderFruitsList;
    public List<GameObject> grindedFruitBowlList;

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
        if (grindedFruitBowlList.Count == grinderSO.grindedFruitBowlCapacity)
        {
            ResetGrinder();
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

    private void ResetGrinder()
    {
        grinderTimerImage.fillAmount = 1f;

        isGrinding = false;

        GrinderFruitCountText();

        StopCoroutine(grinderCoroutine);
    }

    public bool CanAddFruit()
    {
        if (grinderFruitsList.Count < grinderSO.grinderCapacity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanCreateGrindedFruit()
    {
        if (grindedFruitBowlList.Count < grinderSO.grindedFruitBowlCapacity)
        {
            GameObject grindedFruit = Instantiate(grindedFruitBowlPrefab, grindedFruitBowlSpawnPoint.transform.position + new Vector3(0, 0, 1 * grindedFruitBowlList.Count), Quaternion.identity);

            grindedFruit.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);

            grindedFruit.transform.SetParent(grindedFruitBowlSpawnPoint.transform);

            grindedFruitBowlList.Add(grindedFruit);

            IsTableFull();

            return true;
        }
        else
        {
            return false;
        }
    }

    private void GrinderFruitCountText()
    {
        grinderFruitCountText.text = grinderFruitsList.Count.ToString();
    }

    public void AddFruitIntoGrinder(List<GameObject> collectedFruitList)
    {
        foreach (var fruit in collectedFruitList)
        {
            grinderFruitsList.Add(fruit);
        }
    }

    private bool IsTableFull()
    {
        if (grindedFruitBowlList.Count == grinderSO.grindedFruitBowlCapacity)
        {
            ResetGrinder();
            return true;
        }
        else
        {
            return false;
        }
    }
}
