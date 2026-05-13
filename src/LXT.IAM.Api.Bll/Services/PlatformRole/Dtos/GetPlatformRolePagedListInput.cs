using LXT.IAM.Api.Common.Models.Base;

namespace LXT.IAM.Api.Bll.Services.PlatformRole.Dtos;

public class GetPlatformRolePagedListInput : PaginationParams
{
    public string? Keyword { get; set; }
}
