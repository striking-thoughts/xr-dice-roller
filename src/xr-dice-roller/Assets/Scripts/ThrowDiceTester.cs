using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDiceTester : MonoBehaviour
{
    public Dice dice;
    public Rigidbody diceRb;

    public bool trigger;

    public bool lookForValue;

    public float explosiveForce;
    public float explosiveRadius;
    public float updwardsMod;
    public Transform explosionOrigin;

    public float waitCheck = 0.5f;

    private float elapsed;


    void Update()
    {
        if(lookForValue)
        {
            var velocity = diceRb.velocity;
            if(velocity.sqrMagnitude <= 0)
            {
                elapsed += Time.deltaTime;
                if(elapsed > waitCheck)
                {
                    var value = dice.CurrentValue;
                    Debug.Log($"Dice Value is: [{value}]");
                    lookForValue = false;
                }
            }
        }

        if(trigger)
        {
            trigger = false;
            diceRb.AddExplosionForce(explosiveForce, explosionOrigin.position, explosiveRadius, updwardsMod);
            lookForValue = true;
            elapsed = 0f;
        }        
    }
}
