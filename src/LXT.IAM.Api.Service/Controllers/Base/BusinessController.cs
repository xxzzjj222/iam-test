using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Base;

/// <summary>
/// 业务控制器基类
/// </summary>
[Authorize]
[ApiController]
public class BusinessController : ControllerBase
{
}
