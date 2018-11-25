-- Static Data

create table "TechGrade" (
    "Id" serial primary key not null,
    "Name" varchar(3) unique not null,
    "Rank" integer not null
);

create table "BGrade" (
    "Id" serial primary key not null,
    "Name" varchar(2) unique not null,
    "Rank" integer not null
);

create table "PoveyGrade" (
    "Id" serial primary key not null,
    "Name" varchar(4) unique not null,
    "Rank" integer not null
);

create table "FurlongGrade" (
    "Id" serial primary key not null,
    "Name" varchar(3) unique not null,
    "Rank" integer not null
);

create table "StyleSymbol" (
    "Id" serial primary key not null,
    "Name" varchar(50) not null unique,
    "Description" varchar(250),
    "SymbolFilePath" varchar(250) unique
);

create table "Hold" (
    "Id" serial primary key not null,
    "Name" varchar(10) not null unique,
    "ParentHoldId" integer null,
    foreign key ("ParentHoldId") references "Hold" ("Id")
);

-- Non Static Data

create table "Problem" (
    "Id" serial primary key not null,
    "Name" varchar(100) unique not null,
    "Description" varchar (5000) null,
    "TechGradeId" integer null,
    "BGradeId" integer null,
    "PoveyGradeId" integer null,
    "FurlongGradeId" integer null,
	"DateSet" timestamp null,
	"FirstAscent" varchar(30) null,
    foreign key ("TechGradeId") references "TechGrade" ("Id"),
    foreign key ("BGradeId") references "BGrade" ("Id"),
    foreign key ("PoveyGradeId") references "PoveyGrade" ("Id"),
    foreign key ("FurlongGradeId") references "FurlongGrade" ("Id"),
    check ("TechGradeId" is not null or "BGradeId" is not null or "PoveyGradeId" is not null or "FurlongGradeId" is not null)
);

create table "HoldRule" (
    "Id" serial primary key not null,
    "Name" varchar(30) unique not null,
    "Description" varchar(1000) null
);

create table "ProblemHold" (
    "Id" serial primary key not null,
	"Position" integer not null,
	"IsStandingStartHold" boolean not null default false,
    "ProblemId" integer not null,
    "HoldId" integer not null,
    foreign key ("ProblemId") references "Problem" ("Id"),
    foreign key ("HoldId") references "Hold" ("Id")
);

create table "ProblemHoldRule" (
	"Id" serial primary key not null,
	"ProblemHoldId" integer not null,
	"HoldRuleId" integer not null,
	foreign key ("ProblemHoldId") references "ProblemHold" ("Id"),
    foreign key ("HoldRuleId") references "Hold" ("Id")
);

create table "ProblemStyleSymbol" (
    "Id" serial primary key not null,
    "ProblemId" integer not null,
    "StyleSymbolId" integer not null,
    foreign key ("ProblemId") references "Problem" ("Id"),
    foreign key ("StyleSymbolId") references "StyleSymbol" ("Id")
);

create table "GeneralRule" (
    "Id" serial primary key not null,
    "Name" varchar(50) unique not null,
	"Description" varchar(250) null
);

create table "ProblemRule" (
	"Id" serial primary key not null,
	"ProblemId" integer not null,
	"GeneralRuleId" integer not null,
	foreign key ("ProblemId") references "Problem" ("Id"),
	foreign key ("GeneralRuleId") references "GeneralRule" ("Id")
);

create table "ProblemIssue" (
	"Id" serial primary key not null,
	"ProblemId" integer not null,
	"Description" varchar(5000) not null,
	"SubmittedBy" varchar(50) not null,
	foreign key ("ProblemId") references "Problem" ("Id")
);

create table "Issue" (
	"Id" serial primary key not null,
	"Description" varchar(5000) not null,
	"SubmittedBy" varchar(50) not null
);