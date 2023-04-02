using System;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// A base object pool wrapper providing generic event implementations.
/// 
/// Derive from this class, call InitPool and use Get and Release.
/// </summary>
/// <typeparam name="T">MonoBehaviour object to pool.</typeparam>
public abstract class BasePool<T> : MonoBehaviour where T : MonoBehaviour
{
    private T _prefab;
    
    private ObjectPool<T> _pool;
    private ObjectPool<T> Pool
    {
        get
        {
            if (_pool == null) throw new InvalidOperationException("InitPool must be called before using the ObjectPool.");
            return _pool;
        }
        set => _pool = value;
    }

    protected void InitPool(T prefab, int initial = 10, int max = 20, bool collectionChecks = false)
    {
        _prefab = prefab;
        Pool = new ObjectPool<T>(
            CreateSetup,
            GetSetup,
            ReleaseSetup,
            DestroySetup,
            collectionChecks,
            initial,
            max);
    }

    protected virtual T CreateSetup() => Instantiate(_prefab);
    protected virtual void GetSetup(T obj) => obj.gameObject.SetActive(true);
    protected virtual void ReleaseSetup(T obj) => obj.gameObject.SetActive(false);
    protected virtual void DestroySetup(T obj) => Destroy(obj);

    public T Get() => Pool.Get();
    public void Release(T obj) => Pool.Release(obj);
}