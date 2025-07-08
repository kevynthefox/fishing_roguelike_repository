using UnityEngine;
using UnityEngine.Pool;

namespace GDK
{
    [CreateAssetMenu(fileName = "ObjectPoolSO", menuName = "Tutorial/Object Pool", order = 0)]
    public class ObjectPoolSO : ScriptableObject
    {
        public GameObject prefab;
        public int defaultCapacity = 10;
        public Vector3 defaultSpawnLocation;
        public bool useDefaultSpawnRotation;
        public Quaternion defaultSpawnRotation;
        
        private ObjectPool<GameObject> objectPool;

        public Transform parent;

        private void OnEnable() {
            objectPool = new ObjectPool<GameObject>(
                CreatePooledObject,
                OnTakeFromPool,
                OnReturnPool,
                OnDestroyObject,
                false,
                defaultCapacity);
        }
        
        private GameObject CreatePooledObject() {
            GameObject go = Instantiate(prefab, defaultSpawnLocation, useDefaultSpawnRotation ? defaultSpawnRotation : Quaternion.identity);
            go.transform.parent = parent;
            go.SetActive(true);

            if ( go.TryGetComponent(out IPoolable poolable) ) {
                poolable.RegisterPool(this);
                poolable.OnPoolObjectCreate();
            }

            return go;
        }

        private void OnTakeFromPool(GameObject go) {
            go.SetActive(true);

            if (go.TryGetComponent(out IPoolable poolable)) {
                poolable.OnPoolObjectTake();
            }
        }

        private void OnReturnPool(GameObject go) {
            if ( go.TryGetComponent(out IPoolable poolable) ) {
                poolable.OnPoolObjectRelease();
            }

            go.SetActive(false);
        }

        private void OnDestroyObject(GameObject go) {
            if ( go.TryGetComponent(out IPoolable poolable) ) {
                poolable.OnPoolObjectDestroy();
            }

            Destroy(go);
        }

        public GameObject Get() {
            return objectPool.Get();
        }

        public void Release(GameObject go) {
            objectPool.Release(go);
        }

    }

}
