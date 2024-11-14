using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;
    private DataManager _data = new DataManager();
    private InputManager _input = new InputManager();
    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();
    private TileMapManager _tileMap = new TileMapManager();
    private UIManager _ui = new UIManager();

    private GameManagerEx _game = new GameManagerEx();

    public static Managers Instance { get { Init(); return _instance; } }
    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static TileMapManager TileMap {  get { return Instance._tileMap; } }
    public static UIManager UI { get { return Instance._ui; } }

    public static GameManagerEx Game { get { return Instance._game; } }

    private static void Init()
    {
        if (_instance == null)
        {
            GameObject go = new GameObject { name = "@Managers" };
            DontDestroyOnLoad(go);

            _instance = go.GetOrAddComponent<Managers>();

            _instance._data.Init();
            _instance._tileMap.Init();
            _instance._pool.Init();
        }
    }

    private void Update()
    {
        _input.UpdateInput();
    }
}
