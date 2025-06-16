EXTERNAL RemoveItem(itemId)
EXTERNAL StartQuest(questId)
EXTERNAL FinishQuest(questId)

// SFX: Alarm

~RemoveItem("03")
~RemoveItem("04")
~RemoveItem("05")
~FinishQuest("3")

Hm.... #speaker:Dewa #expression:A_Dewa_Speak
Ini benar lewat sini kan? #speaker:Dewa #expression:A_Dewa_Speak #emoji:A_Emoji_Question
Aku benar-benar tidak tau harus kemana #speaker:Dewa #expression:A_Dewa_Speak
... #speaker:Dewa #expression:A_Dewa_Speak

// Telpon Berdering

Huhh? #speaker:Dewa #expression:A_Dewa_Speak
Ohhh… ini dari pemilik tempat kos #speaker:Dewa #expression:A_Dewa_Speak
Haloo #speaker:Dewa #expression:A_Dewa_Speak

Halo… dengan mas dewa? #speaker:Ibu Kos

Iya bu ini dengan Dewa #speaker:Dewa #expression:A_Dewa_Speak

Oiya kos saya ada di sebelah musholla an-nur ya #speaker:Ibu Kos
Kalau dari depan gapura selamat datang arahnya kemana ya? #speaker:Dewa #expression:A_Dewa_Speak

Ohhh tinggal lurus saja nanti sampai sendiri kok #speaker:Ibu Kos

Lurus? Setelah itu? #speaker:Dewa #expression:A_Dewa_Speak

Setelah itu- #speaker:Ibu Kos

// Suara telfon terputus

?? #speaker:Dewa #expression:A_Dewa_Speak
Halo? halo? #speaker:Dewa #expression:A_Dewa_Speak
Telponnya terputus… #speaker:Dewa #expression:A_Dewa_Speak
Katanya tinggal lurus saja tapi aku tidak begitu yakin #speaker:Dewa #expression:A_Dewa_Speak
Ohhh aku akan coba mengecek maps #speaker:Dewa #expression:A_Dewa_Phone_In
.... ..... #speaker:Dewa #expression:A_Dewa_Phone_Idle
Tidak ada disini #speaker:Dewa #expression:A_Dewa_Phone_Idle
Mushola yang disebut tidak ada di maps #speaker:Dewa #expression:A_Dewa_Phone_Idle
Kalau begitu aku akan coba mencarinya sendiri #speaker:Dewa #expression:A_Dewa_Phone_Out

~StartQuest("5") // Start quest besar (Cari Kos)

-> END