# LXT.IAM.Api

统一账号、认证与授权中心项目，整体分层和接口风格参考 `LXT.AIEnglish.Api`。

## 项目目标

- 提供统一账号注册、登录、刷新令牌、当前用户信息能力
- 提供统一验证码发送能力，支持短信和邮箱
- 提供统一第三方登录接入能力，当前已支持微信小程序、抖音小程序
- 提供统一用户、应用、平台角色、平台功能、OAuth 客户端管理能力
- 提供统一邀请码与邀请关系管理能力
- 提供统一开放接口，供业务系统做用户查询、服务间认证、统计上报
- 提供统一首页统计所需的用户趋势、活跃趋势、业务指标汇总能力

## 目录结构

- `src/LXT.IAM.Api.Common`
  常量、异常、公共模型、工具类、Dapper
- `src/LXT.IAM.Api.Model`
  配置模型、多语言资源
- `src/LXT.IAM.Api.Storage`
  实体、`DbContext`
- `src/LXT.IAM.Api.Dal`
  基础仓储
- `src/LXT.IAM.Api.Bll`
  业务服务、DTO、Mapper、统计与开放接口能力
- `src/LXT.IAM.Api.Service`
  Web API 启动入口、控制器、中间件、SQL Migration

## 技术栈

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core + Pomelo MySQL
- 自定义 SQL Migration 机制
- JWT
- Dapper
- Serilog
- Alibaba Cloud SMS SDK
- MailKit
- SKIT.FlurlHttpClient.Wechat.Api
- SKIT.FlurlHttpClient.ByteDance.MicroApp

## 数据库迁移

项目启动时自动执行 SQL 升级脚本，不使用 EF Core Code First Migration。

迁移目录：

- `src/LXT.IAM.Api.Service/Migrations/Pretreat`
- `src/LXT.IAM.Api.Service/Migrations/Upgrade`

版本记录表：

- `app_version`
- `app_upgrade_log`

当前已包含：

- `v1.0.0` 初始化建表与基础种子数据

## 运行前配置

修改文件：

- `src/LXT.IAM.Api.Service/appsettings.json`

关键配置项：

- `Db:IAMDb:ConnStr`
- `Jwt:SecurityKey`
- `SnowflakeIdOptions:WorkId`
- `Swagger:AutoOpenBrowser`
- `SMS`
- `Email`
- `AppOptions`
- `DouyinAppOptions`

### 短信配置

参考 `ZKBX.AIThesis.API`：

- `SMS:Enabled`
- `SMS:ALIBABA_CLOUD_ACCESS_KEY_ID`
- `SMS:ALIBABA_CLOUD_ACCESS_KEY_SECRET`
- `SMS:ALIBABA_CLOUD_ACCESS_SIGNNAME`
- `SMS:ALIBABA_CLOUD_ACCESS_TEMPLATECODE`
- `SMS:SendIntervalSeconds`
- `SMS:LimitTimeMinutes`
- `SMS:LimitCount`
- `SMS:TemplateText`

### 邮箱配置

参考 `ZKBX.AIThesis.API`：

- `Email:Enabled`
- `Email:PostfixServerIp`
- `Email:PostfixServerPort`
- `Email:CurrName`
- `Email:CurrEmailAddr`
- `Email:MailPwd`
- `Email:ELimitTime`
- `Email:ElimitCount`
- `Email:SendIntervalSeconds`
- `Email:SubjectTemplate`
- `Email:BodyTemplate`

### 微信小程序配置

- `AppOptions:AppId`
- `AppOptions:AppSecret`

### 抖音小程序配置

- `DouyinAppOptions:AppId`
- `DouyinAppOptions:AppSecret`
- `DouyinAppOptions:AccessToken`

## 启动

```powershell
dotnet build LXT.IAM.Api.sln
dotnet run --project src\LXT.IAM.Api.Service\LXT.IAM.Api.Service.csproj --urls http://127.0.0.1:5037
```

开发环境下：

- `launchSettings.json` 已开启浏览器自动打开
- `appsettings.json` 中 `Swagger:AutoOpenBrowser=true` 时，直接 `dotnet run` 也会自动打开 Swagger

Swagger 地址：

- `http://127.0.0.1:5037/swagger/index.html`

## 核心接口

### 认证

- `POST /api/auth/login/password`
- `POST /api/auth/login/code`
- `POST /api/auth/login/wechat-miniapp`
- `POST /api/auth/login/douyin-miniapp`
- `POST /api/auth/register`
- `POST /api/auth/refresh-token`
- `GET /api/auth/me`
- `PUT /api/auth/change-password`

### 验证码

- `POST /api/verify-code/send`

说明：

- `receiverType=phone` 时走短信发送
- `receiverType=email` 时走邮箱发送
- 已支持发送频控、时间窗口次数限制

### 用户管理

- `POST /api/user/page`
- `GET /api/user/{userId}`
- `PUT /api/user/{userId}/freeze`
- `PUT /api/user/{userId}/unfreeze`
- `POST /api/user/{userId}/apps`
- `PUT /api/user/{userId}/reset-password`

### 应用管理

- `POST /api/app/page`
- `GET /api/app/options`

### 邀请码

- `GET /api/invite-code/my`
- `GET /api/invite-code/resolve/{code}`

### Dashboard

- `GET /api/dashboard/overview`
- `POST /api/dashboard/user-trend`
- `POST /api/dashboard/activity-trend`
- `POST /api/dashboard/business-trend`
- `POST /api/daily-stat/refresh`

### 平台权限

- `POST /api/platform-role/page`
- `GET /api/platform-role/options`
- `POST /api/platform-role`
- `PUT /api/platform-role/{id}`
- `POST /api/platform-role/assign-user-roles`
- `GET /api/platform-function/tree`
- `GET /api/platform-function/current-user-functions`
- `POST /api/platform-function`
- `POST /api/platform-function/assign-role-functions`

### 开放接口

- `POST /api/open-auth/token`
- `POST /api/open-auth/introspect`
- `GET /api/open-user/by-app`
- `GET /api/open-user/{userId}`
- `POST /api/open-user/batch`
- `POST /api/open-stat/user-activity/report`
- `POST /api/open-stat/business-metric/report`
- `POST /api/open-stat/role-snapshot/report`

### 系统初始化

- `GET /api/system-init/status`
- `POST /api/system-init/initialize`
- `POST /api/system-init/repair`

## 当前实现说明

当前版本已具备：

- 统一用户主表、标识表、凭证表、登录会话表
- 统一异常模型与多语言错误输出
- 短信验证码、邮箱验证码发送
- 密码登录、验证码登录、注册、刷新令牌
- 微信小程序登录、抖音小程序登录
- 统一邀请码与邀请关系模型
- 平台角色、平台功能、OAuth 客户端管理
- 服务间认证与 Scope 校验
- 活跃上报、业务指标上报、角色快照上报
- Dashboard 汇总与日报刷新
- 超级管理员初始化与修复

后续建议继续补充：

- 微信公众号登录
- 更完整的异常资源国际化整理
- 更细粒度的平台按钮权限
- 业务系统对接改造脚本与联调文档


