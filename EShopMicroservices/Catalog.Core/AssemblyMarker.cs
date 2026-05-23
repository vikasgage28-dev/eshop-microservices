namespace Catalog.Core
{
    /// <summary>
    /// Marker class used to reference the Catalog.Core assembly.
    /// Used by MediatR registration in Catalog.API's Program.cs:
    ///   AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly))
    /// </summary>
    public abstract class AssemblyMarker { }
}
