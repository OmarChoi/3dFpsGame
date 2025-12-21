public interface IZombieDataState<TData> where TData : struct
{
    void EnterWithData(in TData data);
}
