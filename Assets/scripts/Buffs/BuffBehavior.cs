
using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

public class BuffBehavior : MonoBehaviour
{
    [SerializeField] public Sprite PointBuffSprtie;
    [SerializeField] public Sprite MultiplierBuffSprtie;
    [SerializeField] public Sprite EaterBuffSprtie;
    [SerializeField] public Sprite SpeedBoostBuffSprtie;
    [SerializeField] public Sprite ExtraHealthBuffSprite;
    [SerializeField] public Sprite SwarmBuffSprite;
    [SerializeField] public Sprite WarpTunnelSprite;
    [SerializeField] public Sprite PhasingBuffSprite;
    private static Random _random = new Random();
    private SpriteRenderer _renderer;
    private Buff _buff;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        switch (_random.Next(8))
        {
            case 0:
                _buff = new PointBuff();
                _buff.sprite = PointBuffSprtie;
                _renderer.sprite = PointBuffSprtie;
                break;
            case 1:
                _buff = new MultiplierBuff();
                _buff.sprite = MultiplierBuffSprtie;
                _renderer.sprite = MultiplierBuffSprtie;
                break;
            case 2:
                _buff = new EaterBuff();
                _buff.sprite = EaterBuffSprtie;
                _renderer.sprite = EaterBuffSprtie;
                break;
            case 3:
                _buff = new SpeedBoostBuff();
                _buff.sprite = SpeedBoostBuffSprtie;
                _renderer.sprite = SpeedBoostBuffSprtie;
                break;
            case 4:
                _buff = new ExtraHealthBuff();
                _buff.sprite = ExtraHealthBuffSprite;
                _renderer.sprite = ExtraHealthBuffSprite;
                break;
            case 5:
                _buff = new SwarmBuff();
                _buff.sprite = SwarmBuffSprite;
                _renderer.sprite = SwarmBuffSprite;
                break;
            case 6:
                _buff = new WarpTunnelBuff();
                _buff.sprite = WarpTunnelSprite;
                _renderer.sprite = WarpTunnelSprite;
                break;
            case 7:
                _buff = new PhasingBuff();
                _buff.sprite = PhasingBuffSprite;
                _renderer.sprite = PhasingBuffSprite;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject;
        if (player.CompareTag("Player"))
        {
            player.GetComponent<BuffHandler>().Apply(_buff);
            Destroy(gameObject);
        }
    }

    
}
