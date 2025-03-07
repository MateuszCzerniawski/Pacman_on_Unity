
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffHandler : MonoBehaviour
{
    [SerializeField] public KeyCode swarmTrigger = KeyCode.E;
    [SerializeField] public KeyCode warpTunnelTrigger = KeyCode.Q;
    [SerializeField] public GameObject gameUI;
    public TriggeredBuff _swarmBuff;
    public TriggeredBuff _warpTunnelBuff;
    private UIManager _uiManager;
    

    private void Awake()
    {
        _uiManager = gameUI.GetComponent<UIManager>();
    }

    private void Update()
    {
        Dictionary<Sprite, char> buffs = new Dictionary<Sprite, char>();
        if(_swarmBuff is not null && !SwarmBuff.isSwarmInUse()){
            buffs[_swarmBuff.sprite] = swarmTrigger.ToString()[0];
        }
        if(_warpTunnelBuff is not null){
            buffs[_warpTunnelBuff.sprite] = warpTunnelTrigger.ToString()[0];
        }
        _uiManager.UpdateBuffs(buffs);
        if (Input.GetKeyDown(swarmTrigger) && _swarmBuff is not null && !SwarmBuff.isSwarmInUse())
        {
            _swarmBuff.Trigger(gameObject);
            _swarmBuff = null;
        }
        if (Input.GetKeyDown(warpTunnelTrigger) && _warpTunnelBuff is not null)
        {
            _warpTunnelBuff.Trigger(gameObject);
            _warpTunnelBuff = null;
        }
    }

    public void Apply(Buff buff)
    {
        if (!buff.canTakeEffect)
        {
            return;
        }
        if (buff is DurationBuff)
        {
            StartCoroutine(HandleDurationBuff((DurationBuff)buff));
        }
        else
        {
            buff.TakeEffect(gameObject);
        }
        buff.canTakeEffect = false;
    }
    
    private IEnumerator HandleDurationBuff(DurationBuff buff)
    {
        buff.TakeEffect(gameObject);
        yield return new WaitForSeconds(buff.GetDuration());
        buff.RemoveEffect(gameObject);
    }

    private IEnumerator HandlePhasingEnd()
    {
        bool canEnd;
        do
        {
            canEnd = true;
            List<Collider2D> colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0).ToList();
            for (int i = 0; i < colliders.Count && canEnd; i++)
            {
                canEnd = colliders[i].gameObject.layer != LayerMask.NameToLayer("map");
            }
            yield return null;
        } while (!canEnd);
        Physics2D.IgnoreLayerCollision(gameObject.layer,LayerMask.NameToLayer("map"),false);
    }

    public void EndPhasing()
    {
        StartCoroutine(HandlePhasingEnd());
    }
}
