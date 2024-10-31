using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageControllerMelee : MonoBehaviour
{
    List<PlayerHitPoints> playersInAttackRange = new List<PlayerHitPoints>();
    public float rawDamage = 10;
    public void attackFinished()
    {
        //Debug.Log("Giving damage");
        foreach (PlayerHitPoints player in playersInAttackRange)
        {
            //Debug.Log("Damaging player: " + player.name);
            player.TakeDamage(rawDamage);
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Trigger enetered by: " + col.name);
        if(col.CompareTag("Player")) playersInAttackRange.Add(col.GetComponent<PlayerHitPoints>());
    }

    private void OnTriggerExit(Collider col)
    {
        //Debug.Log("Trigger exited by: " + col.name);
        if (col.CompareTag("Player")) playersInAttackRange.Remove(col.GetComponent<PlayerHitPoints>());
    }

}
