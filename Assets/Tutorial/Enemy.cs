using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Rigidbody2D enemyrig;
    public Transform playerTransform;
    [SerializeField] float speed;
    bool incapacitado;
    float incapacitadoTimer;
    [SerializeField] int vida = 3;
    SpriteRenderer spriteRenderer;
    bool isRunningAway = false;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Gato").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        PeriodoIncapacitado();
        if (playerTransform != null && isRunningAway == false)
        {
            Vector2 direction = playerTransform.position - transform.position;
            enemyrig.velocity = direction.normalized * speed;

        }
        if(isRunningAway == true)
        {
            Vector2 dir = (transform.position - playerTransform.position).normalized;
            transform.position += (Vector3)dir * speed * Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Tiro"))
        {
            vida = vida - 1;
        }
        if (vida <= 0)
        {
            incapacitado = true;
            
        }
        if (incapacitado == true)
        {
            incapacitadoTimer = 0f;
            GetComponent<Collider2D>().enabled = false;
            enemyrig.constraints = RigidbodyConstraints2D.FreezeAll;
        }

    }
    void PeriodoIncapacitado()
    {
        if (incapacitado == true)
        {
            incapacitadoTimer += Time.deltaTime;
            
        }
        if (incapacitadoTimer >= 3f)
        {
            incapacitado = false;
            incapacitadoTimer = 0f;
            if (incapacitado == false)
            {
                vida = 3;
                GetComponent<Collider2D>().enabled = true;
                enemyrig.constraints = RigidbodyConstraints2D.None;
            }
        }
        
    }
    public void ColorChange()
    {
        Color color = new Color(Random.value, Random.value, Random.value);
        spriteRenderer.material.color = color;
    }
    public void RunAway()
    {
        isRunningAway = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Gato") && isRunningAway == true) 
        {
            Destroy(this.gameObject);
        }
    }
}
