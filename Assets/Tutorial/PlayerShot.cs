using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    public int bulletDirection;
    [SerializeField] float shotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(1 * bulletDirection,0,0) *  shotSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Wall 2")  ||
            collision.gameObject.CompareTag("Movel"))
        {
            Destroy(this.gameObject);
        }
    }
}
