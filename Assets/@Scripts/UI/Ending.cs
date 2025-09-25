using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public Image logoImage;
    public float animationDuration = 3f;
    public float moveDistance = 100f;


    public float fadeDuration = 2f;

    private Vector3 initialPosition;
    private Color initialColor;
    private Color targetColor;

    void Awake()
    {
        if (logoImage == null)
        {
            logoImage = GetComponent<Image>();
        }

        initialPosition = logoImage.rectTransform.localPosition;
        initialColor = logoImage.color;
        targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);

    }

    public void StartAnimation(Tilemap currentStageTilemap)
    {
        logoImage.rectTransform.localPosition = initialPosition - new Vector3(0, moveDistance, 0);
        logoImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        StartCoroutine(AnimateLogo(currentStageTilemap));
    }

    private IEnumerator AnimateLogo(Tilemap tilemapToFade)
    {
        yield return new WaitForSeconds(1.5f);

        float elapsedTime = 0f;

        Vector3 startPosition = logoImage.rectTransform.localPosition;
        Color startColor = logoImage.color;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            t = t * t * (3f - 2f * t);

            logoImage.rectTransform.localPosition = Vector3.Lerp(startPosition, initialPosition, t);
            logoImage.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        logoImage.rectTransform.localPosition = initialPosition;
        logoImage.color = targetColor;
        if (tilemapToFade != null)
        {
            StartCoroutine(FadeOutTilemap(tilemapToFade));
        }
        else
        {
            StartCoroutine(RestartAfterDelay());
        }
    }
    private IEnumerator FadeOutTilemap(Tilemap tilemapToFade)
    {
        float elapsedTime = 0f;
        Color startColor = tilemapToFade.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            tilemapToFade.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        tilemapToFade.color = targetColor;

        StartCoroutine(RestartAfterDelay());
    }
    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.RestartGame();
    }
}
