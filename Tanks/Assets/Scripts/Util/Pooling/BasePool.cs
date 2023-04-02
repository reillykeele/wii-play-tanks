using UnityEngine;
using UnityEngine.Rendering;

namespace Util.Pooling
{
    public abstract class BasePool<T> : MonoBehaviour where T : MonoBehaviour
    {
        // TODO: Update to newer Unity version
        // private T _prefab;
        // private ObjectPool<T> _pool;
        //
        // protected void InitPool(T prefab, int initial = 10, int max = 20, bool collectionChecks = false)
        // {
        //     _prefab = prefab;
        //     _pool = new ObjectPool<T>(
        //         CreateSetup,
        //         GetSetup,
        //         ReleaseSetup,
        //         DestroySetup,
        //         collectionChecks,
        //         initial,
        //         max);
        // }
        //
        // protected virtual T CreateSetup() => Instantiate(_prefab);
        // protected virtual void GetSetup(T obj) => obj.gameObject.SetActive(true);
        // protected virtual void ReleaseSetup(T obj) => obj.gameObject.SetActive(false);
        // protected virtual void DestroySetup(T obj) => Destroy(obj);
        //
        // public T Get() => _pool.Get();
        // public void Release(T obj) => _pool.Release(obj);
    }
}
