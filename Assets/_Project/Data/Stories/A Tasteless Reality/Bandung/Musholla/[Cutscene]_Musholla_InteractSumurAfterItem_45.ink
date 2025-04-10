EXTERNAL AddItem(itemId)
EXTERNAL RemoveItem(itemId)
EXTERNAL StartQuest(questId)
EXTERNAL FinishQuest(questId)

Nahhh… ini ada sumber air #speaker:Dewa #expression:A_Dewa_Speak
Tapi apakah air disini bisa diminum?

Se-sepertinya bisa… #speaker:Waffa Kecil #expression:A_WafaChild_Speak
Ibuku bilang air disini semuanya datang langsung dari mata air diatas gunung itu #speaker:Waffa Kecil #expression:A_WafaChild_Speak

Kalau begitu kita bisa mengambil air dari sumur ini #speaker:Dewa #expression:A_Dewa_Speak
Siapkan gelasnya Waffa #speaker:Dewa #expression:A_Dewa_Speak

Baik kak #speaker:Waffa Kecil #expression:A_WafaChild_Speak

// After get Water

~RemoveItem("10")
~RemoveItem("11")
~AddItem("12")

-> END