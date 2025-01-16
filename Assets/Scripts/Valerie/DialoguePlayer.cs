using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using System.Linq;
using System.Collections.Generic;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private string FullTextToRead;
    [SerializeField] private List<string> TextSentances;
    [SerializeField] private float CharDelay; //Cannot be 0. Default to 0.25f.
    [SerializeField] private float PeriodDelay; //Default to 1.0f
    [SerializeField] private float CommaDelay; //Default to 0.5f
    [SerializeField] private float EndTextDelay; //Default to 3.0f

    [SerializeField] private TextMeshProUGUI DialogueDisplay;

    [SerializeField] private int ActiveSentanceIndex;
    [SerializeField] private bool AutoPlay;
    [SerializeField] private float AutoP_SentanceDelay; //default to 3.0f

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AttemptDefaultDelays();
        
        FullTextToRead = GetTxtString("BSRG_DialogueIntro");
        CallNextSentance();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !AutoPlay)
        {
            CallNextSentance();
        }
    }
    private void CallNextSentance()
    {
        ActiveSentanceIndex += 1;
        StartCoroutine(PlayDialogue(ActiveSentanceIndex));
    }
    public void AttemptDefaultDelays()
    {
        if (CharDelay <= 0.0f)
        {
            CharDelay = 0.25f;
        }
        if (PeriodDelay <= 0.0f)
        {
            PeriodDelay = 1.0f;
        }
        if (CommaDelay <= 0.0f)
        {
            CommaDelay = 0.5f;
        }
        if (AutoP_SentanceDelay <= 0.0f)
        {
            AutoP_SentanceDelay = 3.0f;
        }
        if (EndTextDelay <= 0.0f)
        {
            EndTextDelay = 3.0f;
        }
    }

    public string GetTxtString(string txtFileName) //The .txt file has to be in the dialogue resource folder.
    {
        string docText = Resources.Load("DialogueTxt/" + txtFileName).ToString();

        string sentance = string.Empty;
        for (int i = 0; i < docText.Length; i++)
        {
            
            string letter = docText[i].ToString();
            if (letter != "/")
            {
                sentance += letter;
            }
            else
            {
                Debug.Log(sentance);
                TextSentances.Add(sentance);
                sentance = string.Empty;
                i += 2;
                
            }

        }
        return docText;

    }
   
    IEnumerator PlayDialogue(int sentanceIndex)
    {
        
        DialogueDisplay.text = string.Empty;
        Debug.Log("Cleared Old Dialogue");

        if (sentanceIndex >= TextSentances.Count())
        {
            Debug.Log("Couldnt start dialogue. No valid sentance in index " + sentanceIndex);
            yield break;
        }
        Debug.Log("Started New Dialogue");
        string sentance = TextSentances[sentanceIndex];
        char[] textChars = sentance.ToCharArray();
        float delayTime = new float();
        for (int i = 0; i < textChars.Length; i++)
        {
            DialogueDisplay.text += textChars[i];
            string letter = textChars[i].ToString();
            switch (letter)
            {
                case ".":
                    delayTime = PeriodDelay;
                    break;
                case ",":
                    delayTime = CommaDelay;
                    break;
                case "@":
                    //enter game scene
                    DialogueDisplay.text = DialogueDisplay.text.Remove(DialogueDisplay.text.Length-1, 1);
                    Debug.Log("Reached end of text document.");
                    delayTime = EndTextDelay;
                    ActiveSentanceIndex = -1;

                    yield return new WaitForSeconds(delayTime);

                    DialogueDisplay.text = string.Empty;
                    yield break;
                    

                default:
                    delayTime = CharDelay;
                    break;
            }

            //if ()
            yield return new WaitForSeconds(delayTime);
        }

        Debug.Log("Ended Dialogue");
        if (AutoPlay)
        {
            Debug.Log("AutoPlay is enabled. Starting next dialogue in " + AutoP_SentanceDelay + " seconds.");
            yield return new WaitForSeconds(AutoP_SentanceDelay);
            CallNextSentance();
        }

        yield return null;
    }
}
