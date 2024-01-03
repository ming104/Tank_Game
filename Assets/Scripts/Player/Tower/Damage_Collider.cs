using UnityEngine;

public class Damage_Collider : MonoBehaviour
{
    private SphereCollider Damage_col;

    void Start()
    {
        Damage_col = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        Invoke("collider_off", 0.5f);
    }
    private void collider_off()
    {
        Damage_col.enabled = false;
    }

    private void OnDisable()
    {
        Damage_col.enabled = true;
    }

}
