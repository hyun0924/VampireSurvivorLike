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
        NameText,
        DescText,
    }

    string _name;
    string _desc;
    string _imageName;

    Define.WeaponType _weaponType = Define.WeaponType.MaxCount;
    Define.StatType _statType = Define.StatType.MaxCount;

    protected override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        BindEvent(gameObject, OnClick);

        GetText((int)Texts.NameText).text = _name;
        GetText((int)Texts.DescText).text = _desc;
        GetImage((int)Images.IconImage).sprite = Managers.Resource.LoadSubSprite("UI", _imageName);
    }

    public void SetType(Define.WeaponType type)
    {
        Stat.weaponSelection data = Managers.Data.WeaponSelectionDict[Enum.GetName(typeof(Define.WeaponType), type)];
        _weaponType = type;
        _name = data.name;
        _desc = data.descriptions[0].description; // TODO: 각자 레벨 알아야함
        _imageName = data.imageName;
    }
    public void SetType(Define.StatType type)
    {
        Stat.statSelection data = Managers.Data.StatSelectionDict[Enum.GetName(typeof(Define.StatType), type)];
        _statType = type;
        _name = data.name;
        _desc = data.description;
        _imageName = data.imageName;
    }

    private void OnClick(PointerEventData data)
    {
        if (_statType != Define.StatType.MaxCount)
        {
            Managers.Game.Player.StatLevelUp(_statType);
        }
        else
        {
            switch (_weaponType)
            {
                case Define.WeaponType.Shovel:
                    Managers.Game.Player.ShovelLevelUp();
                    break;
                case Define.WeaponType.Shotgun:
                    Managers.Game.Player.ShotgunLevel++;
                    break;
            }
        }

        Managers.Resource.Destroy(transform.parent.parent.gameObject);
        Time.timeScale = 1.0f;
    }
}
