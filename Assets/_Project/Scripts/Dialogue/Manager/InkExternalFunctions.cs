using UnityEngine;
using Ink.Runtime;

namespace ATBMI
{
    public class InkExternalFunctions
    {
        public void Bind(Story story, Animator emoteAnimator)
        {
            story.BindExternalFunction("PlayEmote", (string emoteName) => PlayEmote(emoteName, emoteAnimator));
        }

        public void Unbind(Story story)
        {
            story.UnbindExternalFunction("PlayEmote");
        }

        public void PlayEmote(string emoteName, Animator emoteAnimator)
        {
            if (emoteAnimator != null)
            {
                emoteAnimator.Play(emoteName);
            }
            else
            {
                Debug.LogWarning("Tried to play emote, but emote animator was "
                    + "not initialized when entering dialogue mode.");
            }
        }
    }
}
