using ReLoop.Shared.Abstractions.Kernel.Database;

namespace ReLoop.Shared.Infrastructure.Postgres;

internal class UnitOfWorkTypeRegistry
{
    public static UnitOfWorkTypeRegistry Instance { get; } = new();

    private readonly Dictionary<string, Type> _types = new();

    public void Register<T>() where T : IBaseUnitOfWork
        => _types[GetKey<T>()] = typeof(T);

    public Type? Resolve<T>()
        => _types.GetValueOrDefault(GetKey<T>());

    private static string GetKey<T>()
        => $"{typeof(T).GetModuleName()}";
}