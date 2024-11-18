using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : BaseStat
{
    [SerializeField] protected int _level;
    [SerializeField] protected int _exp;
    [SerializeField] protected int _totalExp;
    [SerializeField] protected int _cooldown;

    public int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            if (Managers.UI.SceneUI is UI_Game)
                (Managers.UI.SceneUI as UI_Game).SetText(UI_Game.Texts.LevelText, $"Lv. {_level}");
            if (Managers.Data.ExpStatsDict.ContainsKey(_level + 1))
                _totalExp = Managers.Data.ExpStatsDict[_level + 1].totalExp;
        }
    }
    public int Exp
    {
        get { return _exp; }
        set
        {
            _exp = value;
            while (_totalExp <= _exp)
            {
                _exp -= _totalExp;
                Level++;
                Managers.UI.ShowPopupUI<UI_Upgrade>();
                Time.timeScale = 0;
            }
            (Managers.UI.SceneUI as UI_Game).SetExpSlider((float)_exp / _totalExp);
        }
    }
    public int TotalExp { get { return _totalExp; } set { _totalExp = value; } }
    public int Cooldown {  get { return _cooldown; } set { _cooldown = value; } }

    private void Start()
    {
        Level = 1;

        _speed = 5f;
        _maxHp = 20;
        _hp = 20;
        _damage = 1;
        _hitDuration = 0.2f;
        _exp = 0;
        _cooldown = 0;
    }
}
