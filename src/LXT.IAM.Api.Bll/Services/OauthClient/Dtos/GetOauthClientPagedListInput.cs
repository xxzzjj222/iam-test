using LXT.IAM.Api.Common.Models.Base;

namespace LXT.IAM.Api.Bll.Services.OauthClient.Dtos;

public class GetOauthClientPagedListInput : PaginationParams
{
    public string? Keyword { get; set; }
}
