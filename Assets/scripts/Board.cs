using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class Board : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap;
    [SerializeField] public TileBase emptyTile;
    [SerializeField] public TileBase wallTile;
    [SerializeField] public TileBase sideNorthTile;
    [SerializeField] public TileBase sideEastTile;
    [SerializeField] public TileBase sideSouthTile;
    [SerializeField] public TileBase sideWestTile;
    [SerializeField] public TileBase verticalCorridorTile;
    [SerializeField] public TileBase horizontalCorridorTile;
    [SerializeField] public TileBase northToEastEdgeTile;
    [SerializeField] public TileBase northToWestEdgeTile;
    [SerializeField] public TileBase southToEastEdgeTile;
    [SerializeField] public TileBase southToWestEdgeTile;
    [SerializeField] public TileBase tNorthTile;
    [SerializeField] public TileBase tEastTile;
    [SerializeField] public TileBase tSouthTile;
    [SerializeField] public TileBase tWestTile;
    [SerializeField] public TileBase uNorthTile;
    [SerializeField] public TileBase uEastTile;
    [SerializeField] public TileBase uSouthTile;
    [SerializeField] public TileBase uWestTile;
    [SerializeField] public TileBase crossTile;
    [SerializeField] public Camera camera;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject point;
    [SerializeField] public GameObject ghost;
    private int height=20;
    private int width=20;
    
    private enum Direction {UP=1,RIGHT=2,DOWN=4,LEFT=8}

    private char[,] _board;
    private int _x;
    private int _y;
    private const char Empty = ' ';
    private const char Wall = 'X';

    private void Start()
    {
        width = PlayerPrefs.GetInt("width");
        height = PlayerPrefs.GetInt("height");
        Initialize();
        Generate();
        ClearPerimeter();
        PasteCage();
        FillMap();
        SetUpPoints();
        AdjustCamera();
        var spawn =new Vector3(width / 2, (-height / 2)+(player.transform.localScale.y+1), 0);
        var healthComponent = player.GetComponent<PlayerHealth>();
        healthComponent.playerSpawn = spawn;
        healthComponent.MoveToSpawn();
        SpawnGhosts((int)(Math.Sqrt(width*height)/Math.Sqrt(Math.Max(width,height))));
    }

    private void Update()
    {
        tilemap.RefreshAllTiles();
    }

    private void Initialize(){
        _board=new char[width,height];
        for(int i=0;i<width;i++)
        {
            for (int j = 0; j < height; j++)
            {
                _board[i,j] = Wall;
            }
        }
    }

    private void Generate()
    {
        Random random = new Random();
        int toCover=height*width/4,drilled=1;
        _x = random.Next(width);
        _y = random.Next(height);
        _board[_x,_y]=Empty;
        Stack<int[]> nodes = new Stack<int[]>();
        nodes.Push(new []{_x,_y});
        do
        {
            Direction[] possibilities=PosMoves();
            if (possibilities.Length != 0)
            {
                Direction choice = possibilities[random.Next(possibilities.Length)];
                switch (choice)
                {
                    case Direction.UP:
                        _board[_x - 1,_y] = Empty;
                        _board[_x - 2, _y] = Empty;
                        _x -= 2;
                        break;
                    case Direction.RIGHT:
                        _board[_x,_y + 1] = Empty;
                        _board[_x,_y + 2] = Empty;
                        _y += 2;
                        break;
                    case Direction.DOWN:
                        _board[_x + 1,_y] = Empty;
                        _board[_x + 2,_y] = Empty;
                        _x += 2;
                        break;
                    case Direction.LEFT:
                        _board[_x,_y - 1] = Empty;
                        _board[_x,_y - 2] = Empty;
                        _y -= 2;
                        break;
                }
                drilled++;
                nodes.Push(new []{_x,_y});
            }
            else
            {
                if(nodes.Count==0) return;
                int[] previous = nodes.Pop();
                _x = previous[0];
                _y = previous[1];
            }
        } while (nodes.Count>1 && drilled<toCover);
        
    }

    private void ClearPerimeter()
    {
        for (int i = 0; i < height; i++)
        {
            _board[0, i] = Empty;
            _board[width-1, i] = Empty;
        }
        for (int i = 0; i < width; i++)
        {
            _board[i, 0] = Empty;
            _board[i, height - 1] = Empty;
        }
    }

    private void SetUpPoints()
    {
        var pointScale = point.transform.localScale;
        var counter = player.GetComponent<PointCounter>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (_board[i, j] == Empty)
                {
                    var pos = new Vector3(i+0.5f*pointScale.x, -j+0.5f*pointScale.y, 0);
                    GameObject p = Instantiate(point, pos, Quaternion.identity);
                    counter.Add(p);
                }
            }
        }
    }
    
    private void PasteCage()
    {
        int midHeight = height / 2;
        int midWidth=width/2;
        if (width % 2 == 0)
        {
            for (int i = midWidth - 3; i < midWidth + 3; i++)
            {
                for (int j = midHeight - 3; j < midHeight + 2; j++)
                {
                    _board[i, j] = Empty;
                }
            }
            for (int i = midWidth - 2; i < midWidth + 2; i++)
            {
                for (int j = midHeight - 2; j < midHeight + 1; j++)
                {
                    _board[i, j] = Wall;
                }
            }
            for (int k = midHeight - 2; k < midHeight; k++)
            {
                _board[midWidth, k] = Empty;
                _board[midWidth-1, k] = Empty;
            }
        }
        else
        {
            for (int i = midWidth - 3; i < midWidth + 4; i++)
            {
                for (int j = midHeight - 3; j < midHeight + 2; j++)
                {
                    _board[i, j] = Empty;
                }
            }
            for (int i = midWidth - 2; i < midWidth + 3; i++)
            {
                for (int j = midHeight - 2; j < midHeight + 1; j++)
                {
                    _board[i, j] = Wall;
                }
            }
            for (int k = midHeight - 2; k < midHeight; k++)
            {
                _board[midWidth, k] = Empty;
                _board[midWidth-1, k] = Empty;
                _board[midWidth+1, k] = Empty;
            }
        }
        Debug.Log(midWidth+","+midHeight);
    }

    private Direction[] PosMoves()
    {
        HashSet<Direction> options = new HashSet<Direction>();
        foreach (Direction d in Enum.GetValues(typeof(Direction)))
        {
            if (ValidDirection(d))
            {
                options.Add(d);
            }
        }
        return options.ToArray();
    }
    private bool ValidDirection(Direction where)
    {
        switch (where)
        {
            case Direction.UP:
                return _x - 2 >= 0 && _board[_x - 2, _y] != Empty;
            case Direction.RIGHT:
                return _y + 2 < width && _board[_x, _y + 2] != Empty;
            case Direction.DOWN:
                return _x + 2 < height && _board[_x + 2, _y] != Empty;
            case Direction.LEFT:
                return _y - 2 >= 0 && _board[_x, _y - 2] != Empty;
            default:
                return false;
        }
    }

    private void FillMap()
    {
        tilemap.ClearAllTiles();
        for(int i=0;i<width;i++)
        {
            for (int j = 0; j < height; j++)
            {
                var pos = tilemap.WorldToCell(new Vector3Int(i, -j, 0));
                if(_board[i,j]==Wall){
                    switch (CountNeighbourhood(i, j))
                    {
                        case 0: //empty
                            tilemap.SetTile(pos, emptyTile);
                            break;
                        case 2 + 4 + 16 + 64 + 128: //north side
                        case 2 + 4 + 16 + 64 + 128+1:
                        case 2 + 4 + 16 + 64 + 128+32:
                            tilemap.SetTile(pos, sideNorthTile);
                            break;
                        case 1 + 2 + 4 + 8 + 16: //east side
                        case 1 + 2 + 4 + 8 + 16+32:
                        case 1 + 2 + 4 + 8 + 16+128:
                            tilemap.SetTile(pos, sideEastTile);
                            break;
                        case 1 + 2 + 8 + 32 + 64: //south
                        case 1 + 2 + 8 + 32 + 64+4:
                        case 1 + 2 + 8 + 32 + 64+128: 
                            tilemap.SetTile(pos, sideSouthTile);
                            break;
                        case 8 + 16 + 32 + 64 + 128: //west
                        case 8 + 16 + 32 + 64 + 128+1:
                        case 8 + 16 + 32 + 64 + 128+4:
                            tilemap.SetTile(pos, sideWestTile);
                            break;
                        case 2 + 64: //h corridor
                        case 2 + 64+1:
                        case 2 + 64+4:
                        case 2 + 64+1+4:
                        case 2 + 64+32:
                        case 2 + 64+128:
                        case 2 + 64+32+128:
                        case 2 + 64+4+128:
                        case 2 + 64+1+32:
                        case 2 + 64+1+128:
                        case 2 + 64+32+4:
                        case 2 + 64+1+32+4:
                        case 2 + 64+1+32+128:
                        case 2 + 64+1+4+128:
                        case 2 + 64+32+4+128:
                        case 2 + 64+1+32+4+128:
                            tilemap.SetTile(pos, horizontalCorridorTile);
                            break;
                        case 8 + 16: //v corridor
                        case 8 + 16+1:
                        case 8 + 16+32:
                        case 8 + 16+1+32:
                        case 8 + 16+4:
                        case 8 + 16+128:
                        case 8 + 16+4+128:
                        case 8 + 16+1+4:
                        case 8 + 16+32+128:
                        case 8 + 16+1+128:
                        case 8 + 16+32+4:
                        case 8 + 16+1+32+4:
                        case 8 + 16+1+32+128:
                        case 8 + 16+1+4+128:
                        case 8 + 16+32+4+128:
                        case 8 + 16+1+32+4+128:
                            tilemap.SetTile(pos, verticalCorridorTile);
                            break;
                        case 8 + 64: //north to east edge
                        case 8 + 64+1:
                        case 8 + 64+128:
                        case 8 + 64+1+128:
                        case 8 + 64+32:
                        case 8 + 64+1+32:
                        case 8 + 64+128+32:
                        case 8 + 64+1+128+32:
                            tilemap.SetTile(pos, northToEastEdgeTile);
                            break;
                        case 8 + 2: //north to west edge
                        case 8 + 2+32:
                        case 8 +2+ 4:
                        case 8 + 2+32+4:
                        case 8 + 2+1:
                        case 8 + 2+32+1:
                        case 8 +2+ 4+1:
                        case 8 + 2+32+4+1:
                            tilemap.SetTile(pos, northToWestEdgeTile);
                            break;
                        case 16 + 64: //south to east edge
                        case 16 + 64+32:
                        case 16 + 64+32+4:
                        case 16 + 64+4:
                        case 16 + 64+128:
                        case 16 + 64+32+128:
                        case 16 + 64+32+4+128:
                        case 16 + 64+4+128:
                            tilemap.SetTile(pos, southToEastEdgeTile);
                            break;
                        case 16 + 2: //south to west edge
                        case 16 + 2+1:
                        case 16 + 2+128:
                        case 16 + 2+1+128:
                        case 16 + 2+4:
                        case 16 + 2+1+4:
                        case 16 + 2+128+4:
                        case 16 + 2+1+128+4:
                            tilemap.SetTile(pos, southToWestEdgeTile);
                            break;
                        case 2 + 64 + 16: //t north
                            tilemap.SetTile(pos, tNorthTile);
                            break;
                        case 8 + 16 + 2: //t east
                            tilemap.SetTile(pos, tEastTile);
                            break;
                        case 2 + 64 + 8: //t south
                            tilemap.SetTile(pos, tSouthTile);
                            break;
                        case 8 + 16 + 64: //t west
                            tilemap.SetTile(pos, tWestTile);
                            break;
                        case 16: //u north
                        case 16+4:
                        case 16+128:
                        case 16+4+128:
                            tilemap.SetTile(pos, uNorthTile);
                            break;
                        case 2: //u east
                        case 2+1:
                        case 2+4:
                        case 2+1+4:
                            tilemap.SetTile(pos, uEastTile);
                            break;
                        case 8: //u south
                        case 8+1:
                        case 8+32:
                        case 8+1+32:
                            tilemap.SetTile(pos, uSouthTile);
                            break;
                        case 64: //u west
                        case 64+32:
                        case 64+128:
                        case 64+32+128:
                            tilemap.SetTile(pos, uWestTile);
                            break;
                        case 2 + 8 + 16 + 64: //cross
                            tilemap.SetTile(pos, crossTile);
                            break;
                        default: //wall
                            tilemap.SetTile(pos, wallTile);
                            break;
                    }
                }
                else
                {
                    tilemap.SetTile(pos,emptyTile);
                }
            }
        }
        tilemap.RefreshAllTiles();
    }

    private int CountNeighbourhood(int w, int h)
    {
        // 1 8  32
        // 2    64
        // 4 16 128
        int neighbouring = 0;
        if(w>0){
            neighbouring += h>0&&_board[w - 1, h - 1] == 'X' ? 1 : 0;
            neighbouring += _board[w - 1, h] == 'X' ? 2 : 0;
            neighbouring += h<height-1&&_board[w - 1, h + 1] == 'X' ? 4 : 0;
        }
        neighbouring+=h>0&&_board[w,h-1]=='X'?8:0;
        neighbouring+=h<height-1&&_board[w,h+1]=='X'?16:0;
        if(w<width-1){
            neighbouring += h>0&&_board[w + 1, h - 1] == 'X' ? 32 : 0;
            neighbouring += _board[w + 1, h] == 'X' ? 64 : 0;
            neighbouring += h<height-1&&_board[w + 1, h + 1] == 'X' ? 128 : 0;
        }
        return neighbouring;
    }

    private void AdjustCamera()
    {
        camera.orthographicSize = Mathf.Max(height, width);
        camera.transform.position=new Vector3(width / 2f, height / -2f, camera.transform.position.z);
    }

    private void SpawnGhosts(int count)
    {
        for(int i=0;i<count;i++){
            Random random = new Random();
            int x = 0, y = 0;
            do
            {
                x = random.Next(width);
                y = random.Next(height);
            } while (_board[x, y] == Wall);
            GameObject g =  Instantiate(ghost, new Vector3(x, -y, 0), quaternion.identity);
            g.GetComponent<GhostBehavior>().SetBoard(_board,Wall);
        }
    }
    
}
