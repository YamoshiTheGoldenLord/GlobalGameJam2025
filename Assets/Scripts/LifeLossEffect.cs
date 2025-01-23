using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeLossEffect : MonoBehaviour
{
    public Image lifeLossImage;
    public float animationDuration = 1.5f;

    public void TriggerLifeLoss()
    {
        StartCoroutine(StretchImage());
    }

    private IEnumerator StretchImage()
    {
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationDuration;

            lifeLossImage.rectTransform.localScale = new Vector3(1, Mathf.Lerp(0, 1, progress), 1);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        lifeLossImage.rectTransform.localScale = new Vector3(1, 0, 1);
    }
}
