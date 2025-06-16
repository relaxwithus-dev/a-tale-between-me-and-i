EXTERNAL AddItem(itemId)
EXTERNAL RemoveItem(itemId)
EXTERNAL StartQuest(questId)
EXTERNAL FinishQuest(questId)
EXTERNAL EnterMinigame()

Nahhh… ini ada sumber air #speaker:Dewa #expression:A_Dewa_Speak
Tapi apakah air disini bisa diminum? #speaker:Dewa #expression:A_Dewa_Speak

Se-sepertinya bisa… #speaker:Waffa Kecil #expression:A_Waffa_Child_Speak
Ibuku bilang air disini semuanya datang langsung dari mata air diatas gunung itu #speaker:Waffa Kecil #expression:A_Waffa_Child_Speak

Kalau begitu kita bisa mengambil air dari sumur ini #speaker:Dewa #expression:A_Dewa_Speak
Siapkan gelasnya Waffa #speaker:Dewa #expression:A_Dewa_Speak

Baik kak #speaker:Waffa Kecil #expression:A_Waffa_Child_Speak #emoji:A_Emoji_Love

~EnterMinigame()

// After get Water

~RemoveItem("10")
~RemoveItem("11")
~AddItem("12")

-> END