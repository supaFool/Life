public abstract class CookableFood : Food
{
    public abstract float PrimeCookingTemp();
    public abstract bool IsCooked();
    public override bool Cookable()
    {
        return true;
    }
}
