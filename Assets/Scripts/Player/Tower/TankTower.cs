using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankTower : MonoBehaviour
{
    private Camera cam;
    public Transform TankPlayer;
    public Transform bulletPos;
    public Slider Charging;
    public Slider CoolTimeSlider;
    [SerializeField] private float CoolTime;
    public GameObject bullet;
    [SerializeField] private float BulletSpeed;

    public ParticleSystem ShootParticle;

    public float Charge_Speed;

    // Start is called before the first frame update
    void Start()
    {
        Charging.value = 0;
        CoolTimeSlider.value = 0;
        Charging.gameObject.SetActive(false);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CoolTimeSlider.value > 0.99f)
        {
            CoolTimeSlider.gameObject.SetActive(false);
            if (Input.GetKey(KeyCode.Space))
            {
                Charging.gameObject.SetActive(true);
                Charging.value += Charge_Speed * Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Fire();
                Charging.value = 0;
                Charging.gameObject.SetActive(false);
                CoolTimeSlider.gameObject.SetActive(true);
                CoolTimeSlider.value = 0;
            }
        }
        else
        {
            CoolTimeSlider.value += CoolTime * Time.deltaTime;
        }
    }

    void Fire()
    {
        RaycastHit hit;
        if (Charging.value > 0.5f)
        {

            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                var bulletPrefab = Bullet_Pool.GetObject();
                bulletPrefab.transform.position = bulletPos.position;
                Vector3 mousepos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                bulletPrefab.transform.rotation = transform.rotation;
                bulletPrefab.GetComponent<Rigidbody>().AddForce(bulletPos.forward * BulletSpeed * Charging.value, ForceMode.Impulse);
            }
        }
        else
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                var bulletPrefab = Bullet_Pool.GetObject(); // 오브젝트 풀링으로 총알을 받아오고
                bulletPrefab.transform.position = bulletPos.position; // 위치를 bulletpos로 초기화
                Vector3 mousepos = new Vector3(hit.point.x, transform.position.y, hit.point.z); // 마우스의 위치를 받아오고
                bulletPrefab.transform.rotation = transform.rotation; // 타워로 -> 바라보는 방향
                bulletPrefab.GetComponent<Rigidbody>().AddForce(bulletPos.forward * BulletSpeed * 0.5f, ForceMode.Impulse); // 발사
            }
        }
        Instantiate(ShootParticle, bulletPos.transform.position, Quaternion.identity);
    }
}
