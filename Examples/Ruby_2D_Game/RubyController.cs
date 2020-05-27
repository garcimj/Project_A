using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
//using System.Runtime.Hosting;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float movement_speed = 4.0f;

    public int maxHealth = 5;
    public int health { get { return currentHealth; }}
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;
    float projectileForce = 300; //300 expressed in newton units

    //Audio
    private AudioSource audioSource;
    public AudioClip cogBulletClip;
    public AudioClip damagedClip;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Vector2 position = rigidbody2d.position; this is old
        //What we're doing here is storing our vertical and horizontal movement into a vector of length 2
        Vector2 move = new Vector2(horizontal, vertical);

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) //Using approximately, because of computer precision problems with using ==
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize(); //Normalizing here to store directional information from the vector, but excluding vector magnitude as it's unimportant for this usage
        }

        //Send data to the animator
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        //deltaTime is a movement adjustment which makes the game handle identically despite the chance of computers having different framerates.
        //position.x = position.x + movement_speed * horizontal * Time.deltaTime;
        //position.y = position.y + movement_speed * vertical * Time.deltaTime;
        //transform.position = position; No longer need this for movement, as we are incorporating physics now
        
        //Now moving with some great animation
        Vector2 position = rigidbody2d.position;
        position = position + move * movement_speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0) isInvincible = false; //endif
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));

            if(hit.collider != null)
            {
                //UnityEngine.Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();

                if(character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
                //endif

            isInvincible = true;
            invincibleTimer = timeInvincible;

            PlaySound(damagedClip);
        }//endif
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth/(float)maxHealth);
        //UnityEngine.Debug.Log(currentHealth + "/" + maxHealth);
    }
        
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab,
        rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity); //Quarternion.identity means "no rotation"

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, projectileForce);
        PlaySound(cogBulletClip);
        animator.SetTrigger("Launch");
    }
}
