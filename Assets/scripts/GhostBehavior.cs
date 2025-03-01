using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

public class GhostBehavior : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] public int radius=10;
    private Random _random;
    private char[,] _board;
    private int _width;
    private int _height;
    private char _wall;
    private Vector2Int _target;

    private void Start()
    {
        _random = new Random();
        _width = PlayerPrefs.GetInt("width");
        _height = PlayerPrefs.GetInt("height");
        ChooseTarget(radius);
        var path = BFS();
        Debug.Log(path is null?"no path":"found path");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().GetDamage(1);
        }
    }

    private void ChooseTarget(int radius)
    {
        int x = (int)transform.position.x, y = (int)transform.position.y*-1;
        do
        {
            x = _random.Next(x-radius,x+radius);
            y = _random.Next(y-radius, y+radius);
            _target = new Vector2Int(x,y);
        } while (!IsValid(_target));
    }

    private List<Vector2Int> BFS()
    {
        int patience = 50;
        Vector2Int current = new Vector2Int((int)gameObject.transform.position.x, (int)gameObject.transform.position.y*-1);
        Debug.Log("from "+current+" to "+_target);
        Dictionary<Vector2Int, Vector2Int> neighbours = new Dictionary<Vector2Int, Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Vector2Int[] dirs ={ new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };
        queue.Enqueue(current);
        neighbours[current] = current;
        while (queue.Count>0)
        {
            patience--;
            if (patience <= 0)
            {
                Debug.Log("out of patience");
                return null;
            }
            Vector2Int tmp = queue.Dequeue();
            visited.Add(tmp);
            if (tmp == _target)
            {
                List<Vector2Int> path = new List<Vector2Int>();
                while (tmp!=current)
                {
                    path.Add(tmp);
                    tmp = neighbours[tmp];
                }
                path.Reverse();
                return path;
            }
            foreach (Vector2Int dir in dirs)
            {
                Vector2Int neighbour = new Vector2Int(tmp.x + dir.x, tmp.y+dir.y);
                if(visited.Contains(neighbour) && IsValid(neighbour) && !neighbours.ContainsKey(neighbour))
                {
                    neighbours[neighbour] = tmp;
                    queue.Enqueue(neighbour);
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
}
