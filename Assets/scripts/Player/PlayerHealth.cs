using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 5;
    [SerializeField] public int startHealth=3;
    [SerializeField] public float invulnerabilityDuration = 3f;
    [SerializeField] public GameObject resultMenu;
    [SerializeField] public GameObject gameUI;
    public Vector2 playerSpawn;
    private int _health;
    private bool _canGetDamage;
    void Start()
    {
        _canGetDamage = true;
        _health = startHealth;
        MoveToSpawn();
        gameUI.GetComponent<UIManager>().UpdateHealth(_health);
    }

    public void AddHp(int hp)
    {
        _health += Math.Clamp(hp,0, maxHealth);
        gameUI.GetComponent<UIManager>().UpdateHealth(_health);
    }

    public void GetDamage(int dmg)
    {
        if (_canGetDamage)
        {
            _health -= Math.Clamp(dmg,0, maxHealth);
            gameUI.GetComponent<UIManager>().UpdateHealth(_health);
            if (_health <= 0)
            {
                resultMenu.GetComponent<ResultMenu>().Lose();
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

    public int GetHealth()
    {
        return _health;
    }
    
    
}
