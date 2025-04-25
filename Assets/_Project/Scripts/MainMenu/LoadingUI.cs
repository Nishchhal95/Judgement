using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    private const string DEFAULT_TEXT = "Connecting";
    
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_Text loadingText;

    private void OnEnable()
    {
        SetText(new StringBuilder(DEFAULT_TEXT));
        StartCoroutine(AnimateTextRoutine());
        StartCoroutine(AnimateBackgroundRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetText(StringBuilder text)
    {
        text.Append("...");
        loadingText.SetText(text);
    }

    private IEnumerator AnimateTextRoutine()
    {
        string loadingTextString = loadingText.text;
        if (string.IsNullOrEmpty(loadingTextString))
        {
            yield break;
        }

        StringBuilder finalText = new StringBuilder();
        StringBuilder[] parts = new StringBuilder[2];
        foreach (var character in loadingTextString)
        {
            if (character == '.')
            {
                parts[1] ??= new StringBuilder();
                parts[1].Append(character);
                continue;
            }

            parts[0] ??= new StringBuilder();
            parts[0].Append(character);
            finalText.Append(character);
        }

        while (gameObject.activeSelf)
        {
            loadingText.SetText(finalText);
            for (int i = 0; i < parts[1].Length; i++)
            {
                yield return new WaitForSecondsRealtime(.5f);
                finalText.Append(parts[1][i]);
                loadingText.SetText(finalText);
            }

            yield return new WaitForSecondsRealtime(.5f);
            finalText.Clear();
            for (int i = 0; i < parts[0].Length; i++)
            {
                finalText.Append(parts[0][i]);
            }
        }
    }
    
    private IEnumerator AnimateBackgroundRoutine()
    {
        Color startColor = Color.yellow;

        float duration = 5f;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            backgroundImage.color = Color.Lerp(startColor, Color.red, timeElapsed / duration);
            yield return null;
        }
    }
}
