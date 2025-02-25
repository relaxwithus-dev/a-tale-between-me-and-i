using UnityEngine;
using DG.Tweening;
using ATBMI.Entities.Player;

public class CutsceneTrigger : MonoBehaviour
{
    public CutsceneManager cutsceneManager; // Hubungkan ke CutsceneManager

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Pastikan pemain memiliki tag "Player"
        {
            cutsceneManager.PlayCutscene(); // Panggil cutscene
            gameObject.SetActive(false); // Nonaktifkan trigger setelah dipakai
        }
    }
}
