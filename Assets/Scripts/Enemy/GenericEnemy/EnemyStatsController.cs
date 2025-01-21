using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatsController : MonoBehaviour
{
    // Start is called before the first frame update
    public Stat speed;
    public Stat damage;
    public Stat attackSpeedCoef;
    public Stat hitPoints;
    public Stat enemyProjectileSpeed;
    void Start()
    {
        if (TryGetComponent<NavMeshAgent>(out NavMeshAgent agent)) agent.speed = speed.generateValue();
        if (TryGetComponent<DamageControllerMelee>(out DamageControllerMelee damageController)) damageController.rawDamage = damage.generateValue();
        if (TryGetComponent<RangeEnemyProjectileAttack>(out RangeEnemyProjectileAttack projectileAttack)) projectileAttack.damage = damage.generateValue();
        if (TryGetComponent<Animator>(out Animator animator)) animator.SetFloat("attackSpeed", attackSpeedCoef.generateValue());
        if (TryGetComponent<EnemyHitPoints>(out EnemyHitPoints enemyHitPoints)) enemyHitPoints.setmaxAndCurrentHp(hitPoints.generateValue());
        if (TryGetComponent<RangeEnemyProjectileAttack>(out RangeEnemyProjectileAttack projectileSpeed)) projectileSpeed.speed = enemyProjectileSpeed.generateValue();
        //Debug.Log(enemyProjectileSpeed.lastGeneratedValue);
    }

    [System.Serializable]
    public struct Stat
    {
        public float maxValue;
        public float minValue;
        public float lastGeneratedValue;
        public float generateValue()
        {
            lastGeneratedValue =  MathF.Round(UnityEngine.Random.Range(minValue, maxValue), 2);
            return lastGeneratedValue;
        }
    }
}
