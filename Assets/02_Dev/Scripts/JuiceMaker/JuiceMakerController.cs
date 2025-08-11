using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JuiceMakerController : MonoBehaviour
{
    [SerializeField] private Image juiceMakerTimerImage;
    [SerializeField] private TextMesh juiceMakerCountText;
    [SerializeField] JuiceMakerSO juiceMakerSO;
    [SerializeField] JuiceSO JuiceSO;
    [SerializeField] GameObject juiceSpawnPoint;

    public PlayerController playerController;
    public GrindedFruitController grindedFruitController;
    private Coroutine juiceMakerCoroutine;
    public List<GameObject> juiceMakerList;
    public List<GameObject> juiceList;


    private bool isJuicing;

    private void Start()
    {
        isJuicing = false;
    }
    

    public void StartJuiceMaker()
    {
        if (juiceList.Count == JuiceSO.juiceCapacity)
        {
            ResetJuiceMaker();
            return;
        }

        if (isJuicing)
        {
            return;
        }
        isJuicing = true;
        SetJuiceMakerImageStatus(true);

        juiceMakerCoroutine = StartCoroutine(JuiceMakerTimerCoroutine());
    }



    private IEnumerator JuiceMakerTimerCoroutine()
    {
        float currentJuiceMakingTimer = juiceMakerSO.juiceMakingTime;

        juiceMakerTimerImage.fillAmount = 1f;

        while (juiceMakerList.Count > 0)
        {
            if (currentJuiceMakingTimer > 0)
            {
                currentJuiceMakingTimer -= Time.deltaTime;

                float fillAmount = currentJuiceMakingTimer / juiceMakerSO.juiceMakingTime;

                juiceMakerTimerImage.fillAmount = fillAmount;

                JuiceMakerCountText();

                yield return null;
            }
            else
            {
                juiceMakerList.RemoveAt(0);

                CreateJuice();

                currentJuiceMakingTimer = juiceMakerSO.juiceMakingTime;
            }
        }
        ResetJuiceMaker();
    }

    private void SetJuiceMakerImageStatus(bool status)
    {
        juiceMakerTimerImage.gameObject.SetActive(status);
    }

    public void ResetJuiceMaker()
    {
        juiceMakerTimerImage.fillAmount = 1f;

        isJuicing = false;

        JuiceMakerCountText();

        StopCoroutine(juiceMakerCoroutine);
    }

    public void JuiceMakerCountText()
    {
        juiceMakerCountText.text = juiceMakerList.Count.ToString();
    }

    public void AddGrindedFruit(GameObject grindedFruit)
    {
        if (CanAddGrindedFruit())
        {
            juiceMakerList.Add(grindedFruit);

            JuiceMakerCountText();
        }
    }
    public bool CanAddGrindedFruit()
    {
        return playerController.collectedGrindedFruitList.Count < JuiceSO.juiceCapacity;
    }

    private bool IsJuiceTableFull()
    {
        if (juiceMakerList.Count == JuiceSO.juiceCapacity)
        {
            ResetJuiceMaker();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CreateJuice()
    {
        if (juiceList.Count < JuiceSO.juiceCapacity)
        {
            GameObject juice = Instantiate(JuiceSO.juicePrefab, juiceSpawnPoint.transform.position + new Vector3(-1 * juiceList.Count, 0, 0), Quaternion.identity);

            juice.transform.SetParent(juiceSpawnPoint.transform);

            juiceList.Add(juice);

            IsJuiceTableFull();

            return true;
        }
        else
        {
            return false;
        }
    }
}
