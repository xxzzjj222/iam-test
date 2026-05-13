using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Base;

[Authorize]
[ApiController]
public class BusinessController : ControllerBase
{
}
