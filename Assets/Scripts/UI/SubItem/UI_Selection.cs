using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Selection : UI_Base
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
            weaponData = Managers.Data.WeaponSelectionDict[typeName];
        else if (type == Define.PlayerLevel.Gun)
            weaponData = Managers.Data.WeaponSelectionDict[Enum.GetName(typeof(Define.GunType), (int)Managers.Game.CurrentPlayerType)];
        else
            statData = Managers.Data.StatSelectionDict[typeName];

        if (weaponData != null)
        {
            _name = weaponData.name;
            _desc = weaponData.descriptions[0].description; // TODO: 각자 레벨 알아야함
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
        Managers.Game.Player.LevelUp(_type);

        Managers.Resource.Destroy(transform.parent.parent.gameObject);
        Time.timeScale = 1.0f;
    }
}
