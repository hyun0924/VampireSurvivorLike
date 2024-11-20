using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UpgradeSelection : UI_Base
{
    enum Images
    {
        IconImage,
    }

    enum Texts
    {
        LevelText,
        NameText,
        DescText,
    }

    string _name;
    string _desc;
    string _level;
    string _imageName;

    Define.PlayerLevel _type = Define.PlayerLevel.MaxCount;

    protected override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        BindEvent(gameObject, OnClick);

        GetText((int)Texts.LevelText).text = _level;
        GetText((int)Texts.NameText).text = _name;
        GetText((int)Texts.DescText).text = _desc;
        GetImage((int)Images.IconImage).sprite = Managers.Resource.LoadSubSprite("UI", _imageName);
    }

    public void SetInfo(Define.PlayerLevel type)
    {
        Stat.weaponSelection weaponData = null;
        Stat.statSelection statData = null;
        _type = type;
        _level = $"Lv.{Managers.Game.Player.Levels[(int)type]}";
        string typeName = Enum.GetName(typeof(Define.PlayerLevel), type);

        if (type == Define.PlayerLevel.Shovel)
            weaponData = Managers.Data.WeaponSelectionDict[Enum.GetName(typeof(Define.Shovel), (int)Managers.Game.CurrentPlayerType)];
        else if (type == Define.PlayerLevel.Gun)
            weaponData = Managers.Data.WeaponSelectionDict[Enum.GetName(typeof(Define.Gun), (int)Managers.Game.CurrentPlayerType)];
        else
            statData = Managers.Data.StatSelectionDict[typeName];

        if (weaponData != null)
        {
            _name = weaponData.name;
            _desc = weaponData.descriptions[Managers.Game.Player.Levels[(int)type]].description;
            _imageName = weaponData.imageName;
        }
        else if (statData != null)
        {
            _name = statData.name;
            _desc = statData.description;
            _imageName = statData.imageName;
        }
    }

    private void OnClick(PointerEventData data)
    {
        Managers.Sound.Play(Define.Audio.Select);

        Managers.Game.Player.LevelUp(_type);

        Managers.UI.ClosePopupUI();
        Time.timeScale = 1.0f;
    }
}
