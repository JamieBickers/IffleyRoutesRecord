-- Static Data

create table TechGrade (
    Id integer primary key,
    Name varchar(3) not null,
    Rank integer not null
);

create table BGrade (
    Id integer primary key,
    Name varchar(3) not null,
    Rank integer not null
);

create table PoveyGrade (
    Id integer primary key,
    Name varchar(3) not null,
    Rank integer not null
);

create table FurlongGrade (
    Id integer primary key,
    Name varchar(3) not null,
    Rank integer not null
);

create table StyleSymbol (
    Id integer primary key,
    Name varchar(50) not null unique,
    Description varchar(250),
    SymbolFilePath varchar(250) unique
);

-- Non Static Data

create table Hold (
    Id integer primary key,
    Name varchar(10) not null unique,
    ParentHoldId integer null,
    foreign key (ParentHoldId) references Hold (Id)
);

create table Problem (
    Id integer primary key,
    Name varchar(100) unique not null,
    Description varchar (5000) not null,
    TechGradeId integer null,
    BGradeId integer null,
    PoveyGradeId integer null,
    FurlongGradeId integer null,
    Verified boolean not null default false,
    foreign key (TechGradeId) references TechGrade (Id),
    foreign key (BGradeId) references BGrade (Id),
    foreign key (PoveyGradeId) references PoveyGrade (Id),
    foreign key (FurlongGradeId) references FurlongGrade (Id),
    check (TechGradeId is not null or BGradeId is not null or PoveyGradeId is not null or FurlongGradeId is not null)
);

create table HoldRule (
    Id integer primary key,
    Name varchar(30) not null unique,
    Description varchar(1000) null
);

create table ProblemHold (
    Id integer primary key,
    ProblemId integer not null,
    HoldId integer not null,
    foreign key (ProblemId) references Problem (Id),
    foreign key (HoldId) references Hold (Id)
);

create table ProblemHoldRule (
	Id integer primary key,
	ProblemHoldId integer not null,
	HoldRuleId integer not null,
	foreign key (ProblemHoldId) references Problem (Id),
    foreign key (HoldRuleId) references Hold (Id)
);

create table ProblemStyleSymbol (
    Id integer primary key,
    ProblemId integer not null,
    StyleSymbolId integer not null,
    foreign key (ProblemId) references Problem (Id)
    foreign key (StyleSymbolId) references StyleSymbol (Id)
);

create table GeneralRule (
    Id integer primary key,
    Name varchar(20) not null,
	Description varchar(250) null
);

create table ProblemRule (
	Id integer primary key,
	ProblemId integer not null,
	GeneralRuleId integer not null,
	foreign key (ProblemId) references Problem (Id),
	foreign key (GeneralRuleId) references Problem (GeneralRule)
);