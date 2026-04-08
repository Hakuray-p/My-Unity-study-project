using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        // Movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);
        anim.SetFloat("Speed", move.magnitude);

        if (move.magnitude > 0.1f)
        {
            transform.forward = move;
            transform.position += move * moveSpeed * Time.deltaTime;
        }

        // Attack
        if (Input.GetMouseButtonDown(0))
            anim.SetTrigger("Attack");

        // Damage
        if (Input.GetKeyDown(KeyCode.H))
            anim.SetTrigger("Hit");

        // Death
        if (Input.GetKeyDown(KeyCode.K))
        {
            isDead = true;
            anim.SetBool("IsDead", true);
            anim.SetTrigger("Die");
        }
    }
}
