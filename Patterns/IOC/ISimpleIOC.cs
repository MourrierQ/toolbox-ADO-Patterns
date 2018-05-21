namespace Patterns.IOC
{
    public interface ISimpleIOC
    {
        TResource GetInstance<TResource>();
        void Register<TInterface, TResource>() where TResource : TInterface;
        void Register<TInterface, TResource>(params object[] Parameters) where TResource : TInterface;
        void Register<TResource>();
        void Register<TResource>(params object[] Parameters);
    }
}