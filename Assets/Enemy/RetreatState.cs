using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : BaseState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("Start Retreating");
        enemy.Animator.SetTrigger("RetreatState"); // Tambahkan trigger animator
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.Player != null)
        {
            // Arah menjauh dari player dikalikan angka agar tujuannya jauh
            Vector3 runDirection = (enemy.transform.position - enemy.Player.transform.position).normalized;
            Vector3 targetDestination = enemy.transform.position + (runDirection * 10f); // Lari sejauh 10 meter
            
            enemy.NavMeshAgent.destination = targetDestination;
        }
    }

    public void ExitState(Enemy enemy) => Debug.Log("Stop Retreating");
}