using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Gameplay.Event;

namespace ATBMI.Dialogue
{
    public class DialogueEmojiHandler : MonoBehaviour
    {
        [SerializeField] private Animator anim;

        private Dictionary<string, Transform> _npcEmojiTargets = new(); // cache the npc emoji target for each npcs
        private AnimatorStateInfo _stateInfo;
        private Transform _emojiPos;
        private Vector3 _screenPosition;

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
            if (_screenPosition != null && _emojiPos != null && anim.gameObject.activeSelf)
            {
                gameObject.transform.position = Camera.main.WorldToScreenPoint(_emojiPos.position);
            }
        }

        private void RegisterNPCEmojiTarget(string npcName, Transform emojiTarget)
        {
            if (!_npcEmojiTargets.ContainsKey(npcName))
            {
                _npcEmojiTargets[npcName] = emojiTarget;
            }
        }
        
        private void UnregisterNPCEmojiTarget()
        {
            _npcEmojiTargets.Clear();
        }

        private void UpdateDialogueEmojiPos(string tagValue)
        {
            if (_npcEmojiTargets.TryGetValue(tagValue, out var targetPos))
            {
                _emojiPos = targetPos;
                _screenPosition = Camera.main.WorldToScreenPoint(targetPos.position);
                gameObject.transform.position = new Vector2(_screenPosition.x, _screenPosition.y);
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

            _stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(_stateInfo.length);

            anim.gameObject.SetActive(false);
        }

    }
}
