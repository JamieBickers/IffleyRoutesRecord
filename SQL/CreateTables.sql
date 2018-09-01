-- Static Data

create table TechGrade (
    Id integer primary key not null,
    Name varchar(3) not null,
    Rank integer not null
);

create table BGrade (
    Id integer primary key not null,
    Name varchar(3) not null,
    Rank integer not null
);

create table PoveyGrade (
    Id integer primary key not null,
    Name varchar(3) not null,
    Rank integer not null
);

create table FurlongGrade (
    Id integer primary key not null,
    Name varchar(3) not null,
    Rank integer not null
);

create table StyleSymbol (
    Id integer primary key not null,
    Name varchar(50) not null unique,
    Description varchar(250),
    SymbolFilePath varchar(250) unique
);

create table Hold (
    Id integer primary key not null,
    Name varchar(10) not null unique,
    ParentHoldId integer null,
    foreign key (ParentHoldId) references Hold (Id)
);

-- Non Static Data

create table Problem (
    Id integer primary key not null,
    Name varchar(100) unique not null,
    Description varchar (5000) not null,
    TechGradeId integer null,
    BGradeId integer null,
    PoveyGradeId integer null,
    FurlongGradeId integer null,
	DateSet datetime null,
	FirstAscent varchar(30) null,
    foreign key (TechGradeId) references TechGrade (Id),
    foreign key (BGradeId) references BGrade (Id),
    foreign key (PoveyGradeId) references PoveyGrade (Id),
    foreign key (FurlongGradeId) references FurlongGrade (Id),
    check (TechGradeId is not null or BGradeId is not null or PoveyGradeId is not null or FurlongGradeId is not null)
);

create table HoldRule (
    Id integer primary key not null,
    Name varchar(30) not null unique,
    Description varchar(1000) null
);

create table ProblemHold (
    Id integer primary key not null,
	Position integer not null,
	IsStandingStartHold boolean not null default false,
    ProblemId integer not null,
    HoldId integer not null,
    foreign key (ProblemId) references Problem (Id),
    foreign key (HoldId) references Hold (Id)
);

create table ProblemHoldRule (
	Id integer primary key not null,
	ProblemHoldId integer not null,
	HoldRuleId integer not null,
	foreign key (ProblemHoldId) references ProblemHold (Id),
    foreign key (HoldRuleId) references Hold (Id)
);

create table ProblemStyleSymbol (
    Id integer primary key not null,
    ProblemId integer not null,
    StyleSymbolId integer not null,
    foreign key (ProblemId) references Problem (Id)
    foreign key (StyleSymbolId) references StyleSymbol (Id)
);

create table GeneralRule (
    Id integer primary key not null,
    Name varchar(20) not null,
	Description varchar(250) null
);

create table ProblemRule (
	Id integer primary key not null,
	ProblemId integer not null,
	GeneralRuleId integer not null,
	foreign key (ProblemId) references Problem (Id),
	foreign key (GeneralRuleId) references GeneralRule (Id)
);