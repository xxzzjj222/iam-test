using LXT.IAM.Api.Bll.Migration.Dtos;
using LXT.IAM.Api.Common.Orms.Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LXT.IAM.Api.Bll.Migration;

public class MigrateService : IMigrateService
{
    private readonly string _databaseName;
    private readonly IDapperProvider _dapperProvider;
    private readonly ILogger<MigrateService> _logger;

    public MigrateService(IConfiguration configuration, IDapperProvider dapperProvider, ILogger<MigrateService> logger)
    {
        _databaseName = configuration["Db:IAMDb:ConnStr"]?.Split(";").First(x => x.ToLower().Contains("database")).Split("=")[1] ?? string.Empty;
        _dapperProvider = dapperProvider;
        _logger = logger;
    }

    public async Task<string> PretreatVersionAsync()
    {
        var sql = $@"select tab1.SCHEMA_NAME as schemaName,tab2.TABLE_NAME as tableName
                    from information_schema.SCHEMATA tab1
                    left join (select * from information_schema.TABLES where TABLE_NAME='app_version') tab2
                    on tab1.SCHEMA_NAME=tab2.TABLE_SCHEMA
                    where tab1.SCHEMA_NAME='{_databaseName}';";
        var pretreatTableInfo = await _dapperProvider.QueryAsync<dynamic>(sql);
        if (string.IsNullOrWhiteSpace(pretreatTableInfo?.schemaName))
        {
            await _dapperProvider.ExecuteAsync($"CREATE SCHEMA IF NOT EXISTS {_databaseName};");
            await CreateAppVersionTableAsync();
            return "v0.0.0";
        }

        if (string.IsNullOrWhiteSpace(pretreatTableInfo?.tableName))
        {
            await CreateAppVersionTableAsync();
        }
        else
        {
            sql = $"SELECT version_name FROM {_databaseName}.app_version WHERE version_name != '' AND version_name IS NOT NULL ORDER BY id DESC LIMIT 1;";
            var dbVersion = await _dapperProvider.QueryAsync<string>(sql);
            if (!string.IsNullOrWhiteSpace(dbVersion))
            {
                return dbVersion;
            }
        }

        var versionInfos = await GetVersionInfosAsync(Path.Combine(Directory.GetCurrentDirectory(), "Migrations", "Pretreat"));
        foreach (var item in versionInfos)
        {
            var sqlMap = await ReadSqlMapAsync(item.VersionFilePath);
            if (!sqlMap.Any())
            {
                continue;
            }

            var checkSql = sqlMap.First().Value;
            var count = await _dapperProvider.QueryAsync<int>(string.Format(checkSql, _databaseName));
            if (count > 0)
            {
                await InsertVersionAsync(item.Version, "migration pretreat");
                return item.Version;
            }
        }

        return "v0.0.0";
    }

    public async ValueTask UpgradeVersionAsync(string currentDbVersion)
    {
        var currentNum = ConvertVersionToNum(currentDbVersion);
        var versionInfos = await GetVersionInfosAsync(Path.Combine(Directory.GetCurrentDirectory(), "Migrations", "Upgrade"));
        foreach (var versionInfo in versionInfos.Where(x => x.VersionNum > currentNum).OrderBy(x => x.VersionNum))
        {
            var sqlMap = await ReadSqlMapAsync(versionInfo.VersionFilePath);
            foreach (var sql in sqlMap.OrderBy(x => x.Key))
            {
                await _dapperProvider.ExecuteAsync(sql.Value);
            }

            await InsertVersionAsync(versionInfo.Version, "migration upgrade");
            _logger.LogInformation("Executed migration version {Version}", versionInfo.Version);
        }
    }

    private async Task CreateAppVersionTableAsync()
    {
        var sql = $@"CREATE TABLE IF NOT EXISTS {_databaseName}.app_version
                     (
                        id int auto_increment PRIMARY KEY,
                        version_name varchar(100) NOT NULL,
                        remark varchar(100) DEFAULT '',
                        create_by char(36) charset ascii not null,
                        create_time datetime NOT NULL,
                        update_by char(36) charset ascii null,
                        update_time datetime null
                     );
                     CREATE TABLE IF NOT EXISTS {_databaseName}.app_upgrade_log
                     (
                        id int auto_increment PRIMARY KEY,
                        version_name varchar(100) NOT NULL,
                        batch_id char(36) NOT NULL,
                        remark varchar(100) DEFAULT '',
                        create_by char(36) charset ascii not null,
                        create_time datetime NOT NULL,
                        update_by char(36) charset ascii null,
                        update_time datetime null
                     );";
        await _dapperProvider.ExecuteAsync(sql);
    }

    private async Task InsertVersionAsync(string version, string remark)
    {
        var batchId = Guid.NewGuid().ToString();
        var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        var emptyUser = Guid.Empty.ToString();
        var sql = $@"INSERT INTO {_databaseName}.app_version(version_name,remark,create_by,create_time,update_by,update_time)
                     VALUES('{version}','{remark}','{emptyUser}','{now}',NULL,NULL);
                     INSERT INTO {_databaseName}.app_upgrade_log(version_name,batch_id,remark,create_by,create_time,update_by,update_time)
                     VALUES('{version}','{batchId}','{remark}','{emptyUser}','{now}',NULL,NULL);";
        await _dapperProvider.ExecuteAsync(sql);
    }

    private async Task<List<MigrateVersionInfoDto>> GetVersionInfosAsync(string path)
    {
        if (!Directory.Exists(path))
        {
            return new List<MigrateVersionInfoDto>();
        }

        return await Task.FromResult(new DirectoryInfo(path).GetDirectories()
            .Where(x => x.Name.StartsWith("v"))
            .Select(x => new MigrateVersionInfoDto
            {
                Version = x.Name,
                VersionNum = ConvertVersionToNum(x.Name),
                VersionFilePath = x.FullName
            })
            .OrderBy(x => x.VersionNum)
            .ToList());
    }

    private async Task<Dictionary<string, string>> ReadSqlMapAsync(string filePath)
    {
        var result = new Dictionary<string, string>();
        foreach (var fileInfo in new DirectoryInfo(filePath).GetFiles("*.sql"))
        {
            using var reader = new StreamReader(fileInfo.OpenRead());
            var content = await reader.ReadToEndAsync();
            if (!string.IsNullOrWhiteSpace(content))
            {
                result[fileInfo.Name.Split(".").First()] = content;
            }
        }

        return result;
    }

    private int ConvertVersionToNum(string version)
    {
        try
        {
            return Convert.ToInt32(version.ToLower().Replace("v", string.Empty).Replace(".", string.Empty));
        }
        catch
        {
            return 0;
        }
    }
}
