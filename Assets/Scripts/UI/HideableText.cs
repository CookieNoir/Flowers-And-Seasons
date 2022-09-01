using UnityEngine;
using UnityEngine.UI;

public class HideableText : MonoBehaviour
{
    [SerializeField] private Text _movesText;

    public void Hide() 
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetText(string text)
    {
        _movesText.text = text;
    }
}