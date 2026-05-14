# LXT.IAM.Api

统一账号与认证中心项目，按 `LXT.AIEnglish.Api` 的分层风格实现。

## 项目目标

- 提供统一账号、登录注册、验证码、refresh token
- 提供统一客户端访问控制
- 提供统一邀请码与邀请关系管理
- 提供开放接口给业务系统做内部用户查询、服务间认证、统计上报
- 提供平台角色、平台菜单、开放客户端等后台管理能力

## 目录结构

- `src/LXT.IAM.Api.Common`
  - 常量、公共模型、工具、Dapper
- `src/LXT.IAM.Api.Model`
  - 预留模型层
- `src/LXT.IAM.Api.Storage`
  - Entity、DbContext
- `src/LXT.IAM.Api.Dal`
  - 基础仓储
- `src/LXT.IAM.Api.Bll`
  - 业务服务、DTO、Migration、Dashboard、Open 接口能力
- `src/LXT.IAM.Api.Service`
  - Web API 启动入口、Controller、Filter、自定义 SQL Migration

## 技术栈

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core + Pomelo MySQL
- 自定义 SQL 迁移机制
- JWT
- Dapper
- Serilog

## 数据库迁移

项目启动时执行 SQL 脚本升级，不使用 EF Core code-first migration。

迁移目录：

- `src/LXT.IAM.Api.Service/Migrations/Pretreat`
- `src/LXT.IAM.Api.Service/Migrations/Upgrade`

版本记录表：

- `app_version`
- `app_upgrade_log`

## 运行前配置

修改文件：

- `src/LXT.IAM.Api.Service/appsettings.json`

关键配置：

- `Db:IAMDb:ConnStr`
- `Jwt:SecurityKey`
- `SnowflakeIdOptions:WorkId`

## 启动

```powershell
dotnet build LXT.IAM.Api.sln
dotnet run --project src\LXT.IAM.Api.Service\LXT.IAM.Api.Service.csproj --urls http://127.0.0.1:5037
```

Swagger：

- `http://127.0.0.1:5037/swagger/index.html`

## 核心接口

### 认证

- `POST /api/auth/login/password`
- `POST /api/auth/login/code`
- `POST /api/auth/register`
- `POST /api/auth/refresh-token`
- `GET /api/auth/me`
- `PUT /api/auth/change-password`

### 验证码

- `POST /api/verify-code/send`

### 用户管理

- `POST /api/user/page`
- `GET /api/user/{commonUserId}`
- `PUT /api/user/{commonUserId}/freeze`
- `PUT /api/user/{commonUserId}/unfreeze`
- `POST /api/user/{commonUserId}/apps`
- `PUT /api/user/{commonUserId}/reset-password`

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
- `GET /api/open-user/{commonUserId}`
- `POST /api/open-user/batch`
- `POST /api/open-stat/user-activity/report`
- `POST /api/open-stat/business-metric/report`
- `POST /api/open-stat/role-snapshot/report`

### 系统初始化

- `GET /api/system-init/status`
- `POST /api/system-init/initialize`
- `POST /api/system-init/repair`

## 当前实现说明

当前版本已经具备：

- 统一用户主表与凭证表
- 验证码发送落库
- 注册、密码登录、验证码登录、refresh token
- 统一邀请码与 AIThesis 兼容邀请码规则
- 平台角色、平台菜单、开放客户端管理
- 服务间认证与 scope 校验
- 活跃/业务/角色快照上报
- dashboard 聚合与日统计汇总
- 超级管理员初始化与修复

当前仍建议后续补充：

- 真实短信/邮件发送网关
- 微信/抖音三方登录
- 更完整的异常模型与统一错误码
- 平台菜单按钮级权限前端联动
