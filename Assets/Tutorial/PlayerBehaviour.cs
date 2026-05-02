using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] Rigidbody2D rig;
    [SerializeField] float moveSpeed;
    bool canShoot;
    [SerializeField] GameObject tiro;
    [SerializeField] float boostTimer;
    [SerializeField] bool isBoosting;
    [SerializeField] public int vida = 3;
    [SerializeField] bool isInvincible = false;
    [SerializeField] float invencTime;
    [SerializeField] bool pegouColeira;
    [SerializeField] SpriteRenderer playerspriteRenderer;
    [SerializeField] GameObject[] hearts;
    private Color originalColor;
    public float flashDuration;
    bool isFlashing = false;
    [SerializeField] public GameObject invincParticles;
    private GameObject  activeParticles;
    // Start is called before the first frame update
    void Start()
    {
        playerspriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = playerspriteRenderer.color;
        
    }

    // Update is called once per frame
    void Update()
    {
        Tiro();
        WASDmove();
        SuperSpeed();
        Invencibility();
        InvencibilityColeira();
        Damage();
        

    }

    void WASDmove()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        rig.velocity = new Vector2(horizontal, vertical).normalized * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PowerUP"))
        {
            Destroy(collision.gameObject);
            canShoot = true;
        }


        if (collision.gameObject.CompareTag("Speed"))
        {
            Destroy(collision.gameObject);
            isBoosting = true;
        }

        if (collision.gameObject.CompareTag("Coleira"))
        {
            Destroy(collision.gameObject);
            isInvincible = true;
            pegouColeira = true;

        }

    }
    void Tiro()
    {
        if (canShoot == true && Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(tiro, transform.position, Quaternion.identity);
        }
    }
    void SuperSpeed()
    {
        if (isBoosting == true)
        {
            moveSpeed = 15;
            boostTimer += Time.deltaTime;
            if (boostTimer >= 10)
            {
                isBoosting = false;
                moveSpeed = 5;
                boostTimer = 0;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && (isInvincible == false))
        {
            vida = vida - 1;
            isInvincible = true;

            if (!pegouColeira && !isFlashing)
            {
                StartCoroutine(FlashWhite());
            }


            if (vida <= 0)
            {
                Destroy(this.gameObject);
                SceneManager.LoadScene("Tutorial");
            }
        }
    }
    void Invencibility()
    {
        if (isInvincible == true && pegouColeira == false)
        {
            invencTime += Time.deltaTime;
            if (invencTime >= 3)
            {
                isInvincible = false;
                invencTime = 0f;
            }
        }

    }
    void InvencibilityColeira()
    {
        if (isInvincible && pegouColeira == true)
        {
            invencTime += Time.deltaTime;
            if(activeParticles == null)
            {
                activeParticles = Instantiate(invincParticles,transform.position, Quaternion.identity);
            }
            if(activeParticles != null)
            {
                activeParticles.transform.position = transform.position;
            }

            if (invencTime >= 10)
            {
                isInvincible = false;
                pegouColeira = false;
                invencTime = 0f;

                if (activeParticles != null)
                {
                    Destroy(activeParticles);
                }
            }
        }
    }
    void Damage()
    {
        if (vida < 1)
        {
            Destroy(hearts[0].gameObject);
        }
        else if (vida < 2)
        {
            Destroy(hearts[1].gameObject);
        }
        else if (vida < 3)
        {
            Destroy(hearts[2].gameObject);
        }
    }
    IEnumerator FlashWhite()
    {
        playerspriteRenderer.color = Color.red;
        yield return new WaitForSeconds(3);
        playerspriteRenderer.color = originalColor;

    }
    
}


