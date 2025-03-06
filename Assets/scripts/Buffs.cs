using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class Buff
{
    public bool canTakeEffect=true;
    public abstract void TakeEffect(GameObject player);
}

public abstract class DurationBuff : Buff
{
    protected float _duration;
    public abstract void RemoveEffect(GameObject player);

    public float GetDuration()
    {
        return _duration;
    }
}

public abstract class TriggeredBuff : Buff
{
    public abstract void Trigger(GameObject player);
}
public class PointBuff : Buff
{
    private static readonly int Points=300;
    public override void TakeEffect(GameObject player)
    {
        player.GetComponent<PointCounter>().AddToScore(Points);
    }
}

public class MultiplierBuff : DurationBuff
{
    private float _multiplier = 3f;

    public MultiplierBuff()
    {
        _duration = 5f;
    }

    public override void TakeEffect(GameObject player)
    {
        player.GetComponent<PointCounter>().SetMultiplier(_multiplier);
    }

    public override void RemoveEffect(GameObject player)
    {
        PointCounter counter = player.GetComponent<PointCounter>();
        counter.SetMultiplier(counter.defaultMultiplier);
    }
}

public class EaterBuff : DurationBuff
{
    public EaterBuff()
    {
        _duration = 5f;
    }
    public override void TakeEffect(GameObject player)
    {
        ChangeEatable(true);
    }

    public override void RemoveEffect(GameObject player)
    {
        ChangeEatable(false);
    }

    private void ChangeEatable(bool eat)
    {
        foreach (GameObject ghost in GameObject.FindGameObjectsWithTag("ghost"))
        {
            ghost.GetComponent<GhostBehavior>().SetEatable(eat);
        }
    }
}

public class SpeedBoostBuff : DurationBuff
{
    private float speedBoost = 3f;
    private PlayerMovement _movement;
    
    public SpeedBoostBuff()
    {
        _duration = 5f;
    }

    public override void TakeEffect(GameObject player)
    {
        _movement = player.GetComponent<PlayerMovement>();
        _movement.playerSpeed += speedBoost;
    }

    public override void RemoveEffect(GameObject player)
    {
        _movement.playerSpeed -= speedBoost;
    }
}

public class ExtraHealthBuff : Buff
{
    public override void TakeEffect(GameObject player)
    {
        player.GetComponent<PlayerHealth>().AddHp(1);
    }
}

public class SwarmBuff : TriggeredBuff
{
    private int _swarmSize = 5;
    private static List<GameObject> swarmMembers = new();
    
    public override void TakeEffect(GameObject player)
    {
        player.GetComponent<BuffHandler>()._swarmBuff = this;
    }
    
    public override void Trigger(GameObject player)
    {
        Vector3 pos = player.transform.position;
        GameObject template = GameObject.FindGameObjectWithTag("swarm");
        for (int i = 0; i < _swarmSize; i++)
        {
            GameObject swarm = GameObject.Instantiate(template,pos,quaternion.identity);
            swarm.GetComponent<SwarmBehavior>().SetList(swarmMembers);
            swarmMembers.Add(swarm);
        }
    }

    public static bool isSwarmInUse()
    {
        return swarmMembers.Count > 0;
    }
}
