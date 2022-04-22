use Net6Review_ProductAPI

exec sp_changedbowner 'sa'

exec sys.sp_cdc_enable_db

select name, is_cdc_enabled from sys.databases

EXEC sys.sp_cdc_enable_table
	@source_schema = 'dbo',
	@source_name = 'Products',
	@role_name = NULL,
	@supports_net_changes = 1

EXECUTE sys.sp_cdc_help_change_data_capture
	@source_schema = 'dbo',
	@source_name = 'Products';

select * from [cdc].[dbo_Products_CT]
