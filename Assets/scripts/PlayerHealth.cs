using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int startHealth=3;
    [SerializeField] public float invulnerabilityDuration = 3f;
    public Vector2 playerSpawn;
    private int _health;
    private bool _canGetDamage;
    void Start()
    {
        _canGetDamage = true;
        _health = startHealth;
        MoveToSpawn();
    }

    public void AddHp(int hp)
    {
        _health += Math.Abs(hp);
    }

    public void GetDamage(int dmg)
    {
        if (_canGetDamage)
        {
            _health -= Math.Abs(dmg);
            if (_health <= 0)
            {
                Loose();
                return;
            }
            MoveToSpawn();
            Debug.Log(_health);
            StartCoroutine(CountInvulnerability());
        }
    }

    public void MoveToSpawn()
    {
        gameObject.transform.position = playerSpawn;
    }

    IEnumerator CountInvulnerability()
    {
        _canGetDamage = false;
        yield return new WaitForSeconds(invulnerabilityDuration);
        _canGetDamage = true;
    }

    private void Loose()
    {
        Debug.Log("LOOSE");
    }
    
    
}
