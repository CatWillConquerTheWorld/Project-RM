using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    private enum ComboTier
    {
        Hidden = -1,
        Tier5 = 0,
        Tier10 = 1,
        Tier20 = 2,
        Tier30 = 3
    }

    [Header("UI References")]
    [SerializeField] private RectTransform comboRoot = null;
    [SerializeField] private Image comboBackgroundImage = null;

    [Header("Single Digit UI (5~9)")]
    [SerializeField] private TMP_Text singleDigitNumberText = null;
    [SerializeField] private TMP_Text singleDigitComboText = null;

    [Header("Double Digit UI (10~29)")]
    [SerializeField] private TMP_Text doubleDigitNumberText = null;
    [SerializeField] private TMP_Text doubleDigitComboText = null;

    [Header("30+ UI")]
    [SerializeField] private TMP_Text thirtyPlusNumberText = null;
    [SerializeField] private TMP_Text thirtyPlusComboText = null;

    [Header("Background")]
    [SerializeField] private Sprite comboBackgroundSprite = null;

    [Header("Animation")]
    [SerializeField] private float hiddenOffsetX = -560f;
    [SerializeField] private float slideInDuration = 0.2f;
    [SerializeField] private float slideOutDuration = 0.15f;
    [SerializeField] private float punchScale = 0.18f;
    [SerializeField] private float punchDuration = 0.12f;
    [SerializeField] private float tier20ShakeStrength = 8f;
    [SerializeField] private float tier20ShakeDuration = 0.45f;
    [SerializeField] private int tier20ShakeVibrato = 12;
    [SerializeField] private float tier30ShakeStrength = 14f;
    [SerializeField] private float tier30ShakeDuration = 0.28f;
    [SerializeField] private int tier30ShakeVibrato = 22;
    [SerializeField] private float doubleDigitComboFontSize = 35f;
    [SerializeField] private float tier20ComboFontSize = 50f;

    public int currentCombo = 0;

    private const int VisibleComboThreshold = 5;
    private static readonly Color WhiteTextColor = Color.white;
    private static readonly Color Tier20NumberColor = new Color32(0xD3, 0xF0, 0xFD, 0xFF);
    private static readonly Color Tier30NumberColor = new Color32(0xEF, 0x91, 0x63, 0xFF);

    private Sequence rootSequence;
    private Tween numberPunchTween;
    private Tween shakeTween;
    private bool isVisible;
    private Vector2 visibleAnchoredPosition;
    private Quaternion singleDigitNumberBaseRotation;
    private Quaternion doubleDigitNumberBaseRotation;
    private Quaternion thirtyPlusNumberBaseRotation;

    private void Awake()
    {
        EnsureReferences();
        HideImmediate();
    }

    private void OnDisable()
    {
        KillTweens();
    }

    public void IncreaseCombo(int nNum = 1)
    {
        currentCombo += nNum;
        Debug.Log("Combo = " + currentCombo);

        if (currentCombo < VisibleComboThreshold)
        {
            return;
        }

        ComboTier currentTier = GetTier(currentCombo);
        RefreshVisuals(currentTier);

        if (!isVisible)
        {
            ShowComboUi();
            return;
        }

        PlayNumberPunch();
        UpdateShake(currentTier);
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        Debug.Log("Reset Combo = " + currentCombo);

        if (!isVisible)
        {
            HideImmediate();
            return;
        }

        HideAnimated();
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }

    public bool GetCurrentComboBool()
    {
        return currentCombo >= VisibleComboThreshold;
    }

    private void EnsureReferences()
    {
        if (comboRoot == null)
        {
            comboRoot = transform as RectTransform;
        }

        if (comboRoot != null)
        {
            visibleAnchoredPosition = comboRoot.anchoredPosition;
        }

        singleDigitNumberBaseRotation = CaptureTextRotation(singleDigitNumberText);
        doubleDigitNumberBaseRotation = CaptureTextRotation(doubleDigitNumberText);
        thirtyPlusNumberBaseRotation = CaptureTextRotation(thirtyPlusNumberText);
    }

    private void RefreshVisuals(ComboTier tier)
    {
        ApplyBackgroundSprite();
        UpdateActiveTexts(tier);
    }

    private void ApplyBackgroundSprite()
    {
        if (comboBackgroundImage == null)
        {
            return;
        }

        if (comboBackgroundSprite != null)
        {
            comboBackgroundImage.sprite = comboBackgroundSprite;
        }
    }

    private void ShowComboUi()
    {
        isVisible = true;
        KillRootSequence();

        if (comboRoot != null)
        {
            comboRoot.anchoredPosition = GetHiddenAnchoredPosition();
            comboRoot.localScale = Vector3.one;
        }

        rootSequence = DOTween.Sequence();
        if (comboRoot != null)
        {
            rootSequence.Append(comboRoot.DOAnchorPos(visibleAnchoredPosition, slideInDuration).SetEase(Ease.OutCubic));
        }

        rootSequence.OnComplete(() =>
        {
            PlayNumberPunch();
            UpdateShake(GetTier(currentCombo));
        });
    }

    private void HideAnimated()
    {
        isVisible = false;
        KillNumberAndShakeTweens();
        KillRootSequence();

        rootSequence = DOTween.Sequence();
        if (comboRoot != null)
        {
            rootSequence.Append(comboRoot.DOAnchorPos(GetHiddenAnchoredPosition(), slideOutDuration).SetEase(Ease.InCubic));
        }

        rootSequence.OnComplete(HideImmediate);
    }

    private void HideImmediate()
    {
        KillNumberAndShakeTweens();
        KillRootSequence();
        isVisible = false;

        if (comboRoot != null)
        {
            comboRoot.anchoredPosition = GetHiddenAnchoredPosition();
            comboRoot.localScale = Vector3.one;
        }

        SetAllVisualsActive(false);
    }

    private void PlayNumberPunch()
    {
        TMP_Text activeNumberText = GetActiveNumberText(GetTier(currentCombo));
        if (activeNumberText == null)
        {
            return;
        }

        RectTransform numberRect = activeNumberText.rectTransform;
        if (numberRect == null)
        {
            return;
        }

        if (numberPunchTween != null && numberPunchTween.IsActive())
        {
            numberPunchTween.Kill();
        }

        numberRect.localScale = Vector3.one;
        numberPunchTween = numberRect.DOPunchScale(Vector3.one * punchScale, punchDuration, 1, 0f).SetUpdate(true);
    }

    private void UpdateShake(ComboTier tier)
    {
        TMP_Text activeNumberText = GetActiveNumberText(tier);
        if (activeNumberText == null)
        {
            return;
        }

        RectTransform numberRect = activeNumberText.rectTransform;
        if (numberRect == null)
        {
            return;
        }

        if (shakeTween != null && shakeTween.IsActive())
        {
            shakeTween.Kill();
        }

        numberRect.localRotation = GetBaseRotation(activeNumberText);

        if (tier == ComboTier.Tier20)
        {
            shakeTween = numberRect.DOShakeRotation(
                tier20ShakeDuration,
                new Vector3(0f, 0f, tier20ShakeStrength),
                tier20ShakeVibrato,
                90f,
                false)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear)
                .SetUpdate(true);
        }
        else if (tier == ComboTier.Tier30)
        {
            shakeTween = numberRect.DOShakeRotation(
                tier30ShakeDuration,
                new Vector3(0f, 0f, tier30ShakeStrength),
                tier30ShakeVibrato,
                90f,
                false)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear)
                .SetUpdate(true);
        }
    }

    private static ComboTier GetTier(int combo)
    {
        if (combo >= 30)
        {
            return ComboTier.Tier30;
        }

        if (combo >= 20)
        {
            return ComboTier.Tier20;
        }

        if (combo >= 10)
        {
            return ComboTier.Tier10;
        }

        if (combo >= VisibleComboThreshold)
        {
            return ComboTier.Tier5;
        }

        return ComboTier.Hidden;
    }

    private static Color GetNumberColor(ComboTier tier)
    {
        if (tier == ComboTier.Tier20)
        {
            return Tier20NumberColor;
        }

        if (tier == ComboTier.Tier30)
        {
            return Tier30NumberColor;
        }

        return WhiteTextColor;
    }

    private void KillTweens()
    {
        KillNumberAndShakeTweens();
        KillRootSequence();
    }

    private void KillNumberAndShakeTweens()
    {
        if (numberPunchTween != null && numberPunchTween.IsActive())
        {
            numberPunchTween.Kill();
        }

        if (shakeTween != null && shakeTween.IsActive())
        {
            shakeTween.Kill();
        }

        ResetTextTransform(singleDigitNumberText);
        ResetTextTransform(doubleDigitNumberText);
        ResetTextTransform(thirtyPlusNumberText);
    }

    private void KillRootSequence()
    {
        if (rootSequence != null && rootSequence.IsActive())
        {
            rootSequence.Kill();
        }
    }

    private void UpdateActiveTexts(ComboTier tier)
    {
        bool useSingleDigit = tier == ComboTier.Tier5;
        bool useDoubleDigit = tier == ComboTier.Tier10 || tier == ComboTier.Tier20;
        bool useThirtyPlusNumber = tier == ComboTier.Tier30;
        bool useThirtyPlusLabel = tier == ComboTier.Tier30;

        SetBackgroundActive(true);
        SetTextActive(singleDigitNumberText, useSingleDigit);
        SetTextActive(singleDigitComboText, useSingleDigit);
        SetTextActive(doubleDigitNumberText, useDoubleDigit);
        SetTextActive(doubleDigitComboText, useDoubleDigit && !useThirtyPlusLabel);
        SetTextActive(thirtyPlusNumberText, useThirtyPlusNumber);
        SetTextActive(thirtyPlusComboText, useThirtyPlusLabel);

        if (singleDigitNumberText != null)
        {
            singleDigitNumberText.text = currentCombo.ToString();
            singleDigitNumberText.color = WhiteTextColor;
        }

        if (singleDigitComboText != null)
        {
            singleDigitComboText.text = "COMBO";
            singleDigitComboText.color = WhiteTextColor;
        }

        if (doubleDigitNumberText != null)
        {
            doubleDigitNumberText.text = currentCombo.ToString();
            doubleDigitNumberText.color = GetNumberColor(tier);
        }

        if (thirtyPlusNumberText != null)
        {
            thirtyPlusNumberText.text = "30+";
            thirtyPlusNumberText.color = GetNumberColor(tier);
        }

        if (doubleDigitComboText != null)
        {
            doubleDigitComboText.text = tier == ComboTier.Tier20 ? "COMBO!!!" : "COMBO!";
            doubleDigitComboText.color = WhiteTextColor;
            doubleDigitComboText.fontSize = tier == ComboTier.Tier20 ? tier20ComboFontSize : doubleDigitComboFontSize;
        }

        if (thirtyPlusComboText != null)
        {
            thirtyPlusComboText.text = "COMBO!!!!!";
            thirtyPlusComboText.color = WhiteTextColor;
        }
    }

    private TMP_Text GetActiveNumberText(ComboTier tier)
    {
        if (tier == ComboTier.Tier5)
        {
            return singleDigitNumberText;
        }

        if (tier == ComboTier.Tier10 || tier == ComboTier.Tier20)
        {
            return doubleDigitNumberText;
        }

        if (tier == ComboTier.Tier30)
        {
            return thirtyPlusNumberText;
        }

        return null;
    }

    private void SetTextActive(TMP_Text textComponent, bool value)
    {
        if (textComponent != null)
        {
            textComponent.gameObject.SetActive(value);
        }
    }

    private void SetBackgroundActive(bool value)
    {
        if (comboBackgroundImage != null)
        {
            comboBackgroundImage.gameObject.SetActive(value);
        }
    }

    private void SetAllVisualsActive(bool value)
    {
        SetBackgroundActive(value);
        SetTextActive(singleDigitNumberText, value);
        SetTextActive(singleDigitComboText, value);
        SetTextActive(doubleDigitNumberText, value);
        SetTextActive(doubleDigitComboText, value);
        SetTextActive(thirtyPlusNumberText, value);
        SetTextActive(thirtyPlusComboText, value);
    }

    private Vector2 GetHiddenAnchoredPosition()
    {
        return new Vector2(visibleAnchoredPosition.x + hiddenOffsetX, visibleAnchoredPosition.y);
    }

    private void ResetTextTransform(TMP_Text textComponent)
    {
        if (textComponent == null)
        {
            return;
        }

        RectTransform textRect = textComponent.rectTransform;
        if (textRect != null)
        {
            textRect.localScale = Vector3.one;
            textRect.localRotation = GetBaseRotation(textComponent);
        }
    }

    private Quaternion CaptureTextRotation(TMP_Text textComponent)
    {
        if (textComponent == null || textComponent.rectTransform == null)
        {
            return Quaternion.identity;
        }

        return textComponent.rectTransform.localRotation;
    }

    private Quaternion GetBaseRotation(TMP_Text textComponent)
    {
        if (textComponent == singleDigitNumberText)
        {
            return singleDigitNumberBaseRotation;
        }

        if (textComponent == doubleDigitNumberText)
        {
            return doubleDigitNumberBaseRotation;
        }

        if (textComponent == thirtyPlusNumberText)
        {
            return thirtyPlusNumberBaseRotation;
        }

        return Quaternion.identity;
    }
}
