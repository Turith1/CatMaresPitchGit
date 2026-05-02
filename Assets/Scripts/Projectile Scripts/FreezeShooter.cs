using UnityEngine;

public class FreezeShooter : MonoBehaviour
{
    [Header("Habilitação")]
    public bool canShoot = false;                
    [Header("Setup")]
    public Transform firePoint;                   
    public GameObject projectilePrefab;
    [SerializeField]
    private VisualFeedBack _feedBack;

    [Header("Tiro")]
    public float cooldown = 0.2f;
    float lastShotTime = -999f;

    [Header("Carga")]
    public KeyCode shootMouse = KeyCode.Mouse1;  
    public float chargeThreshold = 0.5f;          
    public float maxChargeTime = 2.0f;            
    float pressTime;
    bool charging;

    [Header("Parâmetros do projétil")]
    public float baseSpeed = 22f;
    public float chargedSpeed = 30f;
    public float baseFreezeSeconds = 2.0f;
    public float chargedFreezeSeconds = 5.0f;
    public float baseScale = 1.0f;
    public float chargedScale = 1.5f;

    public void GrantTemporarily(float seconds)
    {
        StopAllCoroutines();
        canShoot = true;
        _feedBack.StupidGun(true);
        if (seconds > 0) StartCoroutine(RevokeAfter(seconds));
    }

    System.Collections.IEnumerator RevokeAfter(float s)
    {
        yield return new WaitForSeconds(s);
        canShoot = false;
        _feedBack.StupidGun(false);
    }

    void Update()
    {
        if (!canShoot || projectilePrefab == null || firePoint == null) return;

        if (Input.GetKeyDown(shootMouse))
        {
            charging = true;
            pressTime = Time.time;
        }

        if (Input.GetKeyUp(shootMouse))
        {
            if (Time.time - lastShotTime < cooldown) { charging = false; return; }

            float held = Mathf.Clamp(Time.time - pressTime, 0f, maxChargeTime);
            bool isCharged = held >= chargeThreshold;

            // fator de 0..1 só quando carregado; se não, fica 0
            float t = isCharged ? Mathf.InverseLerp(chargeThreshold, maxChargeTime, held) : 0f;

            Fire(isCharged, t);
            lastShotTime = Time.time;
            charging = false;
        }
    }

    void Fire(bool charged, float t)
    {
        var go = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // escala visual
        float scale = Mathf.Lerp(baseScale, chargedScale, t);
        go.transform.localScale *= scale;

        // parâmetros de gelo e velocidade
        float freeze = charged ? Mathf.Lerp(baseFreezeSeconds, chargedFreezeSeconds, t) : baseFreezeSeconds;
        float speed = charged ? Mathf.Lerp(baseSpeed, chargedSpeed, t) : baseSpeed;

        var proj = go.GetComponent<FreezeProjectile>();
        if (proj) proj.Init(speed, freeze);
        else
        {
            
            var rb = go.GetComponent<Rigidbody>();
            if (rb) rb.velocity = firePoint.forward * speed;
        }

    }
}
