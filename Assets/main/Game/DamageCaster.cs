using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CampType //阵营
{
    Player,
    Enemy
}

public class DamageCaster : MonoBehaviour
{
    public BoxCollider _box;
    public EnemyBase enemyBase; //敌人基类
    public CampType currentCamp; //当前的阵容类型



    private void Awake()
    {
        _box = GetComponent<BoxCollider>();
        _box.enabled = false;

        if (currentCamp == CampType.Enemy)
        {
            enemyBase = GetComponentInParent<EnemyBase>();
            if (enemyBase == null)
            {
                Debug.LogWarning($"{name} 找不到 EnemyBase，确认层级是否正确");
            }
        }
    }

    
   


    //进入触发器
    private void OnTriggerEnter(Collider other)
    {
        if (currentCamp == CampType.Player && other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.Hurt(Player.Instance.AttackPower);
            }
            return;
        }

        if (currentCamp == CampType.Enemy && other.CompareTag("Player") && enemyBase != null)
        {
            Player.Instance.Hurt(enemyBase.attackPower);
        }
    }

}
