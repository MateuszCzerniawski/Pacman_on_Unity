
using System;
using System.Collections;
using UnityEngine;

public class BuffHandler : MonoBehaviour
{
    [SerializeField] public KeyCode swarmTrigger = KeyCode.E;
    [SerializeField] public KeyCode warpTunnelTrigger = KeyCode.Q;
    public TriggeredBuff _swarmBuff;
    public TriggeredBuff _warpTunnelBuff;

    private void Update()
    {
        if (Input.GetKeyDown(swarmTrigger) && _swarmBuff is not null && !SwarmBuff.isSwarmInUse())
        {
            _swarmBuff.Trigger(gameObject);
        }

        if (Input.GetKeyDown(warpTunnelTrigger) && _warpTunnelBuff is not null)
        {
            
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
        Debug.Log("buff "+buff+"trigerred for "+buff.GetDuration());
        yield return new WaitForSeconds(buff.GetDuration());
        buff.RemoveEffect(gameObject);
        Debug.Log("buff "+buff+"removed");
    }
}
