namespace TexasHoldem.Logic
{
    public interface IDeepCloneable<out T>
    {
        T DeepClone();
    }
}
