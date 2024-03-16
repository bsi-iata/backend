create table if not exists iata.contacts
(
    Id         bigint auto_increment
    primary key,
    Name       varchar(256)  not null,
    Phone      varchar(128)  not null,
    Email      varchar(256)  not null,
    Contry     varchar(256)  not null,
    State      varchar(256)  not null,
    City       varchar(256)  not null,
    Zip        varchar(256)  not null,
    Address    varchar(1024) not null,
    CreateTime datetime      not null,
    UpdateTime datetime      not null
    )
    comment '联系人';

create index contacts_Name_index
    on iata.contacts (Name);

create table if not exists iata.goods
(
    Id         bigint auto_increment
    primary key,
    PackageId  bigint        not null,
    Commodity  varchar(256)  not null,
    Pcs        double        not null,
    Price      double        not null,
    Amount     double        not null,
    HsCode     varchar(128)  not null,
    `Usage`    varchar(256)  not null,
    Materia    varchar(128)  not null,
    Orginal    varchar(256)  not null,
    Photo      varchar(1024) not null,
    AIResult   varchar(2048) not null,
    CreateTime datetime      not null,
    UpdateTime datetime      not null
    );

create index goods_PackageId_index
    on iata.goods (PackageId);

create table if not exists iata.orders
(
    Code             varchar(128) not null
    primary key,
    UserId           bigint       not null,
    Airline          varchar(32)  not null,
    Flight           varchar(128) not null,
    Status           int          not null,
    Reference        varchar(128) not null,
    Remark           varchar(512) not null,
    DepartureAddress varchar(512) not null,
    ArrivalAddress   varchar(512) not null,
    Departure        datetime     null,
    Arrival          datetime     not null,
    CreateTime       datetime     not null,
    UpdateTime       datetime     not null
    );

create index orders_UserId_index
    on iata.orders (UserId);

create table if not exists iata.packages
(
    Id         bigint auto_increment comment '自增主键'
    primary key,
    OrderCode  varchar(128) not null,
    ContactId  bigint       not null,
    Weight     double       not null,
    Volumn     double       not null,
    Quantity   double       not null,
    Remark     varchar(512) not null,
    GoodsDesc  varchar(512) not null,
    CreateTime datetime     not null,
    UpdateTime datetime     not null
    );

create index packages_ContactId_index
    on iata.packages (ContactId);

create index packages_OrderCode_index
    on iata.packages (OrderCode);

