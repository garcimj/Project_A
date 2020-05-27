using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    public float movement_speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;

    int damageIncrement = -1;

    Rigidbody2D rigidbody2d;

    float timer;
    int direction = 1;

    //Now grab the Animator component
    Animator animator;

    bool broken = true;
    public ParticleSystem smokeEffect;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        } //endif

        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y = position.y + movement_speed * Time.deltaTime * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
            
        }
        else
        {
            position.x = position.x + movement_speed * Time.deltaTime * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        } 
        
        //transform.position = position; No longer need this for movement, as we are incorporating physics now
        rigidbody2d.MovePosition(position);

        if(!broken)
        {
            return;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(damageIncrement);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        //plays the fixed animation
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
    }
}
