using UnityEngine;
using System.Collections;

public class CoinShine : MonoBehaviour
{
    [Header("Shine settings")]

    public float shineInterval = 3f;
    public float shineDuration = 0.5f;
    public Color shineColor = Color.yellow;
    public float emissionIntensity = 3f;

    private Material coinMaterial;
    private Color baseEmissionColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            coinMaterial = renderer.material;
            baseEmissionColor = coinMaterial.GetColor("_EmissionColor");
        }

        StartCoroutine(ShineLoop());
    }

    private IEnumerator ShineLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(shineInterval);

            StartCoroutine(ShineOnce());
        }
    }

    private IEnumerator ShineOnce()
    {
        float timer = 0f;

        //fading in

        while (timer < shineDuration / 2)
        {
            timer += Time.deltaTime;
            float t = timer / (shineDuration / 2);
            SetEmission(Color.Lerp(baseEmissionColor, shineColor * emissionIntensity, t));
            yield return null;
        }

        //fading out
        timer = 0f;
        while (timer < shineDuration / 2)
        {
            timer += Time.deltaTime;
            float t = timer / (shineDuration / 2);
            SetEmission(Color.Lerp(shineColor * emissionIntensity, baseEmissionColor, t));
            yield return null;
        }
    }

    private void SetEmission(Color color)
    {
        if (coinMaterial != null)
        {
            coinMaterial.SetColor("_EmissionColor", color);
            DynamicGI.SetEmissive(GetComponent<Renderer>(), color);
        }

    }
}

