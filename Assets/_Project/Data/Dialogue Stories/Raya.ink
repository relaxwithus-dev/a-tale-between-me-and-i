EXTERNAL PlayEmote(emoteName)

-> Smile

=== Smile ===
~PlayEmote("A_Emoji_Happy")
We smiled at each other again across the coffee shop. #speaker:Raya #expression:A_Raya_Speak
I had seen her coming in at this same time for over a week now. 
We had spoken a couple of times, but I could not bring myself to talk to her more.
As I looked back down at my coffee, I needed to decide. #speaker:Raya #expression:A_Raya_Happy
* [I decided to go talk to her.]
    "Uh. Hi!" I said, maybe a little too loud, as I approached her. #speaker:Player #expression:A_Atma_Speak
    -> Forgive
* [I gave up for now. Maybe tomorrow.]
    I shook my head to myself and looked away from her and out the window. Today was not the day. #speaker:Player #expression:A_Atma_Speak
    -> Forgive

=== Forgive ===
~PlayEmote("A_Emoji_Exclamation")
Should I really forgive her again? #speaker:Raya #expression:A_Raya_Speak
I thought about the options in front of me as I considered what she told me. #speaker:Raya #expression:A_Raya_Sad
* I forgive her.
    ** She does the same behavior again.
        I just end up hurt again. #speaker:Player #expression:A_Atma_Speak
        -> END
    ** She really does change.
        She does not have another affair and maybe we can save our relationship. #speaker:Player #expression:A_Atma_Speak
        -> END
* I do not forgive her.
    ** I would have to move out.
        I would need to find another apartment. #speaker:Player #expression:A_Atma_Speak
        -> END
    ** I stay with her and try to live again without being in a relationship.
        I could try going back to being friends like we were before our relationship. #speaker:Player #expression:A_Atma_Speak
        -> END