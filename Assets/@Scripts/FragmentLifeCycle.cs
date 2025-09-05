using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentLifeCycle : MonoBehaviour
{
    public bool useFadeEffect = true;
    public float fadeDuration = 1f;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        if (useFadeEffect)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                return;
            }
        }
        if (useFadeEffect)
        {
            StartCoroutine(FadeOutAndDestroy());
        }
        else
        {
            StartCoroutine(DestroyAfterTime());
        }

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
    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(fadeDuration);

        Destroy(gameObject);
    }
}
