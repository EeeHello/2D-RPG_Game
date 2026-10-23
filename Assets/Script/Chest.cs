using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX entityVfx => GetComponent<Entity_VFX>();

    [Header("Open deatils")]
    [SerializeField] private Vector2 knockBack;

    public void TakeDamage(float damage, Transform damageDealer)
    {
        entityVfx.PlayOnDamageVfx();
        anim.SetBool("chestOpen", true);
        rb.linearVelocity = knockBack;

        rb.angularVelocity = Random.Range(-200, 200); 
    }


}
