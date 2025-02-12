using System;
using System.Collections.Generic;
using Common;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Cms
{
    private static CmsTable<CmsEntity> _all = new CmsTable<CmsEntity>();

    private static bool _isInit;

    public static void Init()
    {
        if (_isInit)
            return;
        _isInit = true;

        AutoAdd();
    }

    private static void AutoAdd()
    {
        var subs = ReflectionUtil.FindAllSubclasses<CmsEntity>();
        foreach (var subclass in subs)
            _all.Add(Activator.CreateInstance(subclass) as CmsEntity);

        var resources = Resources.LoadAll<CmsEntityPfb>("CMS");
        foreach (var resEntity in resources)
        {
            Debug.Log("LOAD ENTITY " + resEntity.GetId());
            _all.Add(new CmsEntity
            {
                ID = resEntity.GetId(),
                Components = resEntity.Components
            });
        }
    }

    public static T Get<T>(string defID = null) where T : CmsEntity
    {
        if (defID == null)
            defID = E.Id<T>();
        var findById = _all.FindById(defID) as T;

        if (findById == null)
        {
            // ok fuck it
            throw new Exception("unable to resolve entity id '" + defID + "'");
        }

        return findById;
    }


    public static T GetData<T>(string defID = null) where T : EntityComponentDefinition, new()
    {
        return Get<CmsEntity>(defID).Get<T>();
    }

    public static List<T> GetAll<T>() where T : CmsEntity
    {
        var allSearch = new List<T>();

        foreach (var a in _all.GetAll())
            if (a is T)
                allSearch.Add(a as T);

        return allSearch;
    }

    public static List<(CmsEntity e, T tag)> GetAllData<T>() where T : EntityComponentDefinition, new()
    {
        var allSearch = new List<(CmsEntity, T)>();

        foreach (var a in _all.GetAll())
            if (a.Is<T>(out var t))
                allSearch.Add((a, t));

        return allSearch;
    }

    public static void Unload()
    {
        _isInit = false;
        _all = new CmsTable<CmsEntity>();
    }
}

public class CmsTable<T> where T : CmsEntity, new()
{
    private List<T> _list = new List<T>();
    private Dictionary<string, T> _dict = new Dictionary<string, T>();

    public void Add(T inst)
    {
        if (inst.ID == null)
            inst.ID = E.Id(inst.GetType());

        _list.Add(inst);
        _dict.Add(inst.ID, inst);
    }

    public T New(string id)
    {
        var t = new T();
        t.ID = id;
        _list.Add(t);
        _dict.Add(id, t);
        return t;
    }

    public List<T> GetAll()
    {
        return _list;
    }

    public T FindById(string id)
    {
        return _dict.GetValueOrDefault(id);
    }

    public T2 FindByType<T2>() where T2 : T
    {
        foreach (var v in _list)
            if (v is T2 v2)
                return v2;
        return null;
    }
}

public partial class CmsEntity
{
    public string ID;

    public List<EntityComponentDefinition> Components = new List<EntityComponentDefinition> { new AnythingTag() };

    public T Define<T>() where T : EntityComponentDefinition, new()
    {
        var t = Get<T>();
        if (t != null)
            return t;

        var entityComponent = new T();
        Components.Add(entityComponent);
        return entityComponent;
    }

    public bool Is<T>(out T unknown) where T : EntityComponentDefinition, new()
    {
        unknown = Get<T>();
        return unknown != null;
    }

    public bool Is<T>() where T : EntityComponentDefinition, new()
    {
        return Get<T>() != null;
    }

    public bool Is(Type type)
    {
        return Components.Find(m => m.GetType() == type) != null;
    }

    public T Get<T>() where T : EntityComponentDefinition, new()
    {
        return Components.Find(m => m is T) as T;
    }
}

[Serializable]
public class EntityComponentDefinition
{
}

public static class CmsUtil
{
    public static T Load<T>(this string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public static Sprite LoadFromSpritesheet(string imageName, string spriteName)
    {
        Sprite[] all = Resources.LoadAll<Sprite>(imageName);

        foreach (var s in all)
        {
            if (s.name == spriteName)
            {
                return s;
            }
        }

        return null;
    }
}

public static class E
{
    public static string Id(Type getType)
    {
        return getType.FullName;
    }

    public static string Id<T>()
    {
        return ID<T>.Get();
    }
}

internal static class ID<T>
{
    private static string _cache;

    public static string Get()
    {
        if (_cache == null)
            _cache = typeof(T).FullName;
        return _cache;
    }

    public static string Get<T>()
    {
        return ID<T>.Get();
    }
}