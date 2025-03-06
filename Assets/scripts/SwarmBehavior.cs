using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SwarmBehavior : MonoBehaviour
{
    private static Random _random=new Random();
    [SerializeField] public float lifeSpan = 3f;
    [SerializeField] public float speed = 10f;
    private List<GameObject> _swarm;
    void Start()
    {
        StartCoroutine(LiveAndDie());
    }
    
    void Update()
    {
        int x = _random.Next(3) % 2 == 1 ? 1 :_random.Next(2) % 2 == 0 ? -1 : 0;
        int y = _random.Next(3) % 2 == 1 ? 1 :_random.Next(2) % 2 == 0 ? -1 : 0;
        float tmp = speed * Time.deltaTime;
        transform.position += new Vector3(x*tmp, y*tmp, 0);
    }

    IEnumerator LiveAndDie()
    {
        yield return new WaitForSeconds(lifeSpan);
        _swarm.Remove(gameObject);
        Destroy(gameObject);
    }

    public void SetList(List<GameObject> list)
    {
        _swarm = list;
    }
}
