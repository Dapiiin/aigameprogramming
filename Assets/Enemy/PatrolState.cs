using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    private bool _isMoving;
    private int _lastIndex = -1; // Menyimpan index sebelumnya

    public void EnterState(Enemy enemy)
    {
        _isMoving = false;
    }

    public void UpdateState(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, enemy.Player.transform.position) < enemy.ChaseDistance)
        {
            enemy.SwitchState(enemy.ChaseState);
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
        // Loop agar tidak memilih waypoint yang sama berturut-turut
        do {
            index = UnityEngine.Random.Range(0, enemy.Waypoints.Count);
        } while (index == _lastIndex && enemy.Waypoints.Count > 1);

        _lastIndex = index;
        Vector3 destination = enemy.Waypoints[index].position;
        enemy.NavMeshAgent.destination = destination;
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("Stop Patrol");
    }
}