namespace ReLoop.Shared.Abstractions.Kernel.Primitives;

public interface ISoftDelete
{
    bool IsDeleted { get; }
}