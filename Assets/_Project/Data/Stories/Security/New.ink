EXTERNAL AddItem(itemId)
EXTERNAL StartQuest(questId)
EXTERNAL FinishQuest(questId)

~StartQuest("1")
Hallo, aku ada sesuatu untukmu #speaker:Pak Satpam #expression:A_PakSatpam_Speak

* [Terima] 
    Apa ini pak, terima kasih! #speaker:Player #expression:A_Atma_Speak
    ~AddItem("105")
    ~FinishQuest("1")
* [Pergi] 
    Maaf pak, saya tidak bisa menerimanya...
    -> END