using System.Collections.Generic;
using UnityEngine;

public class PartnerNPCController : MonoBehaviour
{
    [Header("�ړ����x")]
    [SerializeField] 
    private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;
    // [Header("�ڕW���W�ɑ΂��鋖�e�덷")]
    // [SerializeField] 
    // private float _stoppingDistance = 0.1f;

    private PartnerAIState _currentState;
    private Dictionary<PartnerAIState, IPartnerAIState> _states = new Dictionary<PartnerAIState, IPartnerAIState>();

    //private Animator _animator;
    // �^�[�Q�b�g���W��ێ�

    // �ړ��p�N���X
    // private InnNPCMover _innNPCMover;
    // public InnNPCMover InnNPCMover => _innNPCMover;

    void Start()
    {
        _currentState = PartnerAIState.STAY;
    }

    void Update()
    {
        _states[_currentState].UpdateState();
        // ChangeAnimWalk(_states[_currentState].IsWalk);
        if (_states[_currentState].IsStateFin) NextState(_currentState);
    }

    void NextState(PartnerAIState state)
    {
        // �I�菈��������
        PartnerAIState newState = state;
        switch (state)
        {
            case PartnerAIState.STAY:
                break;
            case PartnerAIState.FOLLOW:
                break;
            case PartnerAIState.FREE_WALK:
                break;
            default:
                newState = state;
                break;
        }
        _states[_currentState].ExitState();
        _states[newState].EnterState();
        _currentState = newState;
    }
}
