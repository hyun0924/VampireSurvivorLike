using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CharacterSelection : UI_Base
{
    enum Images
    {
        IconImage,
        ShovelImage,
        GunImage
    }

    enum Texts
    {
        NameText,
        DescText
    }

    private int _index;
    private string _name;
    private string _description;
    private string _unlockCondition;
    private Color32 _color;

    protected override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));


        Image iconImage = GetImage((int)Images.IconImage);
        Image shovelImage = GetImage((int)Images.ShovelImage);
        Image gunImage = GetImage((int)Images.GunImage);
        TextMeshProUGUI descText = GetText((int)Texts.DescText);

        iconImage.sprite = Managers.Resource.LoadSubSprite($"Farmer_{_index}", "Stand 0");
        GetText((int)Texts.NameText).text = _name;
        shovelImage.sprite = Managers.Resource.LoadSubSprite("UI", $"Select {_index}");
        gunImage.sprite = Managers.Resource.LoadSubSprite("UI", $"Select {_index + 3}");

        if (Managers.Game.IsLocked[_index])
        {
            GetComponent<Image>().color = new Color32(145, 145, 145, 255);
            iconImage.color = Color.black;
            shovelImage.color = Color.black;
            gunImage.color = Color.black;
            descText.text = _unlockCondition;
        }
        else
        {
            GetComponent<Image>().color = _color;
            descText.text = _description;
        }
    }

    public void SetInfo(Stat.PlayerInfo info)
    {
        _index = info.idx;
        _name = info.name;
        _description = info.desc;
        _unlockCondition = info.unlock;

        _color = new Color32((byte)info.color[0], (byte)info.color[1], (byte)info.color[2], 255);

        BindEvent(gameObject, SelectCharacter);
    }

    private void SelectCharacter(PointerEventData evt)
    {
        if (Managers.Game.IsLocked[_index])
        {
            Managers.Sound.Play(Define.Audio.Range);
            return;
        }
        Managers.Sound.Play(Define.Audio.Select);
        Managers.Game.CurrentPlayerType = (Define.Player)_index;
        Managers.UI.ClosePopupUI();
        Managers.Scene.LoadScene(Define.Scene.Game);
    }
}