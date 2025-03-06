using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GhostBehavior : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] public int radius=10;
    [SerializeField] public int eatValue = 100;
    private Random _random;
    private char[,] _board;
    private int _width;
    private int _height;
    private char _wall;
    private Vector2Int _target;
    private Queue<Vector2Int> _path;
    private bool _eatable;

    private void Start()
    {
        _random = new Random();
        _eatable = false;
        _width = PlayerPrefs.GetInt("width",-1);
        _height = PlayerPrefs.GetInt("height",-1);
        if (_width < 0 || _height < 0 || _board is null)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (_path is null)
        {
            TakePath();
        }
        Move();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject;
        if (!player.CompareTag("Player")){ return; }
        var health = player.GetComponent<PlayerHealth>();
        if (!_eatable)
        {
            health.GetDamage(1);
        }
        else
        {
            player.GetComponent<PointCounter>().AddToScore(eatValue);
            transform.position = health.playerSpawn;
            TakePath();
        }
    }
    

    private void ChooseTarget(int radius)
    {
        int x = (int)transform.position.x, y = (int)transform.position.y*-1;
        do
        {
            x = Math.Clamp(_random.Next(x - radius, x + radius),0,_width-1);
            y = Math.Clamp(_random.Next(y-radius, y+radius),0,_height-1);
            _target = new Vector2Int(x,y);
        } while (!IsValid(_target));
    }

    private Queue<Vector2Int> BFS()
    {
        Vector2Int current = new Vector2Int((int)gameObject.transform.position.x, (int)gameObject.transform.position.y*-1);
        Dictionary<Vector2Int, Vector2Int> neighbours = new Dictionary<Vector2Int, Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Vector2Int[] dirs ={ new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };
        queue.Enqueue(current);
        neighbours[current] = current;
        while (queue.Count>0)
        {
            Vector2Int tmp = queue.Dequeue();
            visited.Add(tmp);
            if (tmp == _target)
            {
                List<Vector2Int> reversed = new List<Vector2Int>();
                while (tmp!=current)
                {
                    reversed.Add(tmp);
                    tmp = neighbours[tmp];
                }
                reversed.Reverse();
                Queue<Vector2Int> path = new Queue<Vector2Int>();
                foreach (Vector2Int node in reversed)
                {
                    path.Enqueue(node);
                }
                return path;
            }
            foreach (Vector2Int dir in dirs)
            {
                Vector2Int neighbour = new Vector2Int(tmp.x + dir.x, tmp.y+dir.y);
                if(!visited.Contains(neighbour) && IsValid(neighbour) && !neighbours.ContainsKey(neighbour))
                {
                    neighbours[neighbour] = tmp;
                    queue.Enqueue(neighbour);
                    visited.Add(neighbour);
                }
            }
        }
        return null;
    }

    private bool IsValid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < _width && pos.y >= 0 && pos.y < _height && _board[pos.x,pos.y]!=_wall;
    }
    
    public void SetBoard(char[,] board, char wall)
    {
        _board = board;
        _wall = wall;
    }

    private void TakePath()
    {
        int currentRadius = radius;
        do
        {
            ChooseTarget(currentRadius);
            _path = BFS();
            currentRadius /= 2;
        } while (_path is null && currentRadius>0);

    }

    private void Move()
    {
        if(_path.Count>0){
            var nextPos = (Vector2)_path.Peek();
            nextPos.y *= -1;
            nextPos += new Vector2(transform.localScale.x*0.25f, transform.localScale.y*0.25f);
            transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, nextPos) < 0.1f)
            {
                _path.Dequeue();
            }
        }
        else
        {
            _path = null;
        }
    }

    public void SetEatable(bool eat)
    {
        _eatable = eat;
        //Tu miejsce na zmianę sprite
    }
}
