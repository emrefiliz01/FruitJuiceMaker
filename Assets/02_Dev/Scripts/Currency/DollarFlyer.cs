using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class DollarFlyer : MonoBehaviour
{
    public GameObject dollarWorldPrefab;
    public GameObject dollarUIPrefab;
    public RectTransform canvasRoot;
    public RectTransform moneyTarget;
    public Camera mainCam;

    public void FlyDollar(Transform spawnPoint)
    {
        GameObject dollar3D = Instantiate(dollarWorldPrefab, spawnPoint.position, Quaternion.identity);

        StartCoroutine(FlyDollarUICoroutine(dollar3D));

        Destroy(dollar3D, 0.4f);
    }

    private IEnumerator FlyDollarUICoroutine(GameObject dollar3D)
    {
        GameObject dollarUI = Instantiate(dollarUIPrefab, canvasRoot);
        RectTransform dollarRect = dollarUI.GetComponent<RectTransform>();

        float maxDuration = 0.5f;
        float timer = 0f;

        Vector2 screenTargetPos = RectTransformUtility.WorldToScreenPoint(null, moneyTarget.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRoot, screenTargetPos, null, out Vector2 localTargetPos);

        while (dollar3D != null && timer < maxDuration)
        {
            Vector3 screenPos = mainCam.WorldToScreenPoint(dollar3D.transform.position);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRoot, screenPos, null, out Vector2 uiPos);

            dollarRect.anchoredPosition = uiPos;

            timer += Time.deltaTime;

            yield return null;
        }

        dollarRect.DOAnchorPos(localTargetPos, 0.7f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            Destroy(dollarUI);
        });
    }
}
