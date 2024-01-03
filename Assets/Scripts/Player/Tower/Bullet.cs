using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerController playerController;
    public GameObject BoomEffect;
    public AudioSource ShootSound;

    void Start()
    {
        playerController = GameObject.Find("Tank_Player").GetComponent<PlayerController>();
        ShootSound = GetComponent<AudioSource>();

    }
    void OnEnable()
    {
        ShootSound.Play();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            //Instantiate(BoomEffect, transform.position, Quaternion.identity);
            Effect_Pool.GetObject(transform.position);
            DestroyBullet();
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            Effect_Pool.GetObject(transform.position);
            DestroyBullet();
        }
        if (other.gameObject.CompareTag("Player"))
        {
            playerController.currentPlayerHp -= 30f;
            Effect_Pool.GetObject(transform.position);
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        Bullet_Pool.ReturnObject(this);
    }


}
