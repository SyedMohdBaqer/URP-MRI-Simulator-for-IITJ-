using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2f;
    public Color fadeColor = new Color(0, 0, 0, 1); // Default black
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        if (fadeOnStart)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        StartCoroutine(FadeRoutine(1, 0)); // Initiates fade from fully transparent to opaque
    }

    public void FadeOut()
    {
        rend.enabled = true; // Ensure the Renderer is enabled before starting the fade out
        StartCoroutine(FadeRoutine(0, 1)); // Initiates fade from opaque to fully transparent
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            rend.material.SetColor("_Color", newColor);
            timer += Time.deltaTime;
            yield return null;
        }

        Color finalColor = fadeColor;
        finalColor.a = alphaOut;
        rend.material.SetColor("_Color", finalColor);

        if (alphaOut == 0)
        {
            rend.enabled = false; // Disable the Renderer if faded to fully transparent
        }
    }
}
