#create database  spilder_uin_data;

#database  `spilder_uin_data`.
create table if not exists `App`
(
	`Id` int primary key,
	`app_name` varchar(32) not null,
	`app_code` varchar(32) not null
);
create table if not exists  `app_config`
(
	`id` smallint primary key,
	`name` varchar(32) not null,
	`cfg_key` varchar(32) not null,
	`cfg_desc` varchar(512),
	`create_time` datetime,
	`cfg_value` varchar(32) not null,
	`parent_id` smallint 
);
create table if not exists `app_ver`
(
	`id` int primary key,
	`app_version` varchar(32) not null,
	`app_id` int not null,
	`is_now_version` tinyint not null, # equal bool
	`create_time` datetime ,
	constraint `fk_id` foreign key (`app_id`) references `app` (`id`)
);
create table if not exists  `Category_data`
(
	`id` int primary key,
	`name` varchar(20) not null,
	`parent_id` int ,
	`parent_code` varchar(10) ,
	`code` varchar(10) not null,
	`sort` int not null,
	`item_type`	smallint not null,
	`is_delete` tinyint not null,
	`create_time` datetime not null,
	`node_level` int not null,
	constraint `fk_type_cfg` foreign key(`item_type`)  references `app_config`(`id`)
);
create table if not exists   `database_npk_version` 
(
	`id` int   primary key auto_increment,
	`name` varchar(2) not null,
	`npk_effect_version` varchar(20) not null,
	`npg_svn` varchar(10) not null,
	`npg_submit_time` datetime not null,
	`npk_path` varchar(200) ,
	`npk_type` smallint not null,
	`note` varchar(100) not null,
	`in_db_time` datetime not null,
	`is_delete` tinyint not null,
	`is_release` tinyint ,
	`npk_author` varchar(20) not null,
	`npk_submiter` varchar(20) not null,
	`npg_database` varchar(30),
	constraint `fk_svntype_cfg` foreign key(`npk_type`)  references `app_config`(`id`)
);
create table if not exists`db_type_desc`
(
	`id` smallint primary key auto_increment,
	`name` varchar(30) not null,
	`create_time` datetime not null,
	`remark` varchar(100) not null, 
	`Is_delete` tinyint not null
);
create table if not exists `ignore_image`
(
	`id` varchar(36) primary key,
	`image_url` varchar(500),
	`create_time` datetime not null,
	`is_delete` tinyint not null
);
create table if not exists `key_work`
(
	`id` varchar(36) primary key,
	`word` varchar(64) not null,
	`useage` varchar(128) not null,	`is_delete` int not null,
	`app_id` int not null,
	constraint `fk_app_key_id`  foreign key(`app_id`) references `App` (`id`)
);
create table if not exists `link_server`
(
	`id` int primary key auto_increment,
	`name` varchar(100) not null,
	`link_name_remark` varchar(200) not null,
	`create_time` datetime not null,
	`is_delete` tinyint not null,
	`statue` smallint not null,
	`db_type` smallint not null,
	constraint `fk_link_db_type_id` foreign key(`db_type`) references `db_type_desc` (`id`)
);
create table if not exists `maybe_spell_name`
(
	`id` varchar(36) primary key,
	`name` varchar(3) not null,
	`code` varchar(16) not null,
	`create_time` datetime not null,
	`create_day_int` int not null,
	`is_special_Chinese` tinyint not null
	);
	create table if not exists `operate_event`
	(
		`id` varchar(36) primary key,
		`event_id` smallint not null,
		`create_time`	datetime not null,
		`rely_table_row_value` varchar(36) not null,
		`is_delete` tinyint not null,
		constraint `fk_cfg_event_id` foreign key(`event_id`) references `app_config`(`id`)
	);
	create table if not exists `pick_up_stop_flag`
	(
		 `id` varchar(36) primary key,
		 `create_time` datetime not null,
		 `last_pick_up_time` datetime not null,
		 `pick_up_from_pc` varchar(16) not null,# ip address
		 `data_save_db` varchar(16) not null,
		 `data_save_table` varchar(16) not null,
		 `pick_up_num` int  not null
	);
	create table if not exists `pick_up_tecent_qq_where`
	(
		`id` varchar(36) primary key,
		`key_work` varchar(50) ,
		`session_id` int,
		`agerg` int ,
		`sex` int,
		`firston` int,
		`video` int ,
		`country` int ,
		`province` int ,
		`city` int,
		`hcountry` int,
		`hprovince` int ,
		`hcity` int,
		`hdistinct` int,
		`online` int
	);
create table if not exists `row_value_type`
(
	`id`  int primary key auto_increment,
	`column_name` varchar(32) not null,
	`column_type` varchar(32) not null,
	`table_name` varchar(32) not null,
	`remark` varchar(1024) ,
	`create_time` datetime not null,
	`create_time_day_int` int not null
);
create table if not exists `special_spell_name`
(
	`id` int primary key auto_increment,
	`namer` varchar(2) not null,
	`code` varchar(16) not null,
	`is_deleted` tinyint not null,
	`is_error_spell` tinyint not null
);
create table if not exists `sync_flag`
(
 `id` varchar(32) primary key ,#  change type int 
 `sync_time` datetime not null
);
create table if not exists `sync_number`
(
	`day_int` int primary key,
	`number` int not null,
	`sync_time` datetime not null,
	`sync_id_number` int not null
);
create table if not exists `tecent_qq_data`
(
	`id` varchar(36) not null,
	`pick_up_where_id` varchar(36),
	`age` int ,
	`city` varchar(50),
	`country` varchar(50),
	`distance` int,
	`face` int,
	`gender` int,
	`nick` int,
	`province` varchar(50),
	`stat` int,
	`uin` varchar(15),
	`head_image_url` varchar(500),
	`create_time` datetime not null,
	`is_gather_image` tinyint,
	`gather_image_time` datetime not null,
	`last_error_gather_image` datetime,
	`gather_image_error_num` int not null,
	`image_relative_path` varchar(200),
	`day_int` int,
	`img_type` int
);
create table if not exists `web_chat_frient_date`
(
	`id` varchar(36) primary key,
	`create_time` datetime not null,
	`self_Define_date_tag` varchar(20),
	`self_Define_type` varchar(10),
	`group_sign` varchar(10),
	`date_belong_name` varchar(50),
	`date_belong_username` varchar(100),
	`date_belong_usernick` varchar(100),
	`username` varchar(100),
	`nickname` varchar(500),
	`head_img_url` varchar(500),
	`contact_flag` varchar(10),
	`member_count` int,
	`member_list` varchar(10),
	`remark_name`	varchar(500),
	`hide_input_bar_flag` int,
	`sex` int,
	`sex_Desc` varchar(3),
	`signature` varchar(500),
	`verify_flag` int,
	`owner_uin` int,
	`py_initial` varchar(500),
	`remark_py_initial` varchar(500),
	`remark_py_quanpin` varchar(500),
	`star_frient` int,
	`app_account_flag` varchar(20),
	`statues` int,
	`attr_status` varchar(20),
	`province` varchar(50),
	`city` varchar(50),
	`alias` varchar(20),
	`sns_flag` int,
	`uin_friend` int,
	`display_name` varchar(20),
	`chat_room_id` int,
	`key_work` varchar(1000),
	`encry_chat_room_id` varchar(20),
	`is_owner` int,
	`uin` varchar(20),
	`alias_Desc` varchar(50)
);
 