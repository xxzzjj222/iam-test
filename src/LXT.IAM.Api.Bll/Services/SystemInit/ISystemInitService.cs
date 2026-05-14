using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Bll.Services.SystemInit.Dtos;

namespace LXT.IAM.Api.Bll.Services.SystemInit;

public interface ISystemInitService : IScopedDependency
{
    Task<InitializeSystemOutput> InitializeAsync(InitializeSystemInput input);
    Task<SystemInitStatusOutput> GetStatusAsync();
    Task<SystemRepairOutput> RepairAsync();
}
