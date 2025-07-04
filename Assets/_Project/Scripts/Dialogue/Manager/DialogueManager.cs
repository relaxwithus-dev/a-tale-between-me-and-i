using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Ink.Runtime;
using ATBMI.Audio;
using ATBMI.Gameplay.Event;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerController playerController;
        
        [Header("Params")]
        [SerializeField] private float typingSpeed;

        [Header("Dialogue UI")]
        [SerializeField] private GameObject dialoguePin;
        [SerializeField] private GameObject continuePin;
        [SerializeField] private TextMeshProUGUI dialogueName;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Color[] dialogueColors;

        [Header("Choices UI")]
        [SerializeField] private GameObject dialogueChoicesContainer;
        private GameObject[] choices;
        private TextMeshProUGUI[] choicesText;

        public bool IsDialoguePlaying { get; private set; }

        private string currentSpeakerName;
        private Coroutine displayLineCoroutine;
        private Story currentStory;
        private List<Choice> currentChoices;
        private bool canContinueToNextLine;
        private bool isAnyChoices;
        private bool isSkippedDialogue;
        private bool isDialogueDisplaying;
        // private bool isSequenceIsPlaying;
        // private CutsceneBaseClass cutscene;

        private const string SPEAKER_TAG = "speaker";
        private const string EXPRESSION_TAG = "expression";
        private const string EMOJI_TAG = "emoji";
        private const string COLOR_TAG = "color";

        private InkExternalFunctions inkExternalFunctions;

        public static DialogueManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            inkExternalFunctions = new InkExternalFunctions();
        }

        private void Start()
        {
            // get all of the choices text
            choices = new GameObject[dialogueChoicesContainer.transform.childCount];
            for (int i = 0; i < choices.Length; i++)
            {
                choices[i] = dialogueChoicesContainer.transform.GetChild(i).gameObject;
            }
            choicesText = new TextMeshProUGUI[choices.Length];
            for (int i = 0; i < choicesText.Length; i++)
            {
                choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
            }

            currentChoices = new List<Choice>();

            IsDialoguePlaying = false;
            dialoguePin.SetActive(false);
            continuePin.SetActive(false);
            dialogueChoicesContainer.transform.parent.gameObject.SetActive(false);
            dialogueText.text = "";

            canContinueToNextLine = false;
            isAnyChoices = false;
            isSkippedDialogue = false;
            isDialogueDisplaying = false;
        }

        // private void Update()
        // {
        //     if (!IsDialoguePlaying) return;

        //     if (GameInputHandler.Instance.IsTapSubmit)
        //     {
        //         // if (_isSkippedDialogue)
        //         //     _isSkippedDialogue = false;

        //         if (canContinueToNextLine)
        //         {
        //             if (currentStory.currentChoices.Count == 0 && !isAnyChoices)
        //             {
        //                 ContinueStory();
        //             }
        //             else if (isAnyChoices)
        //             {
        //                 DisplayChoicesWithInput();
        //             }
        //         }
        //     }
        // }

        private void Update()
        {
            if (!IsDialoguePlaying) return;

            if (GameInputHandler.Instance.IsTapSelect)
            {
                if (isDialogueDisplaying)
                {
                    isSkippedDialogue = true;
                }

                if (canContinueToNextLine)
                {
                    ContinueStory();
                }
            }
        }

        public void EnterDialogueMode(TextAsset inkJSON)
        {
            Debug.Log("Dialogue asset = " + inkJSON.name);
            
            currentStory = new Story(inkJSON.text);
            IsDialoguePlaying = true;
            dialoguePin.SetActive(true);
            
            playerController.StopMovement();
            inkExternalFunctions.Bind(currentStory);
            
            ContinueStory();
        }
        
        private void ContinueStory()
        {
            while (currentStory.canContinue)
            {
                string nextLine = currentStory.Continue();

                // Skip processing if the next line is an empty string
                if (!string.IsNullOrWhiteSpace(nextLine))
                {
                    if (displayLineCoroutine != null)
                    {
                        StopCoroutine(displayLineCoroutine);
                    }

                    displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
                    return;
                }
            }

            if (!currentStory.canContinue && currentStory.currentChoices.Count == 0)
            {
                StartCoroutine(ExitDialogueMode());
            }
            else if (!isAnyChoices && currentStory.currentChoices.Count > 0)
            {
                DisplayChoices();
            }
            else if (isAnyChoices && currentStory.currentChoices.Count > 0)
            {
                DisplayChoicesWithInput();
            }
        }
        
        private IEnumerator DisplayLine(string line)
        {
            // if (string.IsNullOrWhiteSpace(line))
            // {
            //     yield break; // Skip displaying the line
            // }

            dialogueChoicesContainer.transform.parent.gameObject.SetActive(false);
            dialoguePin.SetActive(true);

            continuePin.SetActive(false);
            HideChoices();

            DialogueEvents.AdjustDialogueUISizeEvent(line.Length);

            // set the dialogue text to full line, but set the visible character to 0
            dialogueText.text = line;
            dialogueText.maxVisibleCharacters = 0;

            isDialogueDisplaying = true;
            canContinueToNextLine = false;
            bool isAddingRichTextTag = false;
            // bool canSkip = false;

            // Process tags before displaying the line
            HandleTags(currentStory.currentTags);
            
            // display char in a line 1 by 1
            foreach (var letter in line.ToCharArray())
            {
                yield return null;
                AudioManager.Instance.PlayAudio(Musics.SFX_Typing);
                
                // if player pressed submit button displayed the line immediately
                if (isSkippedDialogue)
                {
                    // Debug.LogWarning("submit button pressed,  the line display immediately!");
                    dialogueText.maxVisibleCharacters = line.Length;
                    break;
                }

                // check if rich text tag add, if found, add it without waiting
                if (letter == '<' || isAddingRichTextTag)
                {
                    isAddingRichTextTag = true;
                    // dialogueText.text += letter;
                    if (letter == '>')
                    {
                        isAddingRichTextTag = false;
                    }
                }
                // if not, add the next letter and wait a small of time
                else
                {
                    dialogueText.maxVisibleCharacters++;
                    // dialogueText.text += letter;
                    yield return new WaitForSeconds(typingSpeed);
                    // canSkip = true;
                }
            }
            
            continuePin.SetActive(true);
            DisplayChoices();
            
            canContinueToNextLine = true;
            isSkippedDialogue = false;
            isDialogueDisplaying = false;
        }

        private void HideChoices()
        {
            foreach (GameObject choice in choices)
            {
                choice.SetActive(false);
            }
        }
        
        private void DisplayChoices()
        {
            currentChoices.Clear();
            currentChoices.AddRange(currentStory.currentChoices);

            // defensive check to make sure our UI can support the number of choices coming in
            if (currentChoices.Count > choices.Length)
                Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);

            if (currentChoices.Count > 0)
            {
                isAnyChoices = true;
            }
            else
            {
                isAnyChoices = false;
            }
        }
        
        private void DisplayChoicesWithInput()
        {
            continuePin.SetActive(false);
            dialoguePin.SetActive(false);
            dialogueChoicesContainer.transform.parent.gameObject.SetActive(true);

            DialogueEvents.UpdateDialogueChoicesUIPosEvent();
            DialogueEvents.StopDialogueAnimEvent(currentSpeakerName);

            int index = 0;
            int maxLength = 0;
            // enable and initialize the choices up to the amount of choices for this line of dialogue
            foreach (Choice choice in currentChoices)
            {
                choices[index].gameObject.SetActive(true);
                choicesText[index].text = choice.text;

                int textLength = choicesText[index].text.Length;
                if (textLength > maxLength)
                {
                    maxLength = textLength;
                }

                index++;
            }

            DialogueEvents.AdjustDialogueChoicesUISizeEvent(maxLength);

            for (int i = index; i < choices.Length; i++)
            {
                choices[i].gameObject.SetActive(false);
            }

            StartCoroutine(SelectFirstChoice());
            // SelectFirstChoice();
        }

        private IEnumerator SelectFirstChoice()
        {
            // Event System requires we clear it first, then wait for at Least one frame before we set the current selected object.
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
        }
        
        private void HandleTags(List<string> currentTags)
        {
            foreach (string tag in currentTags)
            {
                // split the key (left side) and the value (right side)
                string[] splitTag = tag.Split(':');
                if (splitTag.Length != 2)
                {
                    Debug.LogError("Tag could not appropriately parsed : " + tag);
                }
                // trim method to clear any white space in the string
                string tagKey = splitTag[0].Trim();
                string tagValue = splitTag[1].Trim();

                // handle the tag
                switch (tagKey)
                {
                    case SPEAKER_TAG:
                        if (dialogueName.text != tagValue)
                        {
                            // Stop animation between speaker
                            DialogueEvents.StopDialogueAnimEvent(currentSpeakerName);
                        }
                        
                        // TODO: change to other method, change the speaker "player" in ink first
                        // if the player name change, change the method
                        currentSpeakerName = tagValue == "Player" ? "Dewa" : tagValue;
                        dialogueName.text = currentSpeakerName;
                        if (tagValue == "Player")
                        {
                            tagValue = "Dewa";
                        }

                        // update dialogue bubble position
                        Debug.Log("Speaker = " + tagValue);
                        DialogueEvents.UpdateDialogueUIPosEvent(tagValue);
                        DialogueEvents.UpdateDialogueEmojiPosEvent(tagValue);
                        break;
                    case EXPRESSION_TAG:
                        // update animation
                        DialogueEvents.PlayDialogueAnimEvent(currentSpeakerName, tagValue);
                        break;
                    case EMOJI_TAG:
                        // update emoji animation
                        DialogueEvents.PlayEmojiAnimEvent(tagValue);
                        break;
                    case COLOR_TAG:
                        var targetColor = GetDialogueColor(tagValue);
                        dialogueText.color = targetColor;
                        break;
                    default:
                        Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                        break;
                }
            }
        }
        
        private IEnumerator ExitDialogueMode()
        {
            yield return new WaitForSeconds(0.2f);
            inkExternalFunctions.Unbind(currentStory);
            
            IsDialoguePlaying = false;
            dialoguePin.SetActive(false);
            dialogueName.text = "";
            dialogueText.text = "";
            dialogueText.color = Color.black;
            
            playerController.StartMovement();
            DialogueEvents.StopDialogueAnimEvent(currentSpeakerName);
            DialogueEvents.OnExitDialogueEvent();
            
            // if (isSequenceIsPlaying)
            // {
            //     cutscene.nextstep(currentstep + 1);
            //     cutscene = null;
            //     currentstep = 0;
            // }
        }

        private Color GetDialogueColor(string color)
        {
            return color switch
            {
                "original" => dialogueColors[0],
                "translate" => dialogueColors[1],
                _ => throw new ArgumentException($"Invalid dialogue tag: {color}", nameof(color))
            };
        }
        
        public void MakeChoice(int choiceIndex)
        {
            // isAnyChoices = false;

            currentStory.ChooseChoiceIndex(choiceIndex);

            Debug.Log(choiceIndex);

            // InputManager.GetInstance().RegisterSubmitPressed(); // spesific for this input manager
            ContinueStory();
        }

        // public void TurnSequenceState(bool isOn, CutsceneBaseClass cutscene, int currentStep)
        // {
        //     isSequenceIsPlaying = isOn;

        //     this.cutscene = cutscene; 
        //     this.currentStep = currentStep;
        // }
    }

}

