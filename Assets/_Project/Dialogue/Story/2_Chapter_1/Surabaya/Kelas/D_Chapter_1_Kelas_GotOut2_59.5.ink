EXTERNAL AddItem(itemId)
EXTERNAL StartQuest(questId)
EXTERNAL FinishQuest(questId)

// All NPCs got out
// Dewa found something shiny on the table desk

??? #speaker:Dewa #expression:A_Dewa_Speak
Sebuah Kunci? #speaker:Dewa #expression:A_Dewa_Speak
Kenapa ada kunci disini? #speaker:Dewa #expression:A_Dewa_Speak #emoji:A_Emoji_Question
Jangan-jangan… #speaker:Dewa #expression:A_Dewa_Speak

~FinishQuest("11") //Selesai quest besar terakhir dari Bandung

-> END