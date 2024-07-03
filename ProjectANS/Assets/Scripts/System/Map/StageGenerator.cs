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
        roomStatus = new int[roomNum, System.Enum.GetNames(typeof(RoomStatus)).Length];

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
        roomStatus[roomCount, (int)RoomStatus.x] = 0;
        roomStatus[roomCount, (int)RoomStatus.y] = 0;
        roomStatus[roomCount, (int)RoomStatus.w] = _mapSize;
        roomStatus[roomCount, (int)RoomStatus.h] = _mapSize;

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
                if (max < roomStatus[maxCheck, (int)RoomStatus.w] * roomStatus[maxCheck, (int)RoomStatus.h])
                {
                    // �ő�ʐϏ㏑��
                    max = roomStatus[maxCheck, (int)RoomStatus.w] * roomStatus[maxCheck, (int)RoomStatus.h];

                    // �e�̕����ԍ��Z�b�g
                    parentNum = maxCheck;
                }
            }

            // �擾��������������Ɋ���
            if (SplitPoint(roomStatus[parentNum, (int)RoomStatus.w], roomStatus[parentNum, (int)RoomStatus.h]))
            {
                // �擾
                roomStatus[roomCount, (int)RoomStatus.x] = roomStatus[parentNum, (int)RoomStatus.x];
                roomStatus[roomCount, (int)RoomStatus.y] = roomStatus[parentNum, (int)RoomStatus.y];
                roomStatus[roomCount, (int)RoomStatus.w] = roomStatus[parentNum, (int)RoomStatus.w] - line;
                roomStatus[roomCount, (int)RoomStatus.h] = roomStatus[parentNum, (int)RoomStatus.h];

                // �e�̕����𐮌`����
                roomStatus[parentNum, (int)RoomStatus.x] += roomStatus[roomCount, (int)RoomStatus.w];
                roomStatus[parentNum, (int)RoomStatus.w] -= roomStatus[roomCount, (int)RoomStatus.w];
            }
            else
            {
                // �擾
                roomStatus[roomCount, (int)RoomStatus.x] = roomStatus[parentNum, (int)RoomStatus.x];
                roomStatus[roomCount, (int)RoomStatus.y] = roomStatus[parentNum, (int)RoomStatus.y];
                roomStatus[roomCount, (int)RoomStatus.w] = roomStatus[parentNum, (int)RoomStatus.w];
                roomStatus[roomCount, (int)RoomStatus.h] = roomStatus[parentNum, (int)RoomStatus.h] - line;

                // �e�̕����𐮌`����
                roomStatus[parentNum, (int)RoomStatus.y] += roomStatus[roomCount, (int)RoomStatus.h];
                roomStatus[parentNum, (int)RoomStatus.h] -= roomStatus[roomCount, (int)RoomStatus.h];
            }
            // �J�E���g�����Z
            roomCount++;
        }

        // �����������Ƀ����_���ȑ傫���̕����𐶐�
        for (int i = 0; i < roomNum; i++)
        {
            // �������W�̐ݒ�
            roomStatus[i, (int)RoomStatus.rx] = Random.Range(roomStatus[i, (int)RoomStatus.x] + offsetWall, (roomStatus[i, (int)RoomStatus.x] + roomStatus[i, (int)RoomStatus.w]) - (roomMin + offsetWall));
            roomStatus[i, (int)RoomStatus.ry] = Random.Range(roomStatus[i, (int)RoomStatus.y] + offsetWall, (roomStatus[i, (int)RoomStatus.y] + roomStatus[i, (int)RoomStatus.h]) - (roomMin + offsetWall));

            // �����̑傫����ݒ�
            roomStatus[i, (int)RoomStatus.rw] = Random.Range(roomMin, roomStatus[i, (int)RoomStatus.w] - (roomStatus[i, (int)RoomStatus.rx] - roomStatus[i, (int)RoomStatus.x]) - offset);
            roomStatus[i, (int)RoomStatus.rh] = Random.Range(roomMin, roomStatus[i, (int)RoomStatus.h] - (roomStatus[i, (int)RoomStatus.ry] - roomStatus[i, (int)RoomStatus.y]) - offset);
        }

        // �}�b�v�㏑��
        for (int count = 0; count < roomNum; count++)
        {
            // �擾���������̊m�F
            for (int h = 0; h < roomStatus[count, (int)RoomStatus.h]; h++)
            {
                for (int w = 0; w < roomStatus[count, (int)RoomStatus.w]; w++)
                {
                    // �����`�F�b�N�|�C���g
                    _mapKind[w + roomStatus[count, (int)RoomStatus.x], h + roomStatus[count, (int)RoomStatus.y]] = (int)objectType.wall;
                }

            }

            // ������������
            for (int h = 0; h < roomStatus[count, (int)RoomStatus.rh]; h++)
            {
                for (int w = 0; w < roomStatus[count, (int)RoomStatus.rw]; w++)
                {
                    _mapKind[w + roomStatus[count, (int)RoomStatus.rx], h + roomStatus[count, (int)RoomStatus.ry]] = (int)objectType.ground;
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
            splitLength[0] = roomStatus[nowRoom, (int)RoomStatus.x] > 0 ?
                roomStatus[nowRoom, (int)RoomStatus.rx] - roomStatus[nowRoom, (int)RoomStatus.x] : int.MaxValue;
            // �E�̕ǂ���̋���
            splitLength[1] = (roomStatus[nowRoom, (int)RoomStatus.x] + roomStatus[nowRoom, (int)RoomStatus.w]) < _mapSize ?
                (roomStatus[nowRoom, (int)RoomStatus.x] + roomStatus[nowRoom, (int)RoomStatus.w]) - (roomStatus[nowRoom, (int)RoomStatus.rx] + roomStatus[nowRoom, (int)RoomStatus.rw]) : int.MaxValue;

            // ���̕ǂ���̋���
            splitLength[2] = roomStatus[nowRoom, (int)RoomStatus.y] > 0 ?
                roomStatus[nowRoom, (int)RoomStatus.ry] - roomStatus[nowRoom, (int)RoomStatus.y] : int.MaxValue;
            // ��̕ǂ���̋���
            splitLength[3] = (roomStatus[nowRoom, (int)RoomStatus.y] + roomStatus[nowRoom, (int)RoomStatus.h]) < _mapSize ?
                (roomStatus[nowRoom, (int)RoomStatus.y] + roomStatus[nowRoom, (int)RoomStatus.h]) - (roomStatus[nowRoom, (int)RoomStatus.ry] + roomStatus[nowRoom, (int)RoomStatus.rh]) : int.MaxValue;

            // �}�b�N�X�łȂ����̂ݐ��
            for (int j = 0; j < splitLength.Length; j++)
            {
                if (splitLength[j] != int.MaxValue)
                {
                    // �㉺���E����
                    if (j < 2)
                    {
                        // ���������ꏊ������
                        roodPoint = Random.Range(roomStatus[nowRoom, (int)RoomStatus.ry] + offset, roomStatus[nowRoom, (int)RoomStatus.ry] + roomStatus[nowRoom, (int)RoomStatus.rh] - offset);

                        // �}�b�v�ɏ�������
                        for (int w = 1; w <= splitLength[j]; w++)
                        {
                            // ���E����
                            if (j == 0)
                            {
                                // ��
                                _mapKind[(-w) + roomStatus[nowRoom, (int)RoomStatus.rx], roodPoint] = (int)objectType.road;
                            }
                            else
                            {
                                // �E
                                _mapKind[w + roomStatus[nowRoom, (int)RoomStatus.rx] + roomStatus[nowRoom, (int)RoomStatus.rw] - offset, roodPoint] = (int)objectType.road;

                                // �Ō�
                                if (w == splitLength[j])
                                {
                                    // ��������
                                    _mapKind[w + offset + roomStatus[nowRoom, (int)RoomStatus.rx] + roomStatus[nowRoom, (int)RoomStatus.rw] - offset, roodPoint] = (int)objectType.road;
                                }
                            }
                        }
                    }
                    else
                    {
                        // ���������ꏊ������
                        roodPoint = Random.Range(roomStatus[nowRoom, (int)RoomStatus.rx] + offset, roomStatus[nowRoom, (int)RoomStatus.rx] + roomStatus[nowRoom, (int)RoomStatus.rw] - offset);

                        // �}�b�v�ɏ�������
                        for (int h = 1; h <= splitLength[j]; h++)
                        {
                            // �㉺����
                            if (j == 2)
                            {
                                // ��
                                _mapKind[roodPoint, (-h) + roomStatus[nowRoom, (int)RoomStatus.ry]] = (int)objectType.road;
                            }
                            else
                            {
                                // ��
                                _mapKind[roodPoint, h + roomStatus[nowRoom, (int)RoomStatus.ry] + roomStatus[nowRoom, (int)RoomStatus.rh] - offset] = (int)objectType.road;

                                // �Ō�
                                if (h == splitLength[j])
                                {
                                    // ��������
                                    _mapKind[roodPoint, h + offset + roomStatus[nowRoom, (int)RoomStatus.ry] + roomStatus[nowRoom, (int)RoomStatus.rh] - offset] = (int)objectType.road;
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
            for (int roodScan = 0; roodScan < roomStatus[nowRoom, (int)RoomStatus.w]; roodScan++)
            {
                // ��������
                if (_mapKind[roodScan + roomStatus[nowRoom, (int)RoomStatus.x], roomStatus[nowRoom, (int)RoomStatus.y]] == (int)objectType.road)
                {
                    // ���̍��W�Z�b�g
                    if (roadVec1 == 0)
                    {
                        // �n�_�Z�b�g
                        roadVec1 = roodScan + roomStatus[nowRoom, (int)RoomStatus.x];
                    }
                    else
                    {
                        // �I�_�Z�b�g
                        roadVec2 = roodScan + roomStatus[nowRoom, (int)RoomStatus.x];
                    }
                }
            }
            // ��������
            for (int roadSet = roadVec1; roadSet < roadVec2; roadSet++)
            {
                // ���E�����㏑��
                _mapKind[roadSet, roomStatus[nowRoom, (int)RoomStatus.y]] = (int)objectType.road;
            }

            roadVec1 = 0;
            roadVec2 = 0;

            for (int roadScan = 0; roadScan < roomStatus[nowRoom, (int)RoomStatus.h]; roadScan++)
            {
                // ��������
                if (_mapKind[roomStatus[nowRoom, (int)RoomStatus.x], roadScan + roomStatus[nowRoom, (int)RoomStatus.y]] == (int)objectType.road)
                {
                    // ���̍��W�Z�b�g
                    if (roadVec1 == 0)
                    {
                        // �n�_�Z�b�g
                        roadVec1 = roadScan + roomStatus[nowRoom, (int)RoomStatus.y];
                    }
                    else
                    {
                        // �I�_�Z�b�g
                        roadVec2 = roadScan + roomStatus[nowRoom, (int)RoomStatus.y];
                    }
                }
            }
            // ��������
            for (int roadSet = roadVec1; roadSet < roadVec2; roadSet++)
            {
                // ���E�����㏑��
                _mapKind[roomStatus[nowRoom, (int)RoomStatus.x], roadSet] = (int)objectType.road;
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