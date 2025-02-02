using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Ink.Runtime;
using ATBMI.Gameplay.Event;
using ATBMI.Gameplay.Handler;
using ATBMI.Entities.Player;

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
        [SerializeField] private TextMeshProUGUI dialogueName;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private GameObject continuePin;

        [Header("Choices UI")]
        [SerializeField] private GameObject dialogueChoicesContainer;
        private GameObject[] choices;
        private TextMeshProUGUI[] choicesText;

        public bool IsDialoguePlaying { get; private set; }

        private Coroutine displayLineCoroutine;
        private Story currentStory;
        private List<Choice> currentChoices;
        private bool canContinueToNextLine;
        private bool isAnyChoices;
        private bool _isSkippedDialogue;
        
        private const string SPEAKER_TAG = "speaker";
        private const string EXPRESSION_TAG = "expression";

        private InkExternalFunctions inkExternalFunctions;

        public static DialogueManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

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
        }

        private void Update()
        {
            if (!IsDialoguePlaying) return;

            if (GameInputHandler.Instance.IsTapInteract)
            {
                if (_isSkippedDialogue)
                    _isSkippedDialogue = false;
                
                if (canContinueToNextLine)
                {
                    if (currentStory.currentChoices.Count == 0 && !isAnyChoices)
                    {
                        ContinueStory();
                    }
                    else if (isAnyChoices)
                    {
                        DisplayChoicesWithInput();
                    }
                }
            }
        }

        public void EnterDialogueMode(TextAsset inkJSON, Animator emoteAnimator)
        {
            playerController.StopMovement();

            Debug.Log("Dialogue asset = " + inkJSON.name);

            currentStory = new Story(inkJSON.text);
            IsDialoguePlaying = true;
            dialoguePin.SetActive(true);

            inkExternalFunctions.Bind(currentStory, emoteAnimator);

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
            else
            {
                DisplayChoices();
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

            DialogEvents.AdjustDialogueUISizeEvent(line.Length);

            // set the dialogue text to full line, but set the visible character to 0
            dialogueText.text = line;
            dialogueText.maxVisibleCharacters = 0;

            canContinueToNextLine = false;
            bool isAddingRichTextTag = false;
            bool canSkip = false;

            // Process tags before displaying the line
                    HandleTags(currentStory.currentTags);

            // display char in a line 1 by 1
            foreach (char letter in line.ToCharArray())
            {
                yield return null;

                // if player pressed submit button displayed the line immediately
                if (canSkip && _isSkippedDialogue)
                {
                    Debug.LogWarning("submit button pressed,  the line display immediately!");
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
                    canSkip = true;
                }
            }

            continuePin.SetActive(true);
            DisplayChoices();

            canContinueToNextLine = true;
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

            DialogEvents.UpdateDialogueChoicesUIPosEvent("Player");
            DialogEvents.StopDialogueAnimEvent();

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

            DialogEvents.AdjustDialogueChoicesUISizeEvent(maxLength);

            for (int i = index; i < choices.Length; i++)
            {
                choices[i].gameObject.SetActive(false);
            }

            // StartCoroutine(SelectFirstChoice());
            SelectFirstChoice();
        }

        private void SelectFirstChoice()
        {
            // Event System requires we clear it first, then wait for at Least one frame before we set the current selected object.
            // EventSystem.current.SetSelectedGameObject(null);
            // yield return new WaitForEndOfFrame();
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
                            DialogEvents.StopDialogueAnimEvent();
                        }

                        // TODO: change to other method
                        dialogueName.text = tagValue == "Player" ? "Atma" : tagValue;

                        // update dialogue bubble position
                        Debug.Log("Speaker = " + tagValue);
                        DialogEvents.UpdateDialogueUIPosEvent(tagValue);
                        break;
                    case EXPRESSION_TAG:
                        // update animation
                        DialogEvents.PlayDialogueAnimEvent(tagValue);
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

            DialogEvents.StopDialogueAnimEvent();

            playerController.StartMovement();
        }

        public void MakeChoice(int choiceIndex)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);

            // InputManager.GetInstance().RegisterSubmitPressed(); // spesific for this input manager
            ContinueStory();
        }
    }

}

