using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Gameplay.Event;

namespace ATBMI
{
    public class DialogueEmojiUIController : MonoBehaviour
    {
        [SerializeField] private Animator anim;

        private Dictionary<string, Transform> npcEmojiTargets = new(); // cache the npc emoji target for each npcs
        private AnimatorStateInfo stateInfo;
        private Transform emojiPos;
        private Vector3 screenPosition;

        private void OnEnable()
        {
            DialogueEvents.RegisterNPCEmojiTarget += RegisterNPCEmojiTarget;
            DialogueEvents.UnregisterDialogueEmojiPoint += UnregisterNPCEmojiTarget;
            DialogueEvents.UpdateDialogueEmojiPos += UpdateDialogueEmojiPos;
            DialogueEvents.PlayEmojiAnim += PlayEmojiAnim;
        }

        private void OnDisable()
        {
            DialogueEvents.RegisterNPCEmojiTarget -= RegisterNPCEmojiTarget;
            DialogueEvents.UnregisterDialogueEmojiPoint -= UnregisterNPCEmojiTarget;
            DialogueEvents.UpdateDialogueEmojiPos -= UpdateDialogueEmojiPos;
            DialogueEvents.PlayEmojiAnim -= PlayEmojiAnim;
        }

        private void Start()
        {
            anim.gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            if (screenPosition != null && emojiPos != null && anim.gameObject.activeSelf)
            {
                gameObject.transform.position = Camera.main.WorldToScreenPoint(emojiPos.position);
            }
        }

        private void RegisterNPCEmojiTarget(string npcName, Transform emojiTarget)
        {
            if (!npcEmojiTargets.ContainsKey(npcName))
            {
                npcEmojiTargets[npcName] = emojiTarget;
            }
        }
        
        private void UnregisterNPCEmojiTarget()
        {
            npcEmojiTargets.Clear();
        }

        private void UpdateDialogueEmojiPos(string tagValue)
        {
            if (npcEmojiTargets.TryGetValue(tagValue, out var targetPos))
            {
                emojiPos = targetPos;
                screenPosition = Camera.main.WorldToScreenPoint(targetPos.position);
                gameObject.transform.position = screenPosition;
            }
            else
            {
                Debug.LogError("NPC " + tagValue + " not found in registered targets!");
            }
        }
        
        private void PlayEmojiAnim(string emojiName)
        {
            anim.gameObject.SetActive(true);

            anim.Play(emojiName);
            StartCoroutine(DisableAfterAnimation());
        }

        private IEnumerator DisableAfterAnimation()
        {
            // Wait for a frame to ensure Animator updates the state info
            yield return null;

            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);

            anim.gameObject.SetActive(false);
        }

    }
}
