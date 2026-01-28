using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public RectTransform fillRect;
    public RectTransform ghostFillRect;
    public Text hpText;

    private float targetFill = 1f;
    private float currentMainFill = 1f;
    private float currentGhostFill = 1f;
    
    public float mainLerpSpeed = 10f;
    public float ghostLerpSpeed = 2f;
    public float ghostDelay = 0.5f;
    private float ghostTimer = 0f;

    void Start()
    {
        TryFindPlayerHealth();
    }

    private void TryFindPlayerHealth()
    {
        PlayerHealth ph = FindFirstObjectByType<PlayerHealth>();
        if (ph != null)
        {
            ph.OnHPChanged += UpdateHP;
            
            float fill = (float)ph.currentHP / ph.maxHP;
            targetFill = fill;
            currentMainFill = fill;
            currentGhostFill = fill;
            if (fillRect != null) fillRect.localScale = new Vector3(fill, 1, 1);
            if (ghostFillRect != null) ghostFillRect.localScale = new Vector3(fill, 1, 1);
            if (hpText != null) hpText.text = $"{ph.currentHP}/{ph.maxHP}";
            Debug.Log($"[HealthBarUI] Connected to {ph.gameObject.name}");
        }
    }

    void UpdateHP(int current, int max)
    {
        targetFill = (float)current / max;
        if (hpText != null) hpText.text = $"{current}/{max}";
        
        if (targetFill < currentGhostFill)
        {
            ghostTimer = ghostDelay;
        }
    }

    void Update()
    {
        if (fillRect != null && Mathf.Abs(currentMainFill - targetFill) > 0.001f)
        {
            currentMainFill = Mathf.Lerp(currentMainFill, targetFill, Time.deltaTime * mainLerpSpeed);
            fillRect.localScale = new Vector3(currentMainFill, 1, 1);
        }
        else if (fillRect != null)
        {
            currentMainFill = targetFill;
            fillRect.localScale = new Vector3(currentMainFill, 1, 1);
        }

        if (ghostTimer > 0)
        {
            ghostTimer -= Time.deltaTime;
        }
        else if (ghostFillRect != null && currentGhostFill > currentMainFill)
        {
            currentGhostFill = Mathf.Lerp(currentGhostFill, currentMainFill, Time.deltaTime * ghostLerpSpeed);
            if (Mathf.Abs(currentGhostFill - currentMainFill) < 0.001f) currentGhostFill = currentMainFill;
            ghostFillRect.localScale = new Vector3(currentGhostFill, 1, 1);
        }
        else if (ghostFillRect != null && currentGhostFill < currentMainFill)
        {
            currentGhostFill = currentMainFill;
            ghostFillRect.localScale = new Vector3(currentGhostFill, 1, 1);
        }
    }
}
