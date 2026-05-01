using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyKillerItem : MonoBehaviour
{
    public UnityEvent coletouItem;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gato"))
        {
            coletouItem.Invoke();
            Destroy(this.gameObject);
        }
    }
}
