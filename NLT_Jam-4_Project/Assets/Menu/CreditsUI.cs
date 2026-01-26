using UnityEngine;
using System.Collections;

public class CreditsUI : MonoBehaviour
{
    public GameObject creditsPanel;
    public float creditsDuration = 5f; // Tempo que os créditos ficam visíveis

    private Coroutine creditsCoroutine;

    public void ShowCredits()
    {
        // Se já tiver rodando uma corrotina, para ela
        if (creditsCoroutine != null)
        {
            StopCoroutine(creditsCoroutine);
        }

        creditsPanel.SetActive(true);
        creditsCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(creditsDuration);
        creditsPanel.SetActive(false);
        creditsCoroutine = null;
    }

    public void HideCredits()
    {
        if (creditsCoroutine != null)
        {
            StopCoroutine(creditsCoroutine);
            creditsCoroutine = null;
        }

        creditsPanel.SetActive(false);
    }
}