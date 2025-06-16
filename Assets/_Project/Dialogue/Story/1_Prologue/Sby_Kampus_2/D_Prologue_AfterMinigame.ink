EXTERNAL AddItem(itemId)
EXTERNAL StartQuest(questId)
EXTERNAL FinishQuest(questId)
EXTERNAL RemoveItem(itemId)
EXTERNAL UpdateStoryChapter(chapter)

~RemoveItem("2")

A-apa yang terjadi di sini? #speaker:Ratna #expression:A_Ratna_Speak #emoji:A_Emoji_Question
Kenapa sepi sekali #speaker:Ratna #expression:A_Ratna_Speak
Siapa itu? Kenapa ada anak kecil #speaker:Ratna #expression:A_Ratna_Speak
Hei! Jangan pukul anak itu! #speaker:Ratna #expression:A_Ratna_Speak #emoji:A_Emoji_Angry

~FinishQuest("1")
~UpdateStoryChapter("Chapter_1")

-> END