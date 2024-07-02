using UnityEngine;

public class StayState : IPartnerAIState
{
    private Transform _npcTransform;
    private const float ANGLE_RANGE = 120f;
    private const float STATE_TIME = 5.0f;
    private const float WAIT_TIME = 2.0f;
    private const float ROTATION_SPEED = 1.0f;

    private float _remainTime;
    private float _waitTime;
    private Vector3 _targetDirection;
    private bool _isRotating;

    public bool IsStateFin => (_remainTime <= 0);

    public StayState(GameObject npc)
    {
        _npcTransform = npc.transform;
    }

    // �X�e�[�g�ɓ��������̏���
    public void EnterState()
    {
        _remainTime = STATE_TIME;
        SetDirection();
        _isRotating = true;
        _waitTime = WAIT_TIME;
    }

    // �X�e�[�g�̍X�V
    public void UpdateState()
    {
        _remainTime -= Time.deltaTime;

        if (_isRotating)
        {
            RotateTowardsTarget();
        }
        else
        {
            _waitTime -= Time.deltaTime;
            if (_waitTime <= 0)
            {
                SetDirection();
                _isRotating = true;
                _waitTime = WAIT_TIME;
            }
        }
    }

    public void ExitState()
    {

    }

    private void SetDirection()
    {
        // �O���̕�������ɂ���
        Vector3 forward = _npcTransform.transform.forward;

        // �����_���Ȋp�x������
        float randomAngle = Random.Range(-ANGLE_RANGE, ANGLE_RANGE);
        _targetDirection = Quaternion.Euler(0, randomAngle, 0) * forward;
    }

    private void RotateTowardsTarget()
    {
        // ���݂̕���
        Vector3 currentDirection = _npcTransform.forward;
        Quaternion currentRotation = _npcTransform.rotation;

        // �ڕW�̉�]
        Quaternion targetRotation = Quaternion.LookRotation(_targetDirection);

        // Slerp�ŉ�]����
        _npcTransform.rotation = Quaternion.Slerp(currentRotation, targetRotation, ROTATION_SPEED * Time.deltaTime);

        // ��]���ڕW�ɂقڈ�v���������m�F
        if (Quaternion.Angle(currentRotation, targetRotation) < 1.0f)
        {
            _isRotating = false;
        }
    }
}
