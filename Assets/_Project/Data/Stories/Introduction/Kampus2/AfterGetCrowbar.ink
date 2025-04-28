EXTERNAL AddItem(itemId)
EXTERNAL StartQuest(questId)
EXTERNAL FinishQuest(questId)

Aku bakal coba buat buka pintu ini #speaker:Ratna #expression:A_Ratna_Speak

// Inisiasi minigame panah

* [sukses]
Ah... #speaker:Ratna #expression:A_Ratna_Kaget
Sepertinya aku berhasil #speaker:Ratna #expression:A_Ratna_Speak
 ~FinishQuest("3")

* [gagal]
Ahh... Aku ga bisa buka pintu ini #speaker:Ratna #expression:A_Ratna_Speak
// SFX: Ledakan
Aku harus tetep coba #speaker:Ratna #expression:A_Ratna_Speak

-> END