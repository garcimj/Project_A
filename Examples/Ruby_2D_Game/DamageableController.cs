using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableController : MonoBehaviour
{
    //We have deleted the usual start and update functions that are generated
    //because we don't need anything to happen other than when a collision happens
    int damageIncrement = -1;

    void OnTriggerStay2D(Collider2D other)
    {
        //Don't need to use the debug log anymore
        //UnityEngine.Debug.Log("Object that entered the trigger : " + other);

        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.ChangeHealth(damageIncrement);
        }
    }
}
