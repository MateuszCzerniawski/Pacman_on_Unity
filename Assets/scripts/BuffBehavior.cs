
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
    private static Random _random = new Random();
    private SpriteRenderer _renderer;
    private Buff _buff;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        /*switch (_random.Next(5))
        {
            case 0:
                _buff = new PointBuff();
                _renderer.sprite = PointBuffSprtie;
                break;
            case 1:
                _buff = new MultiplierBuff();
                _renderer.sprite = MultiplierBuffSprtie;
                break;
            case 2:
                _buff = new EaterBuff();
                _renderer.sprite = EaterBuffSprtie;
                break;
            case 3:
                _buff = new SpeedBoostBuff();
                _renderer.sprite = SpeedBoostBuffSprtie;
                break;
            case 4:
                _buff = new ExtraHealthBuff();
                _renderer.sprite = ExtraHealthBuffSprite;
                break;
        }*/
        _buff = new SwarmBuff();
        _renderer.sprite = SwarmBuffSprite;
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
