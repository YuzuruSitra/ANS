using UnityEngine;
using UnityEngine.AI;

public class FollowState : IPartnerAIState
{
    private Transform _player;
    NavMeshAgent _agent;
    private float _speed = 6.0f;
    private const float STATE_TIME = 4.0f;
    private float _remainTime;
    public bool IsStateFin => (_remainTime <= 0);

    public FollowState(GameObject player, NavMeshAgent agent)
    {
        _player = player.transform;
        _agent = agent;
    }

    // �X�e�[�g�ɓ��������̏���
    public void EnterState()
    {
        _remainTime = STATE_TIME;
        _agent.isStopped = false;
        _agent.speed = _speed;
    }

    // �X�e�[�g�̍X�V
    public void UpdateState()
    {
        _agent.destination = _player.position;
        _remainTime -= Time.deltaTime;
    }

    public void ExitState()
    {
        _agent.isStopped = true;
    }
}
