namespace Item
{
    public interface IItem
    {
        // �A�C�e���̖��O
        string Name { get; }

        // �A�C�e����ID
        int ItemID { get; }
        ItemKind ItemKind { get; }

        // �A�C�e���̐���
        string Description { get; }

        // �A�C�e���̉��l
        int Value { get; }

        // �A�C�e���̎g�p���\�b�h
        void UseEffect();

        // �A�C�e�����C���x���g���ɒǉ�����Ƃ��ɌĂ΂�郁�\�b�h
        //void AddToInventory();

        // �A�C�e�����폜����Ƃ��ɌĂ΂�郁�\�b�h
        //void RemoveFromInventory();

        // �A�C�e���̏���^���ǂ���
        bool IsConsumable { get; }
    }

    public enum ItemKind
    {
        Dynamite
    }
}