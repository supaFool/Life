public abstract class Food : Consumable
{
    public abstract bool Cookable();

    public override bool CanDrink()
    {
        return false;
    }
}
