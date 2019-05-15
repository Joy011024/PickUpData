create table if not exists `spilder_uin_data`.`App`
(
	`Id` int primary key,
	`app_name` varchar(32) not null,
	`app_code` varchar(32) not null
);
create table if not exists `spilder_uin_data`.`app_config`
(
	`id` smallint primary key,
	`name` varchar(32) not null,
	`cfg_key` varchar(32) not null,
	`cfg_desc` varchar(512),
	`create_time` datetime,
	`cfg_value` varchar(32) not null,
	`parent_id` smallint 
);
create table if not exists `spilder_uin_data`.`app_ver`
(
	`id` int primary key,
	`app_version` varchar(32) not null,
	`app_id` int not null,
	`is_now_version` tinyint not null, # equal bool
	`create_time` datetime ,
	constraint `fk_id` foreign key (`app_id`) references `app` (`id`)
);
create table if not exists `spilder_uin_data`.`Category_data`
(
	`id` int primary key,
	`name` varchar(20) not null,
	`parent_id` int ,
	`parent_code` varchar(10) ,
	`code` varchar(10) not null,
	`sort` int not null,
	`item_type`	varchar(20) ,
	`is_delete` tinyint not null,
	`create_time` datetime not null,
	`node_level` int not null
);

 