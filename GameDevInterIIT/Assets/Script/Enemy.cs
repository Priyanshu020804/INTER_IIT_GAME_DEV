using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    Animator animator;
    private string[] DamageAnims = { "Hit_01", "Hit_02" };

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        if (currentHealth > 0 && damage>0)
        {
            int index = UnityEngine.Random.Range(0, DamageAnims.Length);
            animator.SetTrigger(DamageAnims[index]);

        }

        if(currentHealth <= 0){
            Die();
        }

    }

    void Die(){
        Debug.Log("Ded");
    }

}