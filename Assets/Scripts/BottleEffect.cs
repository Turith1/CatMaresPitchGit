using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class BottleEffect : MonoBehaviour
{
    [Header("InvulnerabilidadeGarrafa")]
    public float _duration = 5.0f;
    public float blinkInterval = 0.1f;      // piscar do mesh
    public float dist = 10;
    private bool isInvulnerable = false;
    private float timer = 0f;

    [Header("Eventos")]
    public UnityEvent onRun;
    public UnityEvent Death;
    public ActionGhosts _rangePlayer;
    public PlayerHealth _heal;

    public Transform _player;
    // Start is called before the first frame update


    void Update()
    {
        if (isInvulnerable)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                isInvulnerable = false;
                Debug.Log($"{GameObject.FindGameObjectsWithTag("Player")} is now vulnerable.");
            }
        }
    }

    public void Activate(float duration)
    {
        isInvulnerable = true;
        timer = duration;
        Debug.Log($"{GameObject.FindGameObjectsWithTag("Player")} is now INVULNERABLE for {duration} seconds.");
    }

    // Exemplo de uso: bloquear dano
    public void Scape()
    {
        if (isInvulnerable && _duration >= .1f)
        {
            if (_rangePlayer != null)
            {
                float dist = Vector3.Distance(-transform.position, _rangePlayer.transform.position);
                return;
            }

            if (timer <= 0f)
            {
                isInvulnerable = false;
            }
        }
    }

}
