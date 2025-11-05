using UnityEngine;

namespace GDK
{
    public interface IPoolable
    {
        void RegisterPool(ObjectPoolSO pool);
        void OnPoolObjectCreate();
        void OnPoolObjectTake();
        void OnPoolObjectRelease();
        void OnPoolObjectDestroy();
    }
}

