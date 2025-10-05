using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DollarFlyer : MonoBehaviour
{
    public GameObject dollarPrefab;
    public Transform worldSpawnPoint;
    public RectTransform canvasRoot;
    public RectTransform moneyTarget;

    public Camera mainCam;

    public void FlyDollar()
    {
        Vector3 screenPos = mainCam.WorldToScreenPoint(worldSpawnPoint.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRoot, screenPos, null, out Vector2 localPos);

        GameObject dollar = Instantiate(dollarPrefab, canvasRoot);
        dollar.GetComponent<RectTransform>().anchoredPosition = localPos;

        dollar.GetComponent<RectTransform>().DOAnchorPos(moneyTarget.anchoredPosition, 0.7f).SetEase(Ease.InOutQuad).OnComplete(() =>
        Destroy(dollar)
        );
    }
}
