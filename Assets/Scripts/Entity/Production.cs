public abstract class ProductionItem : Item
{
    public abstract string[] SkillAffected();
    public abstract string[] ToolsRequired();
    public abstract string[] UnitRequired();
}
