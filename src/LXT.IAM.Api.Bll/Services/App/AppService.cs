using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Bll.Services.App.Dtos;

namespace LXT.IAM.Api.Bll.Services.App;

public class AppService : IAppService
{
    public Task<PagedList<AppOutput>> GetPagedListAsync(GetAppPagedListInput input)
    {
        return Task.FromResult(new PagedList<AppOutput>
        {
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            Total = 2,
            Items =
            [
                new AppOutput { Id = 1006, Name = "AIEnglish H5", Code = "AIEnglish_H5", Category = "front", ClientType = "h5", AutoGrantForNormalUser = true, Status = 1 },
                new AppOutput { Id = 1009, Name = "AIEnglish 管理后台", Code = "AIEnglish_Management", Category = "management", ClientType = "web", AutoGrantForNormalUser = false, Status = 1 }
            ]
        });
    }

    public Task<List<LongStringKV>> GetOptionsAsync()
    {
        return Task.FromResult(new List<LongStringKV>
        {
            new() { Key = 1006, Value = "AIEnglish_H5" },
            new() { Key = 1009, Value = "AIEnglish_Management" }
        });
    }
}
