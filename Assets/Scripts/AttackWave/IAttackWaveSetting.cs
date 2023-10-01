namespace GameCore
{
    public interface IAttackWaveSetting
    {
        float MonsterHp { get; }
        AttackWave[] GetAttackWaves();
    }
}