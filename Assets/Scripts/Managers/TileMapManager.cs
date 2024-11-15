using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class TileMapManager
{
    GameObject _grid;
    Tilemap[] _tilemaps;

    Collider2D[] lastColliders = null;

    Stack<GameObject> _unusedTilemaps = new Stack<GameObject>();

    TileBase tileBase;

    int _pixelSize = 108;

    public void Init()
    {
        _grid = new GameObject { name = "@TileMap" };
        _grid.GetOrAddComponent<Grid>();

        _tilemaps = new Tilemap[4];
        tileBase = Managers.Resource.Load<TileBase>("TileMap/TileBase");

        GenerateTileMaps();
    }

    private void GenerateTileMaps()
    {
        int width = Screen.width / _pixelSize + 1;
        int height = Screen.height / _pixelSize + 1;
        for (int k = 0; k < _tilemaps.Length; k++)
        {
            GameObject go = Managers.Resource.Instantiate("Tilemap", _grid.transform);
            go.name = $"Tilemap {k}";
            _tilemaps[k] = go.GetComponent<Tilemap>();
            _tilemaps[k].size = new Vector3Int(width, height);

            BoxCollider2D collider = go.GetOrAddComponent<BoxCollider2D>();
            collider.size = new Vector2Int(width, height);
            collider.offset = new Vector2(width / 2.0f, height / 2.0f);
            collider.isTrigger = true;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _tilemaps[k].SetTile(new Vector3Int(i, j), tileBase);
                }
            }

            go.transform.position = new Vector3(-width, -height, 0) + new Vector3(k % 2 * width, k / 2 * height, 0);
        }
    }

    public void UpdateTileMap(Vector3 playerPos, Vector3 moveVec)
    {
        int width = Screen.width / _pixelSize + 1;
        int height = Screen.height / _pixelSize + 1;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(playerPos, new Vector3(width, height), 0, LayerMask.GetMask("Tile"));

        if (lastColliders != null)
        {
            foreach (Collider2D col in lastColliders.Except(hitColliders))
            {
                col.gameObject.SetActive(false);
                _unusedTilemaps.Push(col.gameObject);
            }

            if (hitColliders.Length > 1 && _unusedTilemaps.Count >= 2)
            {
                int startX = int.MaxValue;
                int startY = int.MaxValue;

                if (hitColliders[1].transform.position.y == hitColliders[0].transform.position.y) // 가로
                {
                    startX = (int)Math.Min(hitColliders[0].transform.position.x, hitColliders[1].transform.position.x);
                    startY = (int)hitColliders[0].transform.position.y;
                    startY += height * ((playerPos.y > startY + height / 2.0f) ? 1 : -1);

                    GameObject tilemap = _unusedTilemaps.Pop();
                    tilemap.transform.position = new Vector3(startX + width, startY, 0);
                    tilemap.SetActive(true);

                    tilemap = _unusedTilemaps.Pop();
                    tilemap.transform.position = new Vector3(startX, startY, 0);
                    tilemap.SetActive(true);
                }
                else // 세로
                {
                    startX = (int)hitColliders[0].transform.position.x;
                    startX += width * ((playerPos.x > startX + width / 2.0f) ? 1 : -1);
                    startY = (int)Math.Min(hitColliders[0].transform.position.y, hitColliders[1].transform.position.y);

                    GameObject tilemap = _unusedTilemaps.Pop();
                    tilemap.transform.position = new Vector3(startX, startY + height, 0);
                    tilemap.SetActive(true);

                    tilemap = _unusedTilemaps.Pop();
                    tilemap.transform.position = new Vector3(startX, startY, 0);
                    tilemap.SetActive(true);
                }
            }
            else if (hitColliders.Length == 1 || hitColliders.Length == 3)
            {
                Debug.Log("Tilemap error");
            }
        }

        lastColliders = new Collider2D[hitColliders.Length];
        Array.Copy(hitColliders, lastColliders, hitColliders.Length);
    }
}
