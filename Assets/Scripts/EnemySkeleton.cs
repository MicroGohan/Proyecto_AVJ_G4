using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : EnemyBehaviour
{

    public float speed;
    public float chaseDistance;
    public float stopDistance;
    public GameObject target;
    private float targetDistance;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        targetDistance = Vector2.Distance(transform.position, target.transform.position);
        if(targetDistance < chaseDistance && targetDistance > stopDistance)
        {
            ChasePlayer();

        }else{

            StopChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        if(transform.position.x < target.transform.position.x)
        
            GetComponent<SpriteRenderer>().flipX = false;
        else
            GetComponent<SpriteRenderer>().flipX = true;
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        animator.SetBool("isRunning", true);
    }

    private void StopChasePlayer()
    {
        animator.SetBool("isRunning", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.CompareTag("PlayerSamurai"))
        {
            Vector2 direccionDMG = new Vector2(transform.position.x, 0);
            collision.gameObject.GetComponent<CharacterController>().beingAttacked(direccionDMG, 1);
        }

    }
}
