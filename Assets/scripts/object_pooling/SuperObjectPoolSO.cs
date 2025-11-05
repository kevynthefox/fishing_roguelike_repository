using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

namespace GDK
{
    [CreateAssetMenu(menuName = "Tutorial/Super Object Pool", fileName = "SuperObjectPool")]
    public class SuperObjectPoolSO : ScriptableObject
    {
        public PoolableMonoBehaviour prefab;
        public int defaultCapacity = 10;
        [Space]
        public Vector3 defaultSpawnLocation;
        [Space]
        public bool useDefaultSpawnRotation;
        public Quaternion defaultSpawnRotation;

        private ObjectPool<PoolableMonoBehaviour> objectPool;

        private void OnEnable() {
            objectPool = new ObjectPool<PoolableMonoBehaviour>(
                CreatePooledObject,
                OnTakeFromPool,
                OnReturnPool,
                OnDestroyObject,
                false,
                defaultCapacity);
        }

        private PoolableMonoBehaviour CreatePooledObject() {
            PoolableMonoBehaviour pm = Instantiate(prefab, defaultSpawnLocation, useDefaultSpawnRotation ? defaultSpawnRotation : Quaternion.identity);
            pm.gameObject.SetActive(true);
            pm.RegisterPool(this);
            pm.OnObjectPoolCreate();
            return pm;
        }

        private void OnTakeFromPool(PoolableMonoBehaviour pm) {
            pm.gameObject.SetActive(true);
            pm.OnObjectPoolTake();
        }

        private void OnReturnPool(PoolableMonoBehaviour pm) {
            pm.OnObjectPoolReturn();
            pm.gameObject.SetActive(false);
        }

        private void OnDestroyObject(PoolableMonoBehaviour pm) {
            pm.OnObjectPoolDestroy();
            Destroy(pm);
        }

        public PoolableMonoBehaviour Get() {
            return objectPool.Get();
        }

        public void Release(PoolableMonoBehaviour pm) {
            objectPool.Release(pm);
        }
    }

    public abstract class PoolableMonoBehaviour : MonoBehaviour
    {
        public UnityAction<PoolableMonoBehaviour> onRelease;
        
        private SuperObjectPoolSO pool;
        
        public void RegisterPool(SuperObjectPoolSO pool) {
            this.pool = pool;
        }

        public void Release() {
            if (pool != null) {
                this.pool.Release(this);
            }
            if (onRelease != null) {
                onRelease(this);
                onRelease = null;
            }
        }

        public virtual void OnObjectPoolCreate() {}
        public virtual void OnObjectPoolTake() {}
        public virtual void OnObjectPoolReturn() {} // Called before getting deactivated
        public virtual void OnObjectPoolDestroy() {} // Called before getting destroyed
        public virtual void OnPostGet() {} // Idea

    }

}
