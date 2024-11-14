using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class UIManager
{
    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    public UI_Scene SceneUI = null;
    int order = 10;

    private static GameObject Root
    {
        get { return Util.GetOrCreateGameObject("@UI_Root"); }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort) canvas.sortingOrder = order++;
        else canvas.sortingOrder = 0;
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        SceneUI = go.GetOrAddComponent<T>();
        go.transform.SetParent(Root.transform);

        return SceneUI as T;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        go.transform.SetParent(Root.transform);

        T popup = go.GetOrAddComponent<T>();
        _popupStack.Push(popup);

        return popup;
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count <= 0) return;

        Managers.Resource.Destroy(_popupStack.Pop().gameObject);
        order--;
    }

    public T AddSubItemUI<T>(Transform parent, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
        go.transform.SetParent(parent);

        return go.GetOrAddComponent<T>();
    }

    public T AddWorldSpaceUI<T>(Transform parent, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");
        go.transform.SetParent(parent);

        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = 3;

        return go.GetOrAddComponent<T>();
    }
}
