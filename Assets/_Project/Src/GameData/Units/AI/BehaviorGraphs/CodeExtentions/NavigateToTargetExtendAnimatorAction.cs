using System;
using GameData.Units.AI.BehaviorGraphs.CodeExtentions;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using UnityEngine.Serialization;

[Serializable, GeneratePropertyBag]
[NodeDescription(                                                        name: "NavigateToTargetExtendAnimator",
    story: "[Agent] Navigates to [target] change [animator] value [to]", category: "Action",
    id: "916587d3f8fb2c62f01ccbac33176a89")]
public partial class NavigateToTargetExtendAnimatorAction : Action
{
    [FormerlySerializedAs("Agent")] [SerializeReference]
    public BlackboardVariable<GameObject> agent;

    [FormerlySerializedAs("Target")] [SerializeReference]
    public BlackboardVariable<GameObject> target;

    [FormerlySerializedAs("Speed")] [SerializeReference]
    public BlackboardVariable<float> speed = new BlackboardVariable<float>(1.0f);

    [FormerlySerializedAs("DistanceThreshold")] [SerializeReference]
    public BlackboardVariable<float> distanceThreshold = new BlackboardVariable<float>(0.2f);

    [FormerlySerializedAs("Animator")] [SerializeReference]
    public BlackboardVariable<Animator> animator;

    [FormerlySerializedAs("AnimatorSpeedParam")] [SerializeReference]
    public BlackboardVariable<string> animatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");

    // This will only be used in movement without a navigation agent.
    [FormerlySerializedAs("SlowDownDistance")] [SerializeReference]
    public BlackboardVariable<float> slowDownDistance = new BlackboardVariable<float>(1.0f);

    private NavMeshAgent _mNavMeshAgent;
    private Animator _mAnimator;
    private float _mPreviousStoppingDistance;
    private Vector3 _mLastTargetPosition;
    private Vector3 _mColliderAdjustedTargetPosition;
    private float _mColliderOffset;

    protected override Status OnStart()
    {
        if (agent.Value == null || target.Value == null)
        {
            return Status.Failure;
        }

        return Initialize();
    }

    protected override Status OnUpdate()
    {
        if (agent.Value == null || target.Value == null)
        {
            return Status.Failure;
        }

        // Check if the target position has changed.
        var boolUpdateTargetPosition =
            !Mathf.Approximately(_mLastTargetPosition.x, target.Value.transform.position.x) ||
            !Mathf.Approximately(_mLastTargetPosition.y, target.Value.transform.position.y) ||
            !Mathf.Approximately(_mLastTargetPosition.z, target.Value.transform.position.z);
        if (boolUpdateTargetPosition)
        {
            _mLastTargetPosition = target.Value.transform.position;
            _mColliderAdjustedTargetPosition = GetPositionColliderAdjusted();
        }

        var distance = GetDistanceXZ();
        if (distance <= (distanceThreshold + _mColliderOffset))
        {
            return Status.Success;
        }

        if (_mNavMeshAgent != null)
        {
            if (boolUpdateTargetPosition)
            {
                _mNavMeshAgent.SetDestination(_mColliderAdjustedTargetPosition);
            }

            if (_mNavMeshAgent.IsNavigationComplete())
            {
                return Status.Success;
            }
        }
        else
        {
            float speed = this.speed;

            if (slowDownDistance > 0.0f && distance < slowDownDistance)
            {
                var ratio = distance / slowDownDistance;
                speed = Mathf.Max(0.1f, this.speed * ratio);
            }

            var agentPosition = agent.Value.transform.position;
            var toDestination = _mColliderAdjustedTargetPosition - agentPosition;
            toDestination.y = 0.0f;
            toDestination.Normalize();
            agentPosition += toDestination * (speed * Time.deltaTime);
            agent.Value.transform.position = agentPosition;

            // Look at the target.
            agent.Value.transform.forward = toDestination;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (_mAnimator != null)
        {
            _mAnimator.SetFloat(animatorSpeedParam, 0);
        }

        if (_mNavMeshAgent != null)
        {
            if (_mNavMeshAgent.isOnNavMesh)
            {
                _mNavMeshAgent.ResetPath();
            }

            _mNavMeshAgent.stoppingDistance = _mPreviousStoppingDistance;
        }

        _mNavMeshAgent = null;
        _mAnimator = null;
    }

    protected override void OnDeserialize()
    {
        Initialize();
    }

    private Status Initialize()
    {
        _mLastTargetPosition = target.Value.transform.position;
        _mColliderAdjustedTargetPosition = GetPositionColliderAdjusted();

        // Add the extents of the colliders to the stopping distance.
        
        _mColliderOffset = 0.0f;
        var agentCollider = agent.Value.GetComponentInChildren<Collider>();
        if (agentCollider != null)
        {
            var colliderExtents = agentCollider.bounds.extents;
            _mColliderOffset += Mathf.Max(colliderExtents.x, colliderExtents.z);
        }

        if (GetDistanceXZ() <= (distanceThreshold + _mColliderOffset))
        {
            return Status.Success;
        }

        // If using animator, set speed parameter.
        _mAnimator = animator.Value;
        if (_mAnimator != null)
        {
            _mAnimator.SetFloat(animatorSpeedParam, speed);
        }

        // If using a navigation mesh, set target position for navigation mesh agent.
        _mNavMeshAgent = agent.Value.GetComponentInChildren<NavMeshAgent>();
        if (_mNavMeshAgent != null)
        {
            if (_mNavMeshAgent.isOnNavMesh)
            {
                _mNavMeshAgent.ResetPath();
            }

            _mNavMeshAgent.speed = speed;
            _mPreviousStoppingDistance = _mNavMeshAgent.stoppingDistance;

            _mNavMeshAgent.stoppingDistance = distanceThreshold + _mColliderOffset;
            _mNavMeshAgent.SetDestination(_mColliderAdjustedTargetPosition);
        }

        return Status.Running;
    }


    private Vector3 GetPositionColliderAdjusted()
    {
        var targetCollider = target.Value.GetComponentInChildren<Collider>();
        if (targetCollider != null)
        {
            return targetCollider.ClosestPoint(agent.Value.transform.position);
        }

        return target.Value.transform.position;
    }

    private float GetDistanceXZ()
    {
        var agentPosition = new Vector3(agent.Value.transform.position.x, _mColliderAdjustedTargetPosition.y,
            agent.Value.transform.position.z);
        return Vector3.Distance(agentPosition, _mColliderAdjustedTargetPosition);
    }
}