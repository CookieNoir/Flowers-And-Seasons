using UnityEngine;

public class FadeableWindow : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField, Min(0.25f)] private float _fadingSpeed = 1f;
    private bool _isFading = true;

    private void Awake()
    {
        enabled = false;
        _canvasGroup.interactable = false;
    }

    public void Appear()
    {
        _canvasGroup.gameObject.SetActive(true);
        _isFading = false;
        _canvasGroup.interactable = false;
        enabled = true;
    }

    public void Fade()
    {
        _canvasGroup.gameObject.SetActive(true);
        _isFading = true;
        _canvasGroup.interactable = false;
        enabled = true;
    }

    protected virtual void OnStopAppearing()
    {
    }

    public void Show()
    {
        enabled = false;
        _canvasGroup.interactable = true;
        _canvasGroup.alpha = 1f;
        _canvasGroup.gameObject.SetActive(true);
    }

    public void Hide()
    {
        enabled = false;
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0f;
        _canvasGroup.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isFading)
        {
            _canvasGroup.alpha -= Time.deltaTime * _fadingSpeed;
            if (_canvasGroup.alpha == 0f)
            {
                enabled = false;
                _canvasGroup.gameObject.SetActive(false);
            }
        }
        else
        {
            _canvasGroup.alpha += Time.deltaTime * _fadingSpeed;
            if (_canvasGroup.alpha == 1f)
            {
                enabled = false;
                _canvasGroup.interactable = true;
                OnStopAppearing();
            }
        }
    }
}