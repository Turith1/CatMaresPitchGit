using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EfeitoDigitacao : MonoBehaviour
{
    public bool IsRunning {  get; private set; }

    [SerializeField] float velocidadeDigitacao = 50f;

    private Coroutine corroutinedigitacao;
    public void CorrerTexto(string textToType, TMP_Text fonteTexto)
    {
      corroutinedigitacao = StartCoroutine(DigitarTexto(textToType, fonteTexto));
    }

    public void Stop()
    {
        StopCoroutine(corroutinedigitacao);
        IsRunning = false;
    }
    private IEnumerator DigitarTexto(string textToType, TMP_Text fonteTexto)
    {
        IsRunning = true;
        fonteTexto.text = string.Empty;
        float t = 0f;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * velocidadeDigitacao;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            fonteTexto.text = textToType.Substring(0, charIndex);
            yield return null;
        }
        IsRunning = false;
        
    }
}
