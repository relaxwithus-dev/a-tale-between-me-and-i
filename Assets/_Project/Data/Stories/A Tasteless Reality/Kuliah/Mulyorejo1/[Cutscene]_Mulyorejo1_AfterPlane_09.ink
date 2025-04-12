EXTERNAL RemoveItem(itemId)
EXTERNAL StartQuest(questId)
EXTERNAL FinishQuest(questId)

// SFX: Alarm

~RemoveItem("03")
~RemoveItem("04")
~RemoveItem("05")
~FinishQuest("3")

Hum.... #speaker:Dewa #expression:A_Dewa_Speak
Ini benar lewat sini kan? #speaker:Dewa #expression:A_Dewa_Speak
Aku benar-benar tidak tau harus kemana #speaker:Dewa #expression:A_Dewa_Speak
... #speaker:Dewa #expression:A_Dewa_Speak

// Telpon Berdering

Huhh? #speaker:Dewa #expression:A_Dewa_Speak
Ohhh… ini dari pemilik tempat kos #speaker:Dewa #expression:A_Dewa_Speak
Haloo #speaker:Dewa #expression:A_Dewa_Speak

Halo… dengan mas dewa? #speaker:IbuKos #expression:A_IbuKos_Speak

Iya bu ini dengan Dewa #speaker:Dewa #expression:A_Dewa_Speak

Oiya kos saya ada di sebelah musholla an-nur ya #speaker:IbuKos #expression:A_IbuKos_Speak

Kalau dari depan gapura selamat datang arahnya kemana ya? #speaker:Dewa #expression:A_Dewa_Speak

Ohhh tinggal lurus saja nanti sampai sendiri kok #speaker:IbuKos #expression:A_IbuKos_Speak

Lurus? Setelah itu? #speaker:Dewa #expression:A_Dewa_Speak

Setelah itu- #speaker:IbuKos #expression:A_IbuKos_Speak

// Suara telfon terputus

?? #speaker:Dewa #expression:A_Dewa_Speak
Halo? halo? #speaker:Dewa #expression:A_Dewa_Speak
Telponnya terputus… #speaker:Dewa #expression:A_Dewa_Speak
Katanya tinggal lurus saja tapi aku tidak begitu yakin #speaker:Dewa #expression:A_Dewa_Speak
Ohhh aku akan coba mengecek maps #speaker:Dewa #expression:A_Dewa_Speak
.... ..... #speaker:Dewa #expression:A_Dewa_Speak
Tidak ada disini #speaker:Dewa #expression:A_Dewa_Speak
Musholla yang disebut tidak ada di maps #speaker:Dewa #expression:A_Dewa_Speak
Kalau begitu aku akan coba mencarinya sendiri #speaker:Dewa #expression:A_Dewa_Speak

~StartQuest("4")

-> END