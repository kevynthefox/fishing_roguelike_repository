using System;
using UnityEngine;
using UnityEngine.Pool;

namespace GDK
{/*
    public class PoolableEnemySpawner : MonoBehaviour
    {
        public GameObject enemy_pool_obj;

        [SerializeField] private behavior_for_ranged_fish enemyPrefab;
        [SerializeField] private EnemyConfigSO[] enemyConfigs;

        [Header("Values")]
        [SerializeField] private float delay;
        [SerializeField] private float spawnRate;

        [Header("Pool")]
        [SerializeField] private int initialPoolSize = 10;

        private ObjectPool<behavior_for_ranged_fish> enemyPool;
        private float nextSpawn;

        private void Awake()
        {
            nextSpawn = Time.time + delay;

            enemyPool = new ObjectPool<behavior_for_ranged_fish>(
                createFunc: CreateEnemyObject,
                actionOnGet: OnEnemyTakeFromPool,
                actionOnRelease: OnEnemyRelease,
                actionOnDestroy: OnEnemyDestroy,
                collectionCheck: false,
                defaultCapacity: initialPoolSize
                );
        }

        private void Update()
        {
            if (Time.time >= nextSpawn)
            {
                nextSpawn = Time.time + spawnRate;
                Spawn();
            }
        }

        private void Spawn()
        {
            behavior_for_ranged_fish newEnemy = enemyPool.Get();
            transform.GetPositionAndRotation(out Vector3 pos, out Quaternion rot);
            newEnemy.transform.SetPositionAndRotation(pos,rot);
            newEnemy.Configure(enemyConfigs[UnityEngine.Random.Range(0, enemyConfigs.Length)]);
            newEnemy.onDeath += OnEnemyDeath;
        }

        private void OnEnemyDeath(behavior_for_ranged_fish enemy)
        {
            enemyPool.Release(enemy);
        }

        //object pooling

        private behavior_for_ranged_fish CreateEnemyObject()
        {
            behavior_for_ranged_fish enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            enemy.gameObject.SetActive(true);
            enemy.gameObject.transform.SetParent(enemy_pool_obj.transform, true);
            return enemy;
        } //this is for when the pool is empty

        private void OnEnemyTakeFromPool(behavior_for_ranged_fish enemy)
        {
            enemy.gameObject.SetActive(true);
        }    

        private void OnEnemyDestroy(behavior_for_ranged_fish enemy)
        {
            Destroy(enemy);
        }

        private void OnEnemyRelease(behavior_for_ranged_fish enemy)
        {
            enemy.onDeath -= OnEnemyDeath;
            enemy.gameObject.SetActive(false); //you gain performance here
        }
    }*/
}