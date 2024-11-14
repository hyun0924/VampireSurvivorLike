using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public abstract class UI_Base : MonoBehaviour
{
    public Dictionary<Type, Object[]> Objects { get; protected set; } = new Dictionary<Type, Object[]>();

    private void Start()
    {
        Init();
    }

    protected abstract void Init();

    protected void Bind<T>(Type type) where T : Object
    {
        string[] names = Enum.GetNames(type);
        Object[] objects = new Object[names.Length];
        Objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject)) objects[i] = gameObject.FindChild<Transform>(names[i]).gameObject;
            else objects[i] = gameObject.FindChild<T>(names[i]);
        }
    }

    protected T Get<T>(int index) where T : Object
    {
        if (Objects.ContainsKey(typeof(T)))
            return Objects[typeof(T)][index] as T;

        return null;
    }

    protected GameObject GetGameObject(int index) { return Get<GameObject>(index); }
    protected Image GetImage(int index) { return Get<Image>(index); }
    protected TextMeshProUGUI GetText(int index) { return Get<TextMeshProUGUI>(index); }
    protected Button GetButton(int index) { return Get<Button>(index); }

    protected void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = go.GetOrAddComponent<UI_EventHandler>();

        switch(type)
        {
            case Define.UIEvent.Click:
                evt.OnPointerClickAction += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragAction += action;
                break;
        }
    }
}
