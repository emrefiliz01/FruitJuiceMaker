using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class DollarFlyer : MonoBehaviour
{
    public GameObject dollarUIPrefab;
    public RectTransform canvasRoot;
    public RectTransform moneyTarget;


    public void FlyDollar(Vector3 spawnPoint)
    {  
        StartCoroutine(FlyDollarUICoroutine());
    }

    private IEnumerator FlyDollarUICoroutine()
    {
        Vector2 targetPos = moneyTarget.anchoredPosition;

        for (int i = 0; i < 5; i++)
        {
            GameObject dollarUI = Instantiate(dollarUIPrefab, canvasRoot);
            RectTransform dollarRect = dollarUI.GetComponent<RectTransform>();

            dollarRect.anchoredPosition = Vector2.zero;

            dollarRect.DOAnchorPos(targetPos, 0.7f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                Destroy(dollarUI);
            });

            yield return null;
        
        }
    }
}
