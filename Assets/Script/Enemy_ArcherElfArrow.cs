using UnityEngine;

public class Enemy_ArcherElfArrow : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsTarget;

    private Collider2D col;
    private Rigidbody2D rb;
    private Entity_Combat combat;

    public bool CanBeCountered => true;

    public void SetUpArrow(float xVelo, Entity_Combat combat)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        this.combat = combat;
        rb.linearVelocity = new Vector2(xVelo, 0);

        if (rb.linearVelocity.x < 0)
        {
            transform.Rotate(0, 180, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if obj is on a layer we want to dmg
        if (((1 << collision.gameObject.layer) & whatIsTarget) != 0)
        {
            // Dmg target
            combat.PerformAttackOnTarget(collision.transform);
            StuckIntoTarget(collision.transform);
        }
    }

    private void StuckIntoTarget(Transform target)
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;

        transform.parent = target;

        Destroy(gameObject, 3f);
    }

    public void HandleCounter()
    {
        // rb.Linearvelocity = new vector2(rb.LinearVelocity.x * -1, 0);
        transform.Rotate(0, 180, 0);

        int enemyLayer = LayerMask.NameToLayer("Enemy");

        whatIsTarget = whatIsTarget | (1 << enemyLayer);    
    }
}
