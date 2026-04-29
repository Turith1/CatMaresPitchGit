using UnityEngine;

public class PlayerCapture : MonoBehaviour
{
    [Header("Habilitação")]
    public bool canCapture = false;              

    [Header("Input")]
    public KeyCode captureKey = KeyCode.E;       

    [Header("Raycast")]
    public Camera cam;                           
    public float range = 4f;
    public LayerMask targetMask;                 // defina Layer "Ghost" e marque aqui

    [Header("Regras")]
    public bool requireFrozen = true;            
    public float cooldown = 0.5f;
    [SerializeField]
    private float lastCaptureTime = -999f;

    [Header("Feedback")]
    public AudioClip captureSfx;
    public GameObject captureVfxPrefab;
    public int bottlesCount;                     

    public void GrantTemporarily(float seconds)
    {
        StopAllCoroutines();
        canCapture = true;
        if (seconds > 0) StartCoroutine(RevokeAfter(seconds));
    }
    System.Collections.IEnumerator RevokeAfter(float s) { yield return new WaitForSeconds(s); canCapture = false; }

    void Awake() { if (!cam) cam = Camera.main; }

    void Update()
    {
        if (!canCapture || cam == null) return;
        if (Time.time - lastCaptureTime < cooldown) return;

        if (Input.GetKeyDown(captureKey))
        {
            TryCapture();
        }
    }

    void TryCapture()
    {
        //Ray ray = new(cam.transform.position, cam.transform.forward);
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 5);
        if (Physics.Raycast(ray, out RaycastHit hit, range, targetMask, QueryTriggerInteraction.Collide))
        {
            var capturable = hit.collider.GetComponentInParent<CapturableGhost>();
            if (!capturable) return;

            
            if (requireFrozen)
            {
                var fz = hit.collider.GetComponentInParent<Freezable>();
                if (!(fz && fz.IsFrozen))
                {
                    // opcional: UI "Precisa congelar antes!"
                    return;
                }
            }

            
            lastCaptureTime = Time.time;
            if (captureSfx) AudioSource.PlayClipAtPoint(captureSfx, hit.point);
            if (captureVfxPrefab) Instantiate(captureVfxPrefab, hit.point, Quaternion.identity);

            capturable.Capture();
            bottlesCount++;
        }
    }
}
