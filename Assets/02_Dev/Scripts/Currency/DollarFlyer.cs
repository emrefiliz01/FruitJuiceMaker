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

            Vector2 randomPos = new Vector2(Random.Range(-70, 70), Random.Range(-70, 70));
            dollarRect.DOAnchorPos(randomPos, 0.4f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                dollarRect.DOAnchorPos(targetPos, 0.7f).SetEase(Ease.InOutQuad).OnComplete(() =>
                {
                    Destroy(dollarUI);
                });
            });
            
            yield return null;
        }
    }
}
