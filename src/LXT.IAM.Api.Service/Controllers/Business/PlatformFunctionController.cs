using LXT.IAM.Api.Bll.Services.PlatformFunction;
using LXT.IAM.Api.Bll.Services.PlatformFunction.Dtos;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

[Route("api/platform-function")]
public class PlatformFunctionController : BusinessController
{
    private readonly IPlatformFunctionService _platformFunctionService;

    public PlatformFunctionController(IPlatformFunctionService platformFunctionService)
    {
        _platformFunctionService = platformFunctionService;
    }

    [HttpGet("tree")]
    public async Task<List<PlatformFunctionOutput>> GetTreeAsync()
    {
        return await _platformFunctionService.GetTreeAsync();
    }

    [HttpGet("current-user-functions")]
    public async Task<List<PlatformFunctionOutput>> GetCurrentUserFunctionsAsync()
    {
        return await _platformFunctionService.GetCurrentUserFunctionsAsync();
    }

    [HttpGet("{id}")]
    public async Task<PlatformFunctionOutput> GetAsync(long id)
    {
        return await _platformFunctionService.GetAsync(id);
    }

    [HttpPost]
    public async Task<long> AddAsync([FromBody] AddPlatformFunctionInput input)
    {
        return await _platformFunctionService.AddAsync(input);
    }

    [HttpPut("{id}")]
    public async Task PutAsync(long id, [FromBody] PutPlatformFunctionInput input)
    {
        await _platformFunctionService.PutAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(long id)
    {
        await _platformFunctionService.DeleteAsync(id);
    }

    [HttpPost("assign-role-functions")]
    public async Task AssignRoleFunctionsAsync([FromBody] AssignPlatformRoleFunctionsInput input)
    {
        await _platformFunctionService.AssignRoleFunctionsAsync(input);
    }
}
