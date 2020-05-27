using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    //We have deleted the usual start and update functions that are generated
    //because we don't need anything to happen other than when a collision happens
    int healthIncrement = 1;

    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        //Don't need to use the debug log anymore
        //UnityEngine.Debug.Log("Object that entered the trigger : " + other);

        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            //Now if we don't need health, it won't be picked up
            if (controller.health < controller.maxHealth)
            {
                controller.ChangeHealth(healthIncrement);
                Destroy(gameObject);
                controller.PlaySound(collectedClip);
            }
            
        }
    }
}
