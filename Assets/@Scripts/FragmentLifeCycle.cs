using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentLifeCycle : MonoBehaviour
{
    public float fadeDuration = 1f;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            return;
        }

        StartCoroutine(FadeOutAndDestroy());
    }
    private IEnumerator FadeOutAndDestroy()
    {
        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;
        while (elapsedTime < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
