using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.AI;

public class CapturableGhost : MonoBehaviour
{
    [Header("Ao capturar")]
    public GameObject bottledGhostPrefab; // opcional: item/garrafa que cai
    public GameObject burstVfx;           
    public AudioClip burstSfx;

    [SerializeField]
    private NavMeshAgent monsterAgent;
    [SerializeField]
    private ActionGhosts actionGhost;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float punchSize = 1.2f;
    [SerializeField]
    private Tween monsterTween;
    [SerializeField]
    private ProcagemPowerUps procs;
    [SerializeField]
    private GameObject portal;

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

        actionGhost.enabled = false;
        anim.enabled = false;
        monsterAgent.isStopped = true;
        monsterAgent.ResetPath();
        monsterAgent.velocity = Vector3.zero;


        /*if (bottledGhostPrefab)
        {
            GameObject garrafa = Instantiate(bottledGhostPrefab, transform.position, Quaternion.identity);
        }*/

        Instantiate(portal, new Vector3(transform.position.x, .5f, transform.position.z), Quaternion.LookRotation(Vector3.up));
        Sequence seq = DOTween.Sequence();

        seq.Append(
            transform.DOScale(0.85f, 0.08f)
        );

        seq.Append(
            transform.DOPunchScale(Vector3.one * 0.4f, 0.25f, 8, 0.8f)
        );

        seq.Join(
            transform.DOPunchRotation(new Vector3(0, 0, 15), 0.25f, 8, 0.8f)
        );

        seq.Append(
            transform.DOScale(Vector3.zero, 0.35f)
                .SetEase(Ease.InBack)
        );

        seq.AppendCallback(() =>
        {
            Instantiate(
                bottledGhostPrefab,
                new Vector3(transform.position.x, .5f, transform.position.z),
                Quaternion.identity
            );

            gameObject.SetActive(false);
        });
        /*monsterTween = transform.DOPunchScale(Vector3.one * punchSize, .5f, 10, 1).SetEase(Ease.InBack)
            .OnComplete(() => { transform.DOScale(new Vector3(.2f, .2f, .2f), .8f); })
            .OnComplete(() => gameObject.SetActive(false)).OnComplete(() => Instantiate(bottledGhostPrefab, transform.position, Quaternion.identity));*/
        //SceneManager.LoadScene("vICTORY");
        //Destroy(gameObject); // elimina o fantasma da cena

        Invoke("CallVictory", 3f);
    }

    private void OnDisable()
    {
        monsterTween.Kill();
    }

    private void CallVictory()
    {
        SceneManager.LoadScene("vICTORY");
    }
}
