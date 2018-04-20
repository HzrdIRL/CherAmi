using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewportBlocker : MonoBehaviour {

    public CanvasGroup viewportBlocker;
    public GameObject readyButton, next1Button, next2Button;
    public Text readyUpText;
    public GameObject helpText, rulesText, creditText;
    public float fadeSpeed = 1f;

    void Awake()
    {
        viewportBlocker.alpha = 1;
        viewportBlocker.blocksRaycasts = true;
    }
    public void AreYouReady()
    {
        ReadyUp("Player " + ((int)GameManager.controller.turn + 1) + ", are you ready?", true);
    }
    public void ReadyUp(string text, bool isButtonEnabled)
    {
        readyButton.SetActive(isButtonEnabled);
        readyUpText.gameObject.SetActive(true);
        creditText.SetActive(false);
        helpText.SetActive(false);
        rulesText.SetActive(false);
        readyUpText.text = text;
        
        BlockViewport();
    }

    public void Credits()
    {
        helpText.SetActive(false);
        rulesText.SetActive(false);
        readyUpText.gameObject.SetActive(false);
        creditText.SetActive(true);
        BlockViewport();
    }
    public void UnblockViewport()
    {
        viewportBlocker.blocksRaycasts = false;
        StartCoroutine(CrossfadeCanvasAlpha(1f, 0f, fadeSpeed));
    }

    public void Rules()
    {
        readyUpText.gameObject.SetActive(false);
        rulesText.SetActive(true);
        creditText.SetActive(false);
        helpText.SetActive(false);
        viewportBlocker.blocksRaycasts = true;
        StartCoroutine(CrossfadeCanvasAlpha(0f, 1f, fadeSpeed));
    }

    public void HowToPlay()
    {
        readyUpText.gameObject.SetActive(false);
        creditText.SetActive(false);
        rulesText.SetActive(false);
        helpText.SetActive(true);
        BlockViewport();
    }

    public void BlockViewport()
    {
        viewportBlocker.blocksRaycasts = true;
        StartCoroutine(CrossfadeCanvasAlpha(0f, 1f, fadeSpeed));
    }

    IEnumerator CrossfadeCanvasAlpha(float startValue, float endValue, float fadeSpeed)
    {
        while (viewportBlocker.alpha != endValue)
        {
            if(startValue > endValue)
            {
                viewportBlocker.alpha -= Time.deltaTime * fadeSpeed;
            }else
            {
                viewportBlocker.alpha += Time.deltaTime * fadeSpeed;
            }
            yield return null;
        }
    }
}
