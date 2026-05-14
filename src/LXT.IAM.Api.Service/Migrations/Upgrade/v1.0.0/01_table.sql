create table if not exists user
(
    id                bigint                 not null primary key,
    remark            varchar(1000)          null,
    create_by         char(36) charset ascii not null,
    create_time       datetime               not null,
    update_by         char(36) charset ascii null,
    update_time       datetime               null,
    user_id           char(36)               not null comment '统一用户ID',
    name              varchar(100)           not null comment '名称',
    avatar            varchar(1000)          null comment '头像',
    phone             varchar(50)            null comment '手机号',
    email             varchar(100)           null comment '邮箱',
    status            int                    not null comment '状态',
    is_frozen         tinyint(1)             not null comment '是否冻结',
    register_app_code varchar(100)           not null comment '注册应用编码',
    last_login_time   datetime               null comment '最后登录时间',
    last_active_time  datetime               null comment '最后活跃时间',
    language_code     varchar(20)            null comment '语言编码',
    country_code      varchar(20)            null comment '国家区号',
    constraint uk_user_user_id unique (user_id)
) comment '统一用户';

create table if not exists user_identifier
(
    id              bigint                 not null primary key,
    remark          varchar(1000)          null,
    create_by       char(36) charset ascii not null,
    create_time     datetime               not null,
    update_by       char(36) charset ascii null,
    update_time     datetime               null,
    user_id         char(36)               not null comment '统一用户ID',
    identifier_type varchar(50)            not null comment '标识类型',
    identifier      varchar(200)           not null comment '标识值',
    country_code    varchar(20)            null comment '国家区号',
    is_primary      tinyint(1)             not null comment '是否主标识',
    is_verified     tinyint(1)             not null comment '是否已验证',
    verified_time   datetime               null comment '验证时间',
    constraint uk_user_identifier unique (identifier_type, identifier)
) comment '用户标识';

create table if not exists user_credential
(
    id                        bigint                 not null primary key,
    remark                    varchar(1000)          null,
    create_by                 char(36) charset ascii not null,
    create_time               datetime               not null,
    update_by                 char(36) charset ascii null,
    update_time               datetime               null,
    user_id                   char(36)               not null comment '统一用户ID',
    credential_type           varchar(50)            not null comment '凭证类型',
    password_hash             varchar(500)           not null comment '密码哈希',
    password_version          varchar(50)            null comment '密码版本',
    need_reset_password       tinyint(1)             not null comment '是否需要重置密码',
    last_password_change_time datetime               null comment '最后修改密码时间'
) comment '用户凭证';

create table if not exists user_social_account
(
    id            bigint                 not null primary key,
    remark        varchar(1000)          null,
    create_by     char(36) charset ascii not null,
    create_time   datetime               not null,
    update_by     char(36) charset ascii null,
    update_time   datetime               null,
    user_id       char(36)               not null comment '统一用户ID',
    platform_type varchar(50)            not null comment '平台类型',
    app_id        varchar(100)           not null comment '应用ID',
    open_id       varchar(200)           not null comment 'OpenId',
    union_id      varchar(200)           null comment 'UnionId',
    status        int                    not null comment '状态',
    bind_time     datetime               not null comment '绑定时间',
    unbind_time   datetime               null comment '解绑时间',
    constraint uk_user_social_account unique (platform_type, app_id, open_id)
) comment '用户第三方账号';

create table if not exists verification_code
(
    id            bigint                 not null primary key,
    remark        varchar(1000)          null,
    create_by     char(36) charset ascii not null,
    create_time   datetime               not null,
    update_by     char(36) charset ascii null,
    update_time   datetime               null,
    user_id       char(36)               null comment '统一用户ID',
    receiver      varchar(200)           not null comment '接收方',
    receiver_type varchar(20)            not null comment '接收类型',
    scene_code    varchar(50)            not null comment '场景编码',
    code_hash     varchar(500)           not null comment '验证码哈希',
    expire_time   datetime               not null comment '过期时间',
    status        int                    not null comment '状态',
    send_channel  varchar(50)            not null comment '发送渠道',
    send_ip       varchar(100)           null comment '发送IP',
    used_time     datetime               null comment '使用时间'
) comment '验证码';

create table if not exists login_session
(
    id                        bigint                 not null primary key,
    remark                    varchar(1000)          null,
    create_by                 char(36) charset ascii not null,
    create_time               datetime               not null,
    update_by                 char(36) charset ascii null,
    update_time               datetime               null,
    session_id                char(36)               not null comment '会话ID',
    user_id                   char(36)               not null comment '统一用户ID',
    app_code                  varchar(100)           not null comment '应用编码',
    client_type               varchar(50)            not null comment '客户端类型',
    login_type                varchar(50)            not null comment '登录类型',
    refresh_token_hash        varchar(500)           not null comment 'RefreshToken 哈希',
    access_token_expire_time  datetime               not null comment 'AccessToken 过期时间',
    refresh_token_expire_time datetime               not null comment 'RefreshToken 过期时间',
    device_id                 varchar(200)           null comment '设备ID',
    ip                        varchar(100)           null comment 'IP',
    logout_time               datetime               null comment '登出时间',
    status                    int                    not null comment '状态',
    constraint uk_login_session_session_id unique (session_id)
) comment '登录会话';

create table if not exists app
(
    id                         bigint                 not null primary key,
    remark                     varchar(1000)          null,
    create_by                  char(36) charset ascii not null,
    create_time                datetime               not null,
    update_by                  char(36) charset ascii null,
    update_time                datetime               null,
    name                       varchar(100)           not null comment '名称',
    code                       varchar(100)           not null comment '编码',
    category                   varchar(50)            not null comment '分类',
    client_type                varchar(50)            not null comment '客户端类型',
    auto_grant_for_normal_user tinyint(1)             not null comment '普通用户自动授权',
    sort                       int                    not null comment '排序',
    status                     int                    not null comment '状态',
    description                varchar(1000)          null comment '描述',
    constraint uk_app_code unique (code)
) comment '应用';

create table if not exists user_app
(
    id         bigint                 not null primary key,
    remark     varchar(1000)          null,
    create_by  char(36) charset ascii not null,
    create_time datetime              not null,
    update_by  char(36) charset ascii null,
    update_time datetime              null,
    user_id    char(36)               not null comment '统一用户ID',
    app_id     bigint                 not null comment '应用ID',
    grant_type varchar(50)            not null comment '授权类型',
    status     int                    not null comment '状态',
    constraint uk_user_app unique (user_id, app_id)
) comment '用户应用授权';

create table if not exists platform_role
(
    id          bigint                 not null primary key,
    remark      varchar(1000)          null,
    create_by   char(36) charset ascii not null,
    create_time datetime               not null,
    update_by   char(36) charset ascii null,
    update_time datetime               null,
    name        varchar(100)           not null comment '名称',
    code        varchar(100)           not null comment '编码',
    description varchar(1000)          null comment '描述',
    constraint uk_platform_role_code unique (code)
) comment '平台角色';

create table if not exists user_platform_role
(
    id          bigint                 not null primary key,
    remark      varchar(1000)          null,
    create_by   char(36) charset ascii not null,
    create_time datetime               not null,
    update_by   char(36) charset ascii null,
    update_time datetime               null,
    user_id     char(36)               not null comment '统一用户ID',
    role_id     bigint                 not null comment '角色ID'
) comment '用户平台角色';

create table if not exists platform_function
(
    id            bigint                 not null primary key,
    remark        varchar(1000)          null,
    create_by     char(36) charset ascii not null,
    create_time   datetime               not null,
    update_by     char(36) charset ascii null,
    update_time   datetime               null,
    name          varchar(100)           not null comment '名称',
    code          varchar(100)           not null comment '编码',
    parent_id     bigint                 null comment '父级ID',
    icon          varchar(1000)          null comment '图标',
    type          varchar(50)            not null comment '类型',
    route_url     varchar(200)           null comment '路由地址',
    component_url varchar(200)           null comment '组件地址',
    is_hidden     tinyint(1)             not null comment '是否隐藏',
    sort          int                    not null comment '排序',
    constraint uk_platform_function_code unique (code)
) comment '平台功能';

create table if not exists platform_role_function
(
    id          bigint                 not null primary key,
    remark      varchar(1000)          null,
    create_by   char(36) charset ascii not null,
    create_time datetime               not null,
    update_by   char(36) charset ascii null,
    update_time datetime               null,
    role_id     bigint                 not null comment '角色ID',
    function_id bigint                 not null comment '功能ID'
) comment '平台角色功能';

create table if not exists invite_code
(
    id              bigint                 not null primary key,
    remark          varchar(1000)          null,
    create_by       char(36) charset ascii not null,
    create_time     datetime               not null,
    update_by       char(36) charset ascii null,
    update_time     datetime               null,
    user_id         char(36)               not null comment '统一用户ID',
    code            varchar(100)           not null comment '邀请码',
    code_type       varchar(50)            not null comment '码类型',
    app_code        varchar(100)           null comment '应用编码',
    biz_role_code   varchar(100)           null comment '业务角色编码',
    external_ref_id varchar(100)           null comment '外部引用ID',
    is_default      tinyint(1)             not null comment '是否默认',
    status          int                    not null comment '状态',
    constraint uk_invite_code_code unique (code)
) comment '邀请码';

create table if not exists invite_relation
(
    id                       bigint                 not null primary key,
    remark                   varchar(1000)          null,
    create_by                char(36) charset ascii not null,
    create_time              datetime               not null,
    update_by                char(36) charset ascii null,
    update_time              datetime               null,
    inviter_user_id          char(36)               not null comment '邀请人用户ID',
    invitee_user_id          char(36)               not null comment '被邀请人用户ID',
    invite_code_id           bigint                 not null comment '邀请码ID',
    source_app_code          varchar(100)           not null comment '来源应用编码',
    register_app_code        varchar(100)           not null comment '注册应用编码',
    resolved_biz_role_code   varchar(100)           null comment '解析后的业务角色编码',
    resolved_external_ref_id varchar(100)           null comment '解析后的外部引用ID',
    bind_time                datetime               not null comment '绑定时间'
) comment '邀请关系';

create table if not exists user_app_role_snapshot
(
    id            bigint                 not null primary key,
    remark        varchar(1000)          null,
    create_by     char(36) charset ascii not null,
    create_time   datetime               not null,
    update_by     char(36) charset ascii null,
    update_time   datetime               null,
    user_id       char(36)               not null comment '统一用户ID',
    app_code      varchar(100)           not null comment '应用编码',
    role_code     varchar(100)           not null comment '角色编码',
    role_name     varchar(100)           not null comment '角色名称',
    source_ref_id varchar(100)           null comment '来源引用ID',
    sync_time     datetime               not null comment '同步时间'
) comment '用户应用角色快照';

create table if not exists oauth_client
(
    id                          bigint                 not null primary key,
    remark                      varchar(1000)          null,
    create_by                   char(36) charset ascii not null,
    create_time                 datetime               not null,
    update_by                   char(36) charset ascii null,
    update_time                 datetime               null,
    client_code                 varchar(100)           not null comment '客户端编码',
    client_secret_hash          varchar(500)           not null comment '客户端密钥哈希',
    client_name                 varchar(100)           not null comment '客户端名称',
    grant_type                  varchar(100)           not null comment '授权类型',
    scopes                      varchar(1000)          null comment '权限范围',
    status                      int                    not null comment '状态',
    access_token_expire_seconds int                    not null comment 'AccessToken 有效期秒数',
    constraint uk_oauth_client_client_code unique (client_code)
) comment '开放客户端';

create table if not exists user_app_activity_daily
(
    id               bigint                 not null primary key,
    remark           varchar(1000)          null,
    create_by        char(36) charset ascii not null,
    create_time      datetime               not null,
    update_by        char(36) charset ascii null,
    update_time      datetime               null,
    stat_date        datetime               not null comment '统计日期',
    app_code         varchar(100)           not null comment '应用编码',
    user_id          char(36)               not null comment '统一用户ID',
    active_times     int                    not null comment '活跃次数',
    last_active_time datetime               not null comment '最后活跃时间',
    constraint uk_user_app_activity_daily unique (stat_date, app_code, user_id)
) comment '用户应用活跃日报';

create table if not exists daily_user_stat
(
    id                bigint                 not null primary key,
    remark            varchar(1000)          null,
    create_by         char(36) charset ascii not null,
    create_time       datetime               not null,
    update_by         char(36) charset ascii null,
    update_time       datetime               null,
    stat_date         datetime               not null comment '统计日期',
    app_code          varchar(100)           not null comment '应用编码',
    total_user_count  int                    not null comment '总用户数',
    new_user_count    int                    not null comment '新增用户数',
    active_user_count int                    not null comment '活跃用户数',
    constraint uk_daily_user_stat unique (stat_date, app_code)
) comment '用户统计日报';

create table if not exists daily_business_stat
(
    id           bigint                 not null primary key,
    remark       varchar(1000)          null,
    create_by    char(36) charset ascii not null,
    create_time  datetime               not null,
    update_by    char(36) charset ascii null,
    update_time  datetime               null,
    stat_date    datetime               not null comment '统计日期',
    app_code     varchar(100)           not null comment '应用编码',
    metric_code  varchar(100)           not null comment '指标编码',
    metric_name  varchar(100)           not null comment '指标名称',
    metric_value bigint                 not null comment '指标值',
    constraint uk_daily_business_stat unique (stat_date, app_code, metric_code)
) comment '业务统计日报';
