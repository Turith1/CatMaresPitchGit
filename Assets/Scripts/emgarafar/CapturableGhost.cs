using UnityEngine;
using UnityEngine.SceneManagement;

public class CapturableGhost : MonoBehaviour
{
    [Header("Ao capturar")]
    public GameObject bottledGhostPrefab; // opcional: item/garrafa que cai
    public GameObject burstVfx;           
    public AudioClip burstSfx;

    [Tooltip("Se verdadeiro, exige que haja Freezable.IsFrozen no momento da captura.")]
    public bool mustBeFrozen = false;


    public void Capture()
    {
        if (mustBeFrozen)
        {
            var fz = GetComponentInParent<Freezable>();
            if (!(fz && fz.IsFrozen)) return;
        }

        if (burstSfx) AudioSource.PlayClipAtPoint(burstSfx, transform.position);
        if (burstVfx) Instantiate(burstVfx, transform.position, Quaternion.identity);

        if (bottledGhostPrefab)
            Instantiate(bottledGhostPrefab, transform.position, Quaternion.identity);

        SceneManager.LoadScene("vICTORY");
        //Destroy(gameObject); // elimina o fantasma da cena

        //Invoke("CallVictory", 3f);
    }

    /*private void CallVictory()
    {
        SceneManager.LoadScene("vICTORY");
    }*/
}
