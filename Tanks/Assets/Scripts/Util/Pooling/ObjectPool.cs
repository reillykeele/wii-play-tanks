using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util.Helpers;

namespace Util.Pooling
{
    public class ObjectPool
    {
        private PoolableObject _prefab;
        private Queue<PoolableObject> _pool;
        private GameObject _poolGameObject;

        private readonly bool _createNewObjects;

        public ObjectPool(PoolableObject prefab, int poolSize, bool createNewObjects = true)
        {
            _prefab = prefab;
            _pool = new Queue<PoolableObject>(poolSize);
            _createNewObjects = createNewObjects;

            _poolGameObject = new GameObject(prefab.name + " Pool");

            CreateObjects(poolSize);
        }

        private void CreateObjects(int poolSize)
        {
            for (var i = 0; i < poolSize; i++)
            {
                var poolableObject = GameObject.Instantiate(_prefab, Vector3.zero, Quaternion.identity, _poolGameObject.transform);
                poolableObject.Parent = this;
                poolableObject.gameObject.Disable();
            }
        }

        public void ReturnObjectToPool(PoolableObject poolableObject)
        {
            _pool.Enqueue(poolableObject);
        }

        public PoolableObject GetObject()
        {
            if (_pool.Count > 0)
            {
                var instance = _pool.Dequeue();
                
                instance.gameObject.Enable();
                
                return instance;
            }
            else if (_createNewObjects)
            {
                // TODO: finish this
                var poolableObject = GameObject.Instantiate(_prefab, Vector3.zero, Quaternion.identity, _poolGameObject.transform);
                poolableObject.Parent = this;
                return poolableObject;
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
