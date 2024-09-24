using UnityEngine;

namespace Item
{
    public class Dynamite : MonoBehaviour, IItem
    {
        [SerializeField]
        private string _name;
        public string Name => _name;
        [SerializeField]
        private ItemKind _itemKind;
        public ItemKind ItemKind => _itemKind;
        [SerializeField]
        private string _description;
        public string Description => _description;
        [SerializeField]
        private bool _isConsumable;
        public bool IsConsumable => _isConsumable;
        [SerializeField]
        private float _detonationTime;
        public float _currentTime;
        private bool _isUsed;
        [SerializeField]
        private float _rayLength = 1.5f; // ���C�̒������w��
        private Ray[] _rays = new Ray[8];

        private void Start()
        {
            var directions = new Vector3[8];
            directions[0] = Vector3.forward;     // +Z ����
            directions[1] = Vector3.back;        // -Z ����
            directions[2] = Vector3.right;       // +X ����
            directions[3] = Vector3.left;        // -X ����
            directions[4] = (Vector3.forward + Vector3.right).normalized;  // +Z +X ����
            directions[5] = (Vector3.forward + Vector3.left).normalized;   // +Z -X ����
            directions[6] = (Vector3.back + Vector3.right).normalized;     // -Z +X ����
            directions[7] = (Vector3.back + Vector3.left).normalized;      // -Z -X ����

            // �e�����Ɍ�����Ray��������
            for (int i = 0; i < directions.Length; i++)
            {
                _rays[i] = new Ray(transform.position, directions[i]);
            }
        }

        private void Update()
        {
            if (_isUsed) return;
            _currentTime += Time.deltaTime;
            if (_currentTime <= _detonationTime) return;
            UseEffect();
            _isUsed = true;
        }

        public void UseEffect()
        {
            for (int i = 0; i < _rays.Length; i++)
            {
                _rays[i].origin = transform.position;

                if (Physics.Raycast(_rays[i], out var hit, _rayLength))
                {
                    if (hit.collider.CompareTag("StageCube"))
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }
            }
        }
    }
}