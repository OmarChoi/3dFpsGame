using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ZombieMovement : MonoBehaviour
{
    private NavMeshAgent _agent;
    private ZombieStats _stats;
    private Coroutine _jumpCoroutine;
    private Coroutine _knockbackCoroutine;
    private const float JumpArcHeight = 1.5f;

    public void Init(NavMeshAgent agent, ZombieStats stats)
    {
        _agent = agent;
        _stats = stats;
    }

    public void MoveTo(Vector3 targetPosition)
    {
        _agent.SetDestination(targetPosition);
    }

    public Vector3 GetRandomPositionInRange(Vector3 center, float range)
    {
        Vector2 randomPosition = UnityEngine.Random.insideUnitCircle * range;
        Vector3 nextPosition = new Vector3(randomPosition.x, 0f, randomPosition.y);
        nextPosition += center;
        return nextPosition;
    }

    public void ExecuteJump(JumpData jumpData, Action onComplete)
    {
        if (_jumpCoroutine != null) return;
        _jumpCoroutine = StartCoroutine(JumpCoroutine(jumpData, onComplete));
    }

    public void ExecuteKnockback(Vector3 direction, float knockbackForce)
    {
        if (_knockbackCoroutine != null) return;
        _knockbackCoroutine = StartCoroutine(KnockbackCoroutine(direction, knockbackForce));
    }

    private IEnumerator JumpCoroutine(JumpData jumpData, Action onComplete)
    {

        Vector3 direction = (jumpData.EndPosition - jumpData.StartPosition);
        direction.y = 0.0f;
        direction.Normalize();
        transform.rotation = Quaternion.LookRotation(direction);

        float jumpHeight = jumpData.EndPosition.y - jumpData.StartPosition.y + JumpArcHeight;
        float jumpDuration = jumpHeight / _stats.JumpForce;
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float progress = elapsedTime / jumpDuration;

            Vector3 currentPosition = Vector3.Lerp(jumpData.StartPosition, jumpData.EndPosition, progress);
            currentPosition.y += jumpHeight * Mathf.Sin(progress * Mathf.PI);
            _agent.Warp(currentPosition);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _agent.Warp(jumpData.EndPosition);
        _agent.CompleteOffMeshLink();
        _agent.isStopped = false;

        onComplete?.Invoke();
        _jumpCoroutine = null;
    }

    private IEnumerator KnockbackCoroutine(Vector3 direction, float knockbackForce)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _stats.HitDuration)
        {
            elapsedTime += Time.deltaTime;
            _agent.Move(direction * (knockbackForce * _stats.KnockbackRate * Time.deltaTime));
            yield return null;
        }
        _knockbackCoroutine = null;
    }
}
