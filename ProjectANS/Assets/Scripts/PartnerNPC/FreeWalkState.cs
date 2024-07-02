using UnityEngine;
using UnityEngine.AI;

public class FreeWalkState : IPartnerAIState
{
    private Transform _npcTransform;
    private NavMeshAgent _agent;
    private float _speed = 3.0f;
    private float _currentWait;
    private const float WAIT_TIME = 2.0f;
    private const float RANGE = 4.0f;
    private const float ANGLE_RANGE = 60f;
    private const float DESTINATION_THRESHOLD = 1.25f;
    private const float STATE_TIME = 10.0f;
    private float _remainTime;

    public FreeWalkState(GameObject npc, NavMeshAgent agent)
    {
        _npcTransform = npc.transform;
        _agent = agent;
    }

    // ���u��
    public bool IsStateFin => (_remainTime <= 0);

    // �X�e�[�g�ɓ��������̏���
    public void EnterState()
    {
        _remainTime = STATE_TIME;
        _agent.isStopped = false;
        SetRandomDestination();
        _agent.speed = _speed;
        _currentWait = WAIT_TIME;
    }

    // �X�e�[�g�̍X�V
    public void UpdateState()
    {
        _remainTime -= Time.deltaTime;
        if (!_agent.pathPending && _agent.remainingDistance <= DESTINATION_THRESHOLD)
            SetRandomDestination();
    }

    public void ExitState()
    {
        _agent.isStopped = true;
    }

    // Set a target point.
    void SetRandomDestination()
    {
        _currentWait -= Time.deltaTime;
        if (_currentWait >= 0) return;
        // �O���̕�������ɂ���
        Vector3 forward = _npcTransform.transform.forward;

        // �����_���Ȋp�x������
        float randomAngle = Random.Range(-ANGLE_RANGE, ANGLE_RANGE);
        Vector3 direction = Quaternion.Euler(0, randomAngle, 0) * forward;

        // �����_���ȋ���������
        float randomDistance = Random.Range(0, RANGE);
        Vector3 destination = _npcTransform.position + direction * randomDistance;

        // NavMesh��̗L���ȃ|�C���g��������
        if (NavMesh.SamplePosition(destination, out NavMeshHit hit, RANGE, NavMesh.AllAreas))
            _agent.SetDestination(hit.position);
        _currentWait = WAIT_TIME;
    }

}
