using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentLifeCycle : MonoBehaviour
{
    public bool useFadeEffect = true;
    public float fadeDuration = 1f;
    private SpriteRenderer spriteRenderer;

    private TimeAffected _timeAffected;

    private void Start()
    {
        _timeAffected = GetComponent<TimeAffected>();

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
            float timeScale = (_timeAffected != null) ? _timeAffected.currentTimeScale : 1.0f;
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            elapsedTime += Time.deltaTime * timeScale;
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
