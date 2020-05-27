using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigibody2d;
    // Using Awake instead of start. Awake is called immediately when the object is created (instantiated)
    void Awake()
    {
        rigibody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rigibody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //we also add a debug log to know what the pojectile touch
        //UnityEngine.Debug.Log("Projectile Collision with" + other.gameObject);

        EnemyController e = other.collider.GetComponent<EnemyController>();
        if(e != null)
        {
            e.Fix();
        }

        Destroy(gameObject);
    }

    void Update()
    {
        //Here magnitude comes from the position vector
        if(transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }
}
