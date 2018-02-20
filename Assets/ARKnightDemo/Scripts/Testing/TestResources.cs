#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TestResources : MonoBehaviour
{
    [SerializeField]
    List<Object> m_ResourceData = new List<Object>();

    static bool m_ReferencesInitialized;
    static readonly Dictionary<string, Object> m_AssetsByName = new Dictionary<string, Object>();
    static readonly Dictionary<System.Type, List<Object>> m_AssetsByType = new Dictionary<System.Type, List<Object>>();

    static void SetupData()
    {
        if (m_ReferencesInitialized)
            return;

        TestResources inst = Resources.Load<TestResources>("TestResources");

        foreach (Object obj in inst.m_ResourceData)
        {
            // Store asset by name
            m_AssetsByName.Add(obj.name, obj);

            // Store asset by Type
            if (obj is GameObject)
            {
                // Store components for GameObjects (this should include the GameObject component as well)
                Component[] components = (obj as GameObject).GetComponents<Component>();
                foreach (Component comp in components)
                    StoreObjectByType(comp);
            }
            else
            {
                StoreObjectByType(obj);
            }

        }

        m_ReferencesInitialized = true;
    }

    static void StoreObjectByType(Object obj)
    {
        if (m_AssetsByType.ContainsKey(obj.GetType()) == false)
            m_AssetsByType[obj.GetType()] = new List<Object>();

        m_AssetsByType[obj.GetType()].Add(obj);
    }

    /// <summary>
    /// Gets an asset by name
    /// </summary>
    /// <returns>The asset by name.</returns>
    /// <param name="name">Name.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static T GetAssetByName<T>(string name) where T: Object
    {
        SetupData();

        if (m_AssetsByName.ContainsKey(name))
        {
            return m_AssetsByName[name] as T;
        }
        return null;
    }

    /// <summary>
    /// Gets all assets of type
    /// </summary>
    /// <returns>The assets by type.</returns>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static List<Object> GetAssetsByType<T>() where T : Object
    {
        SetupData();

        if (m_AssetsByType.ContainsKey(typeof(T)))
            return m_AssetsByType[typeof(T)];

        return null;
    }

    /// <summary>
    /// Gets the first asset of type
    /// </summary>
    /// <returns>The first asset by type.</returns>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static T GetAssetByType<T>() where T : Object
    {
        SetupData();

        List<Object> assets = GetAssetsByType<T>();
        if (assets != null && assets.Count > 0)
            return assets[0] as T;

        return null;
    }


}
#endif
