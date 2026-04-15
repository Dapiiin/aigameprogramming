using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // PASTIKAN BAGIAN INI HANYA ADA SATU
    [SerializeField] public List<Transform> Waypoints = new List<Transform>();

    [SerializeField] public float ChaseDistance; 
    [SerializeField] public Player Player;

    private BaseState _currentState;

    [HideInInspector] public PatrolState PatrolState = new PatrolState();
    [HideInInspector] public ChaseState ChaseState = new ChaseState();
    [HideInInspector] public RetreatState RetreatState = new RetreatState();
    [HideInInspector] public NavMeshAgent NavMeshAgent;
    [HideInInspector] public Animator Animator;

    public void SwitchState(BaseState state)
    {
        if (_currentState != null) _currentState.ExitState(this);
        _currentState = state;
        _currentState.EnterState(this);
    }
    
    public void Dead()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>(); 
        _currentState = PatrolState;
    }

    private void Start()
    {
        if (Player != null)
        {
            Player.OnPowerUpStart += StartRetreating;
            Player.OnPowerUpStop += StopRetreating;
        }

        if (_currentState != null) _currentState.EnterState(this);
    }

    private void Update()
    {
        if (_currentState != null) _currentState.UpdateState(this);
    }

    private void StartRetreating() => SwitchState(RetreatState);
    private void StopRetreating() => SwitchState(PatrolState);

    private void OnCollisionEnter(Collision collision)
    {
        if (_currentState != RetreatState)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Player>().Dead();
            }
        }
    }
}