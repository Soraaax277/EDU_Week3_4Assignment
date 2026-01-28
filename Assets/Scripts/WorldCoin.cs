using UnityEngine;

public class WorldCoin : MonoBehaviour
{
    public float moveDuration = 1.0f;
    private float elapsed = 0f;
    private Vector3 startPos;
    private Transform uiTarget;
    private Camera mainCam;

    void Start()
    {
        startPos = transform.position;
        mainCam = Camera.main;
        
        CoinUI coinUI = FindFirstObjectByType<CoinUI>();
        if (coinUI != null)
        {
            uiTarget = coinUI.coinIconTransform;
        }
    }

    void Update()
    {
        if (uiTarget == null) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / moveDuration);
        float easedT = EasingFunctions.EaseInOutQuad(t);

        transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, uiTarget.position);
        float startDepth = Vector3.Dot(startPos - mainCam.transform.position, mainCam.transform.forward);
        float currentDepth = Mathf.Lerp(startDepth, 0.3f, easedT);
        
        Vector3 worldTarget = mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, currentDepth));

        transform.position = Vector3.Lerp(startPos, worldTarget, easedT);
        transform.localScale = Vector3.one * (1f - easedT * 0.5f);

        if (t >= 1f)
        {
            if (CoinUI.Instance != null) CoinUI.Instance.AddCoins(1);
            Destroy(gameObject);
        }
    }
}
