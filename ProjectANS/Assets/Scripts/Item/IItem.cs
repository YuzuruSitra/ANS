namespace Item
{
    public interface IItem
    {
        // �A�C�e���̖��O
        string Name { get; }

        ItemKind ItemKind { get; }

        // �A�C�e���̐���
        string Description { get; }

        // �A�C�e���̎g�p���\�b�h
        void UseEffect();

        // �A�C�e���̏���^���ǂ���
        bool IsConsumable { get; }
    }

    public enum ItemKind
    {
        Dynamite
    }
}