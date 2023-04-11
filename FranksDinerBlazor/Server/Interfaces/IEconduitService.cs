using FranksDinerBlazor.Shared.Models.Econduit;

namespace FranksDinerBlazor.Server.Interfaces
{
    public interface IEconduitService
    {
        Task<bool> RunTransaction(RunTransaction parameters);
    }
}