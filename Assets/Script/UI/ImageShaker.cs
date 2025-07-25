using UnityEngine;
using System.Collections;

public class ImageShaker : MonoBehaviour
{
    private Vector3 originalPos;
    private Coroutine shakeRoutine;

    // public method to call
    public void Shake(float intensity = 10f, float duration = 0.3f)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutine(intensity, duration));
    }

    private IEnumerator ShakeRoutine(float intensity, float duration)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * intensity;
            float offsetY = Random.Range(-1f, 1f) * intensity;

            rectTransform.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPos;
        shakeRoutine = null;
    }
}
