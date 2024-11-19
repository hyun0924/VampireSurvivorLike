using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileMapManager
{
    GameObject _grid;
    Tilemap[] _tilemaps;

    Stack<GameObject> _unusedTilemaps = new Stack<GameObject>();
    List<GameObject> _usingTilemaps = new List<GameObject>();

    TileBase tileBase;
    Vector2Int _currentPlayerIdx = Vector2Int.zero;

    int _pixelSize = 108;
    int _width;
    int _height;

    public void Init()
    {
        _grid = new GameObject { name = "@TileMap" };
        _grid.GetOrAddComponent<Grid>();

        _tilemaps = new Tilemap[9];
        tileBase = Managers.Resource.Load<TileBase>("TileMap/TileBase");
        _width = Screen.width / _pixelSize + 1;
        _height = Screen.height / _pixelSize + 1;

        GenerateTileMaps();
    }

    private void GenerateTileMaps()
    {
        for (int k = 0; k < _tilemaps.Length; k++)
        {
            GameObject go = Managers.Resource.Instantiate("Tilemap", _grid.transform);
            go.name = $"Tilemap {k}";
            _tilemaps[k] = go.GetComponent<Tilemap>();
            _tilemaps[k].size = new Vector3Int(_width, _height);

            BoxCollider2D collider = go.GetOrAddComponent<BoxCollider2D>();
            collider.size = new Vector2Int(_width, _height);
            collider.offset = new Vector2(_width / 2.0f, _height / 2.0f);
            collider.isTrigger = true;

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _tilemaps[k].SetTile(new Vector3Int(i, j), tileBase);
                }
            }

            _unusedTilemaps.Push(go);
        }

        RelocateTileMap();
    }

    public void RelocateTileMap()
    {
        bool[,] isTileMap = new bool[3, 3];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                isTileMap[i, j] = false;
            }
        }

        foreach (var tilemap in _usingTilemaps)
        {
            int idxX = tilemap.transform.position.x < 0 ? -1 * (int)MathF.Ceiling(MathF.Abs(tilemap.transform.position.x / _width)) : (int)tilemap.transform.position.x / _width;
            int idxY = tilemap.transform.position.y < 0 ? -1 * (int)MathF.Ceiling(MathF.Abs(tilemap.transform.position.y / _height)) : (int)tilemap.transform.position.y / _height;

            isTileMap[1 - (idxX - _currentPlayerIdx.x), 1 - (idxY - _currentPlayerIdx.y)] = true;
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (isTileMap[i, j]) continue;

                if (_unusedTilemaps.Count == 0) { Debug.Log("Tilemap error"); return; } // TODO: 맵 새로 만들게 하기?
                GameObject tilemap = _unusedTilemaps.Pop();
                tilemap.transform.position = new Vector3((1 + _currentPlayerIdx.x - i) * _width, (1 + _currentPlayerIdx.y - j) * _height);
                tilemap.SetActive(true);
                _usingTilemaps.Add(tilemap);
            }
        }
    }

    public void UpdateTileMap(Vector3 playerPos, Vector3 moveVec)
    {
        int idxX = playerPos.x < 0 ? -1 * (int)MathF.Ceiling(MathF.Abs(playerPos.x / _width)) : (int)(playerPos.x / _width);
        int idxY = playerPos.y < 0 ? -1 * (int)MathF.Ceiling(MathF.Abs(playerPos.y / _height)) : (int)(playerPos.y / _height);

        if (_currentPlayerIdx.x != idxX || _currentPlayerIdx.y != idxY)
        {
            _currentPlayerIdx = new Vector2Int(idxX, idxY);
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(playerPos, new Vector3(_width, _height), 0, LayerMask.GetMask("Tile"));
            List<GameObject> hitGameObject = new List<GameObject>();

            foreach (Collider2D col in hitColliders) { hitGameObject.Add(col.gameObject); }

            for (int i = _usingTilemaps.Count - 1; i >= 0; i--)
            {
                GameObject go = _usingTilemaps[i];
                if (!hitGameObject.Contains(go))
                {
                    go.SetActive(false);
                    _unusedTilemaps.Push(go);
                    _usingTilemaps.Remove(go);
                }
            }

            RelocateTileMap();
        }
    }

    public void Clear()
    {
        Managers.Resource.Destroy(_grid);
        _grid = null;
        _unusedTilemaps.Clear();
        _usingTilemaps.Clear();
        _currentPlayerIdx = Vector2Int.zero;
    }
}
