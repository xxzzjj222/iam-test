select count(*)
from information_schema.TABLES
where TABLE_SCHEMA = '{0}'
  and TABLE_NAME = 'user';
