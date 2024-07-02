using UnityEngine;
using UnityEngine.AI;

public class FollowState : IPartnerAIState
{
    private Transform _player;
    NavMeshAgent _agent;

    public FollowState(GameObject player, NavMeshAgent agent)
    {
        _player = player.transform;
        _agent = agent;
    }

    // ���u��
    public bool IsStateFin => false;

    // �X�e�[�g�ɓ��������̏���
    public void EnterState()
    {
        
    }

    // �X�e�[�g�̍X�V
    public void UpdateState()
    {
        _agent.destination = _player.position;
    }

    public void ExitState()
    {

    }
}
