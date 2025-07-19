using UnityEngine;

public class EndingMaskShrink : MonoBehaviour
{
    [SerializeField] private RectTransform circleMask;
    [SerializeField] private float shrinkDuration = 2f;
    private Vector2 initialSize;

    private void Start()
    {
        initialSize = circleMask.sizeDelta;
        StartCoroutine(ShrinkMask());
    }

    private System.Collections.IEnumerator ShrinkMask()
    {
        float timer = 0f;

        while (timer < shrinkDuration)
        {
            float t = timer / shrinkDuration;
            float scale = Mathf.Lerp(1f, 0f, t); // 1 → 0 축소
            circleMask.sizeDelta = initialSize * scale;

            timer += Time.deltaTime;
            yield return null;
        }

        // 완전히 축소 후 종료 처리
        circleMask.sizeDelta = Vector2.zero;
        Debug.Log("💀 화면 축소 끝! 엔딩 연출 진행...");
    }
}
