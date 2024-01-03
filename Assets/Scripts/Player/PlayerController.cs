using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("탱크 이동속도")]
    public float speed = 10f;

    [Header("탱크 체력")]
    public float MaxPlayerHp = 100f;
    private float CurrentPlayerHp = 100f;
    public float currentPlayerHp
    {
        get { return CurrentPlayerHp; }
        set { CurrentPlayerHp = value; }
    }

    [Header("체력")]
    public Slider HPSlider;
    public TMPro.TextMeshProUGUI HPText;

    [Header("탱크 타워 회전을 위한 컴포넌트")]
    public GameObject tankTowerObj;
    public Vector3 mousePosition;
    public Camera cam;
    Rigidbody rb;
    Vector3 dir = Vector3.zero;

    [Header("탱크 움직임 이펙트")]
    public ParticleSystem TankMoveEffect_L;
    public ParticleSystem TankMoveEffect_R;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        HPSlider.value = CurrentPlayerHp / MaxPlayerHp;// 나누기 MaxPlayerHp(100) 이유 = Hp바에 최대 value가 1이라서
        HPText.text = CurrentPlayerHp + "/" + MaxPlayerHp;
    }

    // Update is called once per frame
    void Update()
    {
        InputAndDir();
        TankTower();
        HPSlider.value = CurrentPlayerHp / MaxPlayerHp;
        HPText.text = CurrentPlayerHp + "/" + MaxPlayerHp;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
    }

    void TankTower()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Vector3 mousepos = new Vector3(hit.point.x, tankTowerObj.transform.position.y, hit.point.z);
            tankTowerObj.transform.LookAt(mousepos);
        }
    }

    void InputAndDir()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        if (dir != Vector3.zero)
        {
            transform.forward = dir;
            //if (!TankMoveEffect_L.isPlaying && !TankMoveEffect_L.isPlaying)
            //{
            //    TankMoveEffect_L.Play();
            //    TankMoveEffect_R.Play();
            //}
        }
        else
        {

            //TankMoveEffect_L.Stop();
            //TankMoveEffect_R.Stop();

        }
    }
}
