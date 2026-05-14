insert into app (id, remark, create_by, create_time, update_by, update_time, name, code, category, client_type, auto_grant_for_normal_user, sort, status, description)
values
(1001, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'AIThesis Web前台', 'AIThesis_Web', 'front', 'web', 1, 1, 1, 'AIThesis Web 前台'),
(1002, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'AIThesis 小程序普通用户', 'AIThesis_Applet_User', 'front', 'applet', 1, 2, 1, 'AIThesis 小程序普通用户端'),
(1003, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'AIThesis 小程序商务', 'AIThesis_Applet_Business', 'front', 'applet', 0, 3, 1, 'AIThesis 小程序商务端'),
(1004, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'AIThesis App', 'AIThesis_App', 'front', 'app', 0, 4, 1, 'AIThesis App'),
(1005, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'AIThesis 管理后台', 'AIThesis_Management', 'management', 'web', 0, 5, 1, 'AIThesis 后台管理'),
(1006, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'AIEnglish H5', 'AIEnglish_H5', 'front', 'h5', 1, 6, 1, 'AIEnglish H5'),
(1007, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'AIEnglish 小程序', 'AIEnglish_MiniApp', 'front', 'applet', 1, 7, 1, 'AIEnglish 小程序'),
(1008, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'AIEnglish App', 'AIEnglish_App', 'front', 'app', 1, 8, 1, 'AIEnglish App'),
(1009, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'AIEnglish 管理后台', 'AIEnglish_Management', 'management', 'web', 0, 9, 1, 'AIEnglish 后台管理'),
(1010, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'FileProject', 'FileProject', 'front', 'web', 1, 10, 1, '文件项目')
on duplicate key update
name = values(name),
category = values(category),
client_type = values(client_type),
auto_grant_for_normal_user = values(auto_grant_for_normal_user),
sort = values(sort),
status = values(status),
description = values(description),
update_time = now();

insert into platform_role (id, remark, create_by, create_time, update_by, update_time, name, code, description)
values
(2001, null, '00000000-0000-0000-0000-000000000000', now(), null, null, '超级管理员', 'SUPER_ADMIN', '统一账号系统超级管理员'),
(2002, null, '00000000-0000-0000-0000-000000000000', now(), null, null, '账号管理员', 'USER_CENTER_ADMIN', '统一账号系统管理员')
on duplicate key update
name = values(name),
description = values(description),
update_time = now();

insert into platform_function (id, remark, create_by, create_time, update_by, update_time, name, code, parent_id, icon, type, route_url, component_url, is_hidden, sort)
values
(4001, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'Dashboard', 'dashboard', null, 'dashboard', 'MENU', 'dashboard', 'dashboard/index', 0, 1),
(4002, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'User', 'user', null, 'user', 'MENU', 'user', 'user/index', 0, 2),
(4003, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'Platform Role', 'platform-role', null, 'lock', 'MENU', 'platform-role', 'platform-role/index', 0, 3),
(4004, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'Oauth Client', 'oauth-client', null, 'key', 'MENU', 'oauth-client', 'oauth-client/index', 0, 4)
on duplicate key update
name = values(name),
parent_id = values(parent_id),
icon = values(icon),
type = values(type),
route_url = values(route_url),
component_url = values(component_url),
is_hidden = values(is_hidden),
sort = values(sort),
update_time = now();

insert into platform_role_function (id, remark, create_by, create_time, update_by, update_time, role_id, function_id)
values
(5001, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 2001, 4001),
(5002, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 2001, 4002),
(5003, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 2001, 4003),
(5004, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 2001, 4004)
on duplicate key update
role_id = values(role_id),
function_id = values(function_id),
update_time = now();

insert into oauth_client (id, remark, create_by, create_time, update_by, update_time, client_code, client_secret_hash, client_name, grant_type, scopes, status, access_token_expire_seconds)
values
(3001, null, '00000000-0000-0000-0000-000000000000', now(), null, null, 'iam_internal', '4E738CA5563C06CFD0018299933D58DB1DD8BF97F6973DC99BF6CDC64B5550BD', 'IAM 内部客户端', 'client_credentials', 'user.read,user.write,stat.write', 1, 7200)
on duplicate key update
client_secret_hash = values(client_secret_hash),
client_name = values(client_name),
grant_type = values(grant_type),
scopes = values(scopes),
status = values(status),
access_token_expire_seconds = values(access_token_expire_seconds),
update_time = now();
