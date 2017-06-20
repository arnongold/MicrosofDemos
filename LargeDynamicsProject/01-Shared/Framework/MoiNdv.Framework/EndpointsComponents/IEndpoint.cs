namespace MoiNdv.Framework.EndpointsComponents
{
    interface IEndpoint
    {
        T CreateBLInstance<T>() where T : class;
    }
}
