public abstract class Skill : Entity
{
    private int m_currentLevel = 0;

    public int CurrentLevel { get => m_currentLevel; set => m_currentLevel = value; }

    public abstract int MaxLevel();

    public int LevelsTillMax()
    {
        return MaxLevel() - m_currentLevel;
    }

}
