using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    private bool _isMoving;
    private int _lastIndex = -1;

    public void EnterState(Enemy enemy)
    {
        _isMoving = false;
        enemy.Animator.SetTrigger("PatrolState"); //
    }

    public void UpdateState(Enemy enemy)
    {
        // Cek jarak untuk mengejar
        if (enemy.Player != null && Vector3.Distance(enemy.transform.position, enemy.Player.transform.position) < enemy.ChaseDistance)
        {
            enemy.SwitchState(enemy.ChaseState);
            return;
        }

        if (!_isMoving || (!enemy.NavMeshAgent.pathPending && enemy.NavMeshAgent.remainingDistance <= enemy.NavMeshAgent.stoppingDistance))
        {
            _isMoving = true;
            MoveToNextWaypoint(enemy);
        }
    }

    private void MoveToNextWaypoint(Enemy enemy)
    {
        if (enemy.Waypoints.Count == 0) return;
        int index;
        do {
            index = Random.Range(0, enemy.Waypoints.Count);
        } while (index == _lastIndex && enemy.Waypoints.Count > 1);

        _lastIndex = index;
        enemy.NavMeshAgent.destination = enemy.Waypoints[index].position;
    }

    public void ExitState(Enemy enemy) => Debug.Log("Stop Patrol");
}