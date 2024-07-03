using UnityEngine;
using Random = UnityEngine.Random;

public class StageGenerator : MonoBehaviour
{
    [SerializeField] Vector3 defaultPosition;
    // Size of one side of the map.
    [SerializeField] private int _mapSize;
    // Block kind.
    private int[,] _mapKind;

    // Number of rooms.
    [SerializeField] private int roomNum;
    // Minimum room size.
    [SerializeField] private int roomMin = 4;
    private int roomCount;                      // �����J�E���g
    private int line = 0;                       // �����_
    private int[,] roomStatus;                  // �����̊Ǘ��z��

    private enum RoomStatus                     // �����̔z��X�e�[�^�X
    {
        x,// �}�b�v���W��
        y,// �}�b�v���W��
        w,// ����������
        h,// ������������

        rx,// �����̐����ʒu
        ry,// �����̐����ʒu
        rw,// �����̕�
        rh,// �����̍���
    }

    enum objectType
    {
        ground = 0,
        wall = 1,
        road = 2,
    }
    
    [SerializeField] private GameObject[] mapObjects;               // �}�b�v�����p�̃I�u�W�F�N�g�z��

    private const int offsetWall = 2;   // �ǂ��痣������
    private const int offset = 1;       // �����p

    void Start()
    {
        MapGenerate();
    }

    private void MapGenerate()
    {
        // �����iStartX�AStartY�A���A�����j
        roomStatus = new int[System.Enum.GetNames(typeof(RoomStatus)).Length, roomNum];

        // �t���A�ݒ�
        _mapKind = new int[_mapSize, _mapSize];


        // �t���A�̏�����
        for (int nowW = 0; nowW < _mapSize; nowW++)
        {
            for (int nowH = 0; nowH < _mapSize; nowH++)
            {
                // �ǂ�\��
                _mapKind[nowW, nowH] = 2;
            }
        }

        // �t���A������
        roomStatus[(int)RoomStatus.x, roomCount] = 0;
        roomStatus[(int)RoomStatus.y, roomCount] = 0;
        roomStatus[(int)RoomStatus.w, roomCount] = _mapSize;
        roomStatus[(int)RoomStatus.h, roomCount] = _mapSize;

        // �J�E���g�ǉ�
        roomCount++;

        // �����̐�������������
        int parentNum = 0;
        int max = 0; 
        for (int splitNum = 0; splitNum < roomNum - 1; splitNum++)
        {
            // �ϐ�������
            parentNum = 0;  // �������镔���ԍ�
            max = 0;        // �ő�ʐ�

            // �ő�̕����ԍ��𒲂ׂ�
            for (int maxCheck = 0; maxCheck < roomNum; maxCheck++)
            {
                // �ʐϔ�r
                if (max < roomStatus[(int)RoomStatus.w, maxCheck] * roomStatus[(int)RoomStatus.h, maxCheck])
                {
                    // �ő�ʐϏ㏑��
                    max = roomStatus[(int)RoomStatus.w, maxCheck] * roomStatus[(int)RoomStatus.h, maxCheck];

                    // �e�̕����ԍ��Z�b�g
                    parentNum = maxCheck;
                }
            }

            // �擾��������������Ɋ���
            if (SplitPoint(roomStatus[(int)RoomStatus.w, parentNum], roomStatus[(int)RoomStatus.h, parentNum]))
            {
                // �擾
                roomStatus[(int)RoomStatus.x, roomCount] = roomStatus[(int)RoomStatus.x, parentNum];
                roomStatus[(int)RoomStatus.y, roomCount] = roomStatus[(int)RoomStatus.y, parentNum];
                roomStatus[(int)RoomStatus.w, roomCount] = roomStatus[(int)RoomStatus.w, parentNum] - line;
                roomStatus[(int)RoomStatus.h, roomCount] = roomStatus[(int)RoomStatus.h, parentNum];

                // �e�̕����𐮌`����
                roomStatus[(int)RoomStatus.x, parentNum] += roomStatus[(int)RoomStatus.w, roomCount];
                roomStatus[(int)RoomStatus.w, parentNum] -= roomStatus[(int)RoomStatus.w, roomCount];
            }
            else
            {
                // �擾
                roomStatus[(int)RoomStatus.x, roomCount] = roomStatus[(int)RoomStatus.x, parentNum];
                roomStatus[(int)RoomStatus.y, roomCount] = roomStatus[(int)RoomStatus.y, parentNum];
                roomStatus[(int)RoomStatus.w, roomCount] = roomStatus[(int)RoomStatus.w, parentNum];
                roomStatus[(int)RoomStatus.h, roomCount] = roomStatus[(int)RoomStatus.h, parentNum] - line;

                // �e�̕����𐮌`����
                roomStatus[(int)RoomStatus.y, parentNum] += roomStatus[(int)RoomStatus.h, roomCount];
                roomStatus[(int)RoomStatus.h, parentNum] -= roomStatus[(int)RoomStatus.h, roomCount];
            }
            // �J�E���g�����Z
            roomCount++;
        }

        // �����������Ƀ����_���ȑ傫���̕����𐶐�
        for (int i = 0; i < roomNum; i++)
        {
            // �������W�̐ݒ�
            roomStatus[(int)RoomStatus.rx, i] = Random.Range(roomStatus[(int)RoomStatus.x, i] + offsetWall, (roomStatus[(int)RoomStatus.x, i] + roomStatus[(int)RoomStatus.w, i]) - (roomMin + offsetWall));
            roomStatus[(int)RoomStatus.ry, i] = Random.Range(roomStatus[(int)RoomStatus.y, i] + offsetWall, (roomStatus[(int)RoomStatus.y, i] + roomStatus[(int)RoomStatus.h, i]) - (roomMin + offsetWall));

            // �����̑傫����ݒ�
            roomStatus[(int)RoomStatus.rw, i] = Random.Range(roomMin, roomStatus[(int)RoomStatus.w, i] - (roomStatus[(int)RoomStatus.rx, i] - roomStatus[(int)RoomStatus.x, i]) - offset);
            roomStatus[(int)RoomStatus.rh, i] = Random.Range(roomMin, roomStatus[(int)RoomStatus.h, i] - (roomStatus[(int)RoomStatus.ry, i] - roomStatus[(int)RoomStatus.y, i]) - offset);
        }

        // �}�b�v�㏑��
        for (int count = 0; count < roomNum; count++)
        {
            // �擾���������̊m�F
            for (int h = 0; h < roomStatus[(int)RoomStatus.h, count]; h++)
            {
                for (int w = 0; w < roomStatus[(int)RoomStatus.w, count]; w++)
                {
                    // �����`�F�b�N�|�C���g
                    _mapKind[w + roomStatus[(int)RoomStatus.x, count], h + roomStatus[(int)RoomStatus.y, count]] = (int)objectType.wall;
                }

            }

            // ������������
            for (int h = 0; h < roomStatus[(int)RoomStatus.rh, count]; h++)
            {
                for (int w = 0; w < roomStatus[(int)RoomStatus.rw, count]; w++)
                {
                    _mapKind[w + roomStatus[(int)RoomStatus.rx, count], h + roomStatus[(int)RoomStatus.ry, count]] = (int)objectType.ground;
                }
            }
        }

        // ���̐���
        int[] splitLength = new int[4];
        int roodPoint = 0;// ���������ꏊ

        // ���������ԋ߂����E���𒲂ׂ�(�\���ɒ��ׂ�)
        for (int nowRoom = 0; nowRoom < roomNum; nowRoom++)
        {
            // ���̕ǂ���̋���
            splitLength[0] = roomStatus[(int)RoomStatus.x, nowRoom] > 0 ?
                roomStatus[(int)RoomStatus.rx, nowRoom] - roomStatus[(int)RoomStatus.x, nowRoom] : int.MaxValue;
            // �E�̕ǂ���̋���
            splitLength[1] = (roomStatus[(int)RoomStatus.x, nowRoom] + roomStatus[(int)RoomStatus.w, nowRoom]) < _mapSize ?
                (roomStatus[(int)RoomStatus.x, nowRoom] + roomStatus[(int)RoomStatus.w, nowRoom]) - (roomStatus[(int)RoomStatus.rx, nowRoom] + roomStatus[(int)RoomStatus.rw, nowRoom]) : int.MaxValue;

            // ���̕ǂ���̋���
            splitLength[2] = roomStatus[(int)RoomStatus.y, nowRoom] > 0 ?
                roomStatus[(int)RoomStatus.ry, nowRoom] - roomStatus[(int)RoomStatus.y, nowRoom] : int.MaxValue;
            // ��̕ǂ���̋���
            splitLength[3] = (roomStatus[(int)RoomStatus.y, nowRoom] + roomStatus[(int)RoomStatus.h, nowRoom]) < _mapSize ?
                (roomStatus[(int)RoomStatus.y, nowRoom] + roomStatus[(int)RoomStatus.h, nowRoom]) - (roomStatus[(int)RoomStatus.ry, nowRoom] + roomStatus[(int)RoomStatus.rh, nowRoom]) : int.MaxValue;

            // �}�b�N�X�łȂ����̂ݐ��
            for (int j = 0; j < splitLength.Length; j++)
            {
                if (splitLength[j] != int.MaxValue)
                {
                    // �㉺���E����
                    if (j < 2)
                    {
                        // ���������ꏊ������
                        roodPoint = Random.Range(roomStatus[(int)RoomStatus.ry, nowRoom] + offset, roomStatus[(int)RoomStatus.ry, nowRoom] + roomStatus[(int)RoomStatus.rh, nowRoom] - offset);

                        // �}�b�v�ɏ�������
                        for (int w = 1; w <= splitLength[j]; w++)
                        {
                            // ���E����
                            if (j == 0)
                            {
                                // ��
                                _mapKind[(-w) + roomStatus[(int)RoomStatus.rx, nowRoom], roodPoint] = (int)objectType.road;
                            }
                            else
                            {
                                // �E
                                _mapKind[w + roomStatus[(int)RoomStatus.rx, nowRoom] + roomStatus[(int)RoomStatus.rw, nowRoom] - offset, roodPoint] = (int)objectType.road;

                                // �Ō�
                                if (w == splitLength[j])
                                {
                                    // ��������
                                    _mapKind[w + offset + roomStatus[(int)RoomStatus.rx, nowRoom] + roomStatus[(int)RoomStatus.rw, nowRoom] - offset, roodPoint] = (int)objectType.road;
                                }
                            }
                        }
                    }
                    else
                    {
                        // ���������ꏊ������
                        roodPoint = Random.Range(roomStatus[(int)RoomStatus.rx, nowRoom] + offset, roomStatus[(int)RoomStatus.rx, nowRoom] + roomStatus[(int)RoomStatus.rw, nowRoom] - offset);

                        // �}�b�v�ɏ�������
                        for (int h = 1; h <= splitLength[j]; h++)
                        {
                            // �㉺����
                            if (j == 2)
                            {
                                // ��
                                _mapKind[roodPoint, (-h) + roomStatus[(int)RoomStatus.ry, nowRoom]] = (int)objectType.road;
                            }
                            else
                            {
                                // ��
                                _mapKind[roodPoint, h + roomStatus[(int)RoomStatus.ry, nowRoom] + roomStatus[(int)RoomStatus.rh, nowRoom] - offset] = (int)objectType.road;

                                // �Ō�
                                if (h == splitLength[j])
                                {
                                    // ��������
                                    _mapKind[roodPoint, h + offset + roomStatus[(int)RoomStatus.ry, nowRoom] + roomStatus[(int)RoomStatus.rh, nowRoom] - offset] = (int)objectType.road;
                                }
                            }
                        }
                    }
                }
            }
        }

        int roadVec1 = 0;// ���̎n�_
        int roadVec2 = 0;// ���̏I�_

        // ���̐ڑ�
        for (int nowRoom = 0; nowRoom < roomNum; nowRoom++)
        {
            roadVec1 = 0;
            roadVec2 = 0;
            // �����q����
            for (int roodScan = 0; roodScan < roomStatus[(int)RoomStatus.w, nowRoom]; roodScan++)
            {
                // ��������
                if (_mapKind[roodScan + roomStatus[(int)RoomStatus.x, nowRoom], roomStatus[(int)RoomStatus.y, nowRoom]] == (int)objectType.road)
                {
                    // ���̍��W�Z�b�g
                    if (roadVec1 == 0)
                    {
                        // �n�_�Z�b�g
                        roadVec1 = roodScan + roomStatus[(int)RoomStatus.x, nowRoom];
                    }
                    else
                    {
                        // �I�_�Z�b�g
                        roadVec2 = roodScan + roomStatus[(int)RoomStatus.x, nowRoom];
                    }
                }
            }
            // ��������
            for (int roadSet = roadVec1; roadSet < roadVec2; roadSet++)
            {
                // ���E�����㏑��
                _mapKind[roadSet, roomStatus[(int)RoomStatus.y, nowRoom]] = (int)objectType.road;
            }

            roadVec1 = 0;
            roadVec2 = 0;

            for (int roadScan = 0; roadScan < roomStatus[(int)RoomStatus.h, nowRoom]; roadScan++)
            {
                // ��������
                if (_mapKind[roomStatus[(int)RoomStatus.x, nowRoom], roadScan + roomStatus[(int)RoomStatus.y, nowRoom]] == (int)objectType.road)
                {
                    // ���̍��W�Z�b�g
                    if (roadVec1 == 0)
                    {
                        // �n�_�Z�b�g
                        roadVec1 = roadScan + roomStatus[(int)RoomStatus.y, nowRoom];
                    }
                    else
                    {
                        // �I�_�Z�b�g
                        roadVec2 = roadScan + roomStatus[(int)RoomStatus.y, nowRoom];
                    }
                }
            }
            // ��������
            for (int roadSet = roadVec1; roadSet < roadVec2; roadSet++)
            {
                // ���E�����㏑��
                _mapKind[roomStatus[(int)RoomStatus.x, nowRoom], roadSet] = (int)objectType.road;
            }
        }

        // �e�I�u�W�F�N�g�̐���
        var groundParent = new GameObject("Ground");
        var wallParent = new GameObject("Wall");
        var roadParent = new GameObject("Road");

        // �z��Ƀv���n�u������
        var objectParents = new GameObject[] { groundParent, wallParent, roadParent };

        // �I�u�W�F�N�g�𐶐�����
        for (int nowH = 0; nowH < _mapSize; nowH++)
        {
            for (int nowW = 0; nowW < _mapSize; nowW++)
            {
                // �ǂ̐���
                if (_mapKind[nowW, nowH] == (int)objectType.wall)
                {
                    GameObject mazeObject = Instantiate(
                        mapObjects[_mapKind[nowW, nowH]],
                        new Vector3(
                            defaultPosition.x + nowW * mapObjects[_mapKind[nowW, nowH]].transform.localScale.x,
                            defaultPosition.y + (mapObjects[(int)objectType.wall].transform.localScale.y - mapObjects[(int)objectType.ground].transform.localScale.y) * 0.5f,
                            defaultPosition.z + nowH * mapObjects[_mapKind[nowW, nowH]].transform.localScale.z),
                        Quaternion.identity,objectParents[_mapKind[nowW, nowH]].transform);
                }

                // �����̐���
                if (_mapKind[nowW, nowH] == (int)objectType.ground)
                {
                    GameObject mazeObject = Instantiate(
                        mapObjects[_mapKind[nowW, nowH]],
                        new Vector3(
                            defaultPosition.x + nowW * mapObjects[_mapKind[nowW, nowH]].transform.localScale.x,
                            defaultPosition.y,
                            defaultPosition.z + nowH * mapObjects[_mapKind[nowW, nowH]].transform.localScale.z),
                        Quaternion.identity, objectParents[_mapKind[nowW, nowH]].transform);
                }

                // �ʘH�̐���
                if (_mapKind[nowW, nowH] == (int)objectType.road)
                {
                    GameObject mazeObject = Instantiate(
                        mapObjects[_mapKind[nowW, nowH]],
                        new Vector3(
                            defaultPosition.x + nowW * mapObjects[_mapKind[nowW, nowH]].transform.localScale.x,
                            defaultPosition.y,
                            defaultPosition.z + nowH * mapObjects[_mapKind[nowW, nowH]].transform.localScale.z),
                        Quaternion.identity,objectParents[_mapKind[nowW, nowH]].transform);
                }

            }
        }
    }

    // �����_�̃Z�b�g(int x, int y)�A�傫�����𕪊�����
    private bool SplitPoint(int x, int y)
    {
        // �����ʒu�̌���
        if (x > y)
        {
            line = Random.Range(roomMin + (offsetWall * 2), x - (offsetWall * 2 + roomMin));// �c����
            return true;
        }
        else
        {
            line = Random.Range(roomMin + (offsetWall * 2), y - (offsetWall * 2 + roomMin));// ������
            return false;
        }
    }

}