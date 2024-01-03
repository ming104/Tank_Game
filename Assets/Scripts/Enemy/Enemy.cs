using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyFSM
    {
        Move = 0,
        Attack = 1,
        Dead = 2
    }

    private EnemyFSM enemyFsm;

    Rigidbody rigid;
    Transform target;

    [SerializeField] private GameObject EnemyBulletPos;
    [Header("HP")]
    [SerializeField] private float MaxHP = 100;
    [SerializeField] private float currentHP = 100;
    public float hp
    {
        get { return currentHP; }
        set { currentHP = value; }
    }

    [Header("추격 속도")]
    [SerializeField] private float moveSpeed = 5f;
    [Header("공격 관련")]
    [SerializeField] private float AttackDistance = 13f;
    [SerializeField] private float BulletSpeed = 35f;
    bool isshoot;

    [Header("근접 거리")]
    [SerializeField] private float contactDistance = 10f;
    [Header("최대 거리")]
    [SerializeField] private float MaxcontactDistance = 15f;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        target = GameObject.Find("Tank_Player").transform;
        enemyFsm = EnemyFSM.Move;
        isshoot = false;
        MaxHP = currentHP;
    }

    void Update()
    {
        TargetLook();

        if (currentHP <= 0)
        {
            enemyFsm = EnemyFSM.Dead;
        }
        switch (enemyFsm)
        {
            case EnemyFSM.Move:
                FollowTarget();
                break;
            case EnemyFSM.Attack:
                EnemyAttack();
                break;
            case EnemyFSM.Dead:
                EnemyDead();
                break;
        }
    }

    void FollowTarget()
    {
        if (Vector3.Distance(transform.position, target.position) < MaxcontactDistance) // 최대 거리
        {
            if (Vector3.Distance(transform.position, target.position) < AttackDistance)
            {
                enemyFsm = EnemyFSM.Attack;
            }
            if (Vector3.Distance(transform.position, target.position) > contactDistance) // 근접 거리 
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.smoothDeltaTime);
            }
        }
        else if (Vector3.Distance(transform.position, target.position) > MaxcontactDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * 3 * Time.smoothDeltaTime);
        }
    }

    void EnemyAttack()
    {
        if (isshoot == false)
        {
            isshoot = true;
            var bulletPrefab = Bullet_Pool.GetObject();
            bulletPrefab.transform.position = EnemyBulletPos.transform.position;
            bulletPrefab.transform.rotation = transform.rotation;
            bulletPrefab.GetComponent<Rigidbody>().AddForce(EnemyBulletPos.transform.forward * BulletSpeed, ForceMode.Impulse);
            StartCoroutine(CoolTime());
        }
        enemyFsm = EnemyFSM.Move;
    }

    void EnemyDead()
    {
        GameManager.instance.score += 100;
        Destroy(gameObject);
    }



    void TargetLook()
    {
        transform.LookAt(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage_Field"))
        {
            currentHP -= 30;
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

    private IEnumerator CoolTime()
    {
        yield return new WaitForSeconds(3f);
        isshoot = false;
    }
}
