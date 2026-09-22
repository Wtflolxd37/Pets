using DG.Tweening;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasRectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool _hideStart;

    [Header("Animation settings")]
    [SerializeField] private float _showDuration = 0.5f;
    [SerializeField] private float _hideDuration = 0.5f;

    [SerializeField] private Ease _showEase;
    [SerializeField] private Ease _hideEase;
    void Start()
    {
        Initialized();
    }
    public void Initialized()
    {
        if (_hideStart)
        {
            Hide();
        }
    }
    #region text
    [Button]

    public virtual void Show(bool instance = false)
    {
        if (instance)
        {
            _canvasRectTransform.gameObject.SetActive(true);
        }
        else
        {
            _canvasRectTransform.gameObject.SetActive(true);
            RectTransform rectTransform = _canvasGroup.GetComponent<RectTransform>();
            rectTransform.DOScale(Vector3.one, _showDuration).SetEase(_showEase);
        }

    }
    #endregion
    #region text 
    [Button]
    public virtual void Hide(bool instance = false)
    {
        if (instance)
        {
            _canvasRectTransform.gameObject.SetActive(false);
        }
        else
        {
            RectTransform rectTransform = _canvasGroup.GetComponent<RectTransform>();
            rectTransform.DOScale(Vector3.zero, _hideDuration).SetEase(_hideEase).OnComplete(() => _canvasRectTransform.gameObject.SetActive(false));
        }
    }
    #endregion
}