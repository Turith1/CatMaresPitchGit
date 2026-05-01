using UnityEngine;
using TMPro;
using System.Collections;
public class Dialogos : MonoBehaviour
{
    [SerializeField] private GameObject caixadeDialogo;
    [SerializeField] private TMP_Text fonteTexto;
    private EfeitoDigitacao efeitoDigitacao;
    [SerializeField] private DialogueObj newDialogue;

    private void Start()
    {
        efeitoDigitacao = GetComponent<EfeitoDigitacao>();
        FecharDialogo();
        Mostrardialogo(newDialogue);
    }

    public void Mostrardialogo(DialogueObj dialogueObj)
    {
        caixadeDialogo.SetActive(true);
        StartCoroutine(StepTroughDialogue(dialogueObj));
    }

    private IEnumerator StepTroughDialogue(DialogueObj dialogueObj)
    {

        foreach(string dialogue in dialogueObj.Dialogue)
        {
            yield return RunTypingEffect(dialogue);
            
            fonteTexto.text = dialogue;

            yield return null;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Q));
        }
        FecharDialogo();
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        efeitoDigitacao.CorrerTexto(dialogue, fonteTexto);
        while (efeitoDigitacao.IsRunning)
        {
            yield return null;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                efeitoDigitacao.Stop();
            }
        }
    }
    private void FecharDialogo()
    {
        caixadeDialogo.SetActive(false);
        fonteTexto.text = string.Empty;
    }
}
