using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBehavior : MonoBehaviour
{
    [SerializeField] public Sprite firstGateSprite;

    [SerializeField] public Sprite secondGateSprite;
    [SerializeField] public float teleportBreak = 1f;
    private float _lifeSpan;
    private GameObject _pairedGate;
    private GateBehavior _pairedGateBehavior;
    private bool _canTeleport = true;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(WaitForNextTeleportation());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_pairedGate is not null && other.gameObject.CompareTag("Player") && _canTeleport && _pairedGateBehavior._canTeleport)
        {
            other.gameObject.transform.position = _pairedGate.transform.position;
            StartCoroutine(WaitForNextTeleportation());
            _pairedGateBehavior.StartCoroutine(WaitForNextTeleportation());
        }
    }

    public void SetLifeSpan(float lifeSpan)
    {
        _lifeSpan = lifeSpan;
        StartCoroutine(LiveAndDie());
    }

    IEnumerator LiveAndDie()
    {
        yield return new WaitForSeconds(_lifeSpan);
        Destroy(gameObject);
    }

    IEnumerator WaitForNextTeleportation()
    {
        _canTeleport = false;
        yield return new WaitForSeconds(teleportBreak);
        _canTeleport = true;
    }

    public GameObject GetPairedGate()
    {
        return _pairedGate;
    }

    public void SetPairedGate(GameObject gate)
    {
        _pairedGate = gate;
        _pairedGateBehavior = _pairedGate.GetComponent<GateBehavior>();
    }

    public void SetAsFirst()
    {
        _renderer.sprite = firstGateSprite;
    }
    public void SetAsSecond()
    {
        _renderer.sprite = secondGateSprite;
    }
}
