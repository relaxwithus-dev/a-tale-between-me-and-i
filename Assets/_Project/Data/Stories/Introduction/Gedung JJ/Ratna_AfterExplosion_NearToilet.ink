EXTERNAL AddItem(itemId)
EXTERNAL StartQuest(questId)
EXTERNAL FinishQuest(questId)

// Create "empty" near toilet range

 ~AddItem("ID")

#speaker:Ratna #expression:A_Ratna_Bingung

Ini kan... #speaker:Ratna #expression:A_Ratna_speak
Kunci untuk gudang kebun #speaker:Ratna #expression:A_Ratna_speak
Mungkin Aku bisa cari alat buat membuka pintu di sana #speaker:Ratna #expression:A_Ratna_speak

~FinishQuest("1")
~StartQuest("2")

-> END