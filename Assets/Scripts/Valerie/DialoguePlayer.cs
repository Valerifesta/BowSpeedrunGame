using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Audio;

public class DialoguePlayer : MonoBehaviour
{
    private string _fullTextToRead;
    [SerializeField] private List<string> TextSentances;
    [SerializeField] private float CharDelay; //Cannot be 0. Default to 0.25f.
    [SerializeField] private float PeriodDelay; //Default to 1.0f
    [SerializeField] private float CommaDelay; //Default to 0.5f
    [SerializeField] private float EndTextDelay; //Default to 3.0f

    [SerializeField] private TextMeshProUGUI DialogueDisplay;

    [SerializeField] private int ActiveSentanceIndex;
    [SerializeField] private bool AutoPlay;
    [SerializeField] private float AutoP_SentanceDelay; //default to 3.0f
    [SerializeField] private AudioSource source;
    //[SerializeField] private AudioClip dialogueVoice;
    [SerializeField] private AudioClip[] _dialogueVoiceOrder;

    [SerializeField] private string currentTextDoc;
    [SerializeField] private string[] textDocsOrder;
    private int currentDocIndex;
    private bool isReadingDoc;

    [SerializeField] TutorialScript tutorial;

    [SerializeField] private bool clearTextBeforeNext;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (source == null)
        {
            source = GetComponent<AudioSource>();
        }
        AttemptDefaultDelays();
        currentDocIndex = -1;
        //FullTextToRead = GetTxtString("BSRG_DialogueIntro");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !AutoPlay)
        {
            CallNextSentance();
        }
        if (Input.GetKeyDown(KeyCode.N) && !isReadingDoc)
        {
            ReadNextDoc();
        }
    }
    public void SetTextColor(Color color)
    {
        DialogueDisplay.color = color;
    }
    public void ReadNextDoc(float delay = 0)
    {
        if (!clearTextBeforeNext)
        {
            DialogueDisplay.text = string.Empty; //Visually removes text
        }
        isReadingDoc = true;
        currentDocIndex += 1;
        ActiveSentanceIndex = -1;
        if (currentDocIndex < textDocsOrder.Length)
        {
            _fullTextToRead = GetTxtString(textDocsOrder[currentDocIndex]);
            currentTextDoc = textDocsOrder[currentDocIndex];
            CallNextSentance(delay);
        }
        else
        {
            Debug.Log("No dialogue docs left.");
        }
    }
    public void CallNextSentance(float delay = 0)
    {
        ActiveSentanceIndex += 1;
        StartCoroutine(PlayDialogue(ActiveSentanceIndex, delay));
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
   
    IEnumerator PlayDialogue(int sentanceIndex, float delay)
    {
        if (delay > 0)
        {
            Debug.Log("Delayed diaogue with " + delay + " seconds");
            yield return new WaitForSeconds(delay);
            //yield return null;
        }
        Debug.Log("Is Past delay now");
        
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
            if (!string.IsNullOrWhiteSpace(letter) && letter != "@")
            {
                source.PlayOneShot(_dialogueVoiceOrder[currentDocIndex]);
            }
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
                    
                    //delayTime = EndTextDelay;
                    //ActiveSentanceIndex = -1;
                    TextSentances.Clear();

                    //yield return new WaitForSeconds(delayTime);

                    isReadingDoc = false;

                    break;
                   // break;
                    


                default:
                    delayTime = CharDelay;
                    break;
            }

            //if ()
            yield return new WaitForSeconds(delayTime);
        }

        Debug.Log("Ended Dialogue");
        if (isReadingDoc)
        {
            if (AutoPlay)
            {
                Debug.Log("AutoPlay is enabled. Starting next dialogue in " + AutoP_SentanceDelay + " seconds.");
                yield return new WaitForSeconds(AutoP_SentanceDelay);
                CallNextSentance();


            }
        }
        else
        {
            Debug.Log("Reached end of text document. Did not call next sentance");

            Debug.Log("The current doc index is " + currentDocIndex + ", which is " + currentTextDoc);
            Debug.Log("Delaying with endTextDelay");
            yield return new WaitForSeconds(EndTextDelay);
            if (clearTextBeforeNext)
            {
                DialogueDisplay.text = string.Empty; //Visually removes text
            }

            switch (currentDocIndex)
            {
                case 0:
                    Debug.Log("Started to await calibratiobn");
                    tutorial.AwaitingCalibration = true;
                    break;

                case 1:
                    Debug.Log("Attempted to start room transition");
                    tutorial.StartRoomTransition();
                    EndTextDelay = 2;
                    break;

                case 2://Finished room transition
                    EndTextDelay = 0.5f;
                    clearTextBeforeNext = false;
                    
                    ReadNextDoc();
                    break;
                case 3://look around. "See if you can find us"
                    //FindFirstObjectByType<TutStare>().IsAwaitingStare = true;
                    tutorial.ActivateStareObj();
                    
                    break;
                case 4://Spotted. Activate platforms
                    tutorial.StartTutorialStep(currentDocIndex);
                    clearTextBeforeNext = true;
                    break;
                case 5://Reached green platform. Spawns enemeies.
                    tutorial.StartTutorialStep(currentDocIndex);
                    clearTextBeforeNext = false;
                    //Cross out toggle icons

                    break;
                case 6://Got shot. Let player use bow again
                    //tutorial.ToggleBowInputs();
                    tutorial.StartTutorialStep(currentDocIndex);
                    //indicate that bow cant be shot, nor arrow toggled
                    break;

                case 7: //Shot last enemy. Moving in train
                    
                    break;

            }

        }
        
        

        yield return null;
    }
}
