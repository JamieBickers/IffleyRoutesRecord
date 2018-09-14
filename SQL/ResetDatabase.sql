drop table ProblemHoldRule;
drop table ProblemHold;
drop table HoldRule;
drop table ProblemStyleSymbol;
drop table StyleSymbol;
drop table Hold;
drop table ProblemRule;
drop table Problem;
drop table GeneralRule;
drop table BGrade;
drop table FurlongGrade;
drop table TechGrade;
drop table PoveyGrade;

-- Static Data

create table TechGrade (
    Id integer primary key not null,
    Name varchar(3) unique not null,
    Rank integer not null
);

create table BGrade (
    Id integer primary key not null,
    Name varchar(3) unique not null,
    Rank integer not null
);

create table PoveyGrade (
    Id integer primary key not null,
    Name varchar(3) unique not null,
    Rank integer not null
);

create table FurlongGrade (
    Id integer primary key not null,
    Name varchar(3) unique not null,
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
    Name varchar(30) unique not null,
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
    Name varchar(20) unique not null,
	Description varchar(250) null
);

create table ProblemRule (
	Id integer primary key not null,
	ProblemId integer not null,
	GeneralRuleId integer not null,
	foreign key (ProblemId) references Problem (Id),
	foreign key (GeneralRuleId) references GeneralRule (Id)
);

insert into TechGrade (Name, Rank) values
('4a-', 1),
('4a', 2),
('4a+', 3),
('4b-', 4),
('4b', 5),
('4b+', 6),
('4c-', 7),
('4c', 8),
('4c+', 9),
('5a-', 10),
('5a', 11),
('5a+', 12),
('5b-', 13),
('5b', 14),
('5b+', 15),
('5c-', 16),
('5c', 17),
('5c+', 18),
('6a-', 19),
('6a', 20),
('6a+', 21),
('6b-', 22),
('6b', 23),
('6b+', 24),
('6c-', 25),
('6c', 26),
('6c+', 27);

insert into BGrade (Name, Rank) values
('B0', 1),
('B1', 2),
('B2', 3),
('B3', 4),
('B4', 5),
('B5', 6),
('B6', 7),
('B7', 8);

insert into PoveyGrade (Name, Rank) values
('Easy', 1),
('4b', 2),
('Hard', 3);

insert into FurlongGrade (Name, Rank) values
('A', 1),
('AA', 2),
('AAA', 3),
('XXX', 4),
('WTF', 5);

insert into StyleSymbol (Name, Description) values
('One Star', ''),
('Two Stars', ''),
('Three Stars', ''),
('Four Stars', ''),
('Suitable for All', ''),
('Tall Man', ''),
('Technical', ''),
('Flexible', ''),
('Strong Man', ''),
('Dynamic', ''),
('Fingery', ''),
('Infamous', ''),
('Ambulance', '');

insert into Hold(Id, Name, ParentHoldId) values
(1, '1', null),
(2, '2', null),
(3, '3', null),
(4, '4', null),
(5, '5', null),
(6, '6', null),
(7, '7', null),
(8, '8', null),
(9, '9', null),
(10, '10', null),
(11, '11', null),
(12, '12', null),
(13, '13', null),
(14, '14', null),
(15, '15', null),
(16, '16', null),
(17, '17', null),
(18, '18', null),
(19, '19', null),
(20, '20', null),
(21, '21', null),
(22, '22', null),
(23, '23', null),
(24, '24', null),
(25, '25', null),
(26, '26', null),
(27, '27', null),
(28, '28', null),
(29, '29', null),
(30, '30', null),
(31, '31', null),
(32, '32', null),
(33, '33', null),
(34, '34', null),
(35, '35', null),
(36, '36', null),
(37, '37', null),
(38, '38', null),
(39, '39', null),
(40, '40', null),
(41, '41', null),
(42, '42', null),
(43, '43', null),
(44, '44', null),
(45, '45', null),
(46, '46', null),
(47, '47', null),
(48, '48', null),
(49, '49', null),
(50, '50', null),
(51, '51', null),
(52, '52', null),
(53, '53', null),
(54, '54', null),
(55, '55', null),
(56, '56', null),
(57, '57', null),
(58, '58', null),
(59, '59', null),
(60, '60', null),
(61, '61', null),
(62, '62', null),
(63, '63', null),
(64, '64', null),
(65, '65', null),
(66, '66', null),
(67, '67', null),
(68, '68', null),
(69, '69', null),
(70, '70', null),
(71, '71', null),
(72, '72', null),
(73, '73', null),
(74, '74', null),
(75, '75', null),
(76, '76', null),
(77, '77', null),
(78, '78', null),
(79, '79', null),
(80, '80', null),
(81, '81', null),
(82, '82', null),
(83, '83', null),
(84, '84', null),
(85, '85', null),
(86, '86', null),
(87, '87', null),
(88, '88', null),
(89, '89', null),
(90, '90', null),
(91, '91', null),
(92, '92', null),
(93, '93', null),
(94, '94', null),
(95, '95', null),
(96, '96', null),
(97, '97', null),
(98, '98', null),
(99, '99', null),
(100, '100', null),
(101, '101', null),
(102, '102', null),
(103, '103', null),
(104, '104', null),
(105, '105', null),
(106, '106', null),
(107, '107', null),
(108, '108', null),
(109, '109', null),
(110, '110', null),
(111, '111', null),
(112, '112', null),
(113, '113', null),
(114, '114', null),
(115, '115', null),
(116, '116', null),
(117, '117', null);

insert into Hold(Id, Name, ParentHoldId) values
(118, '58A', 58),
(119, '100A', 100),
(120, '57A', 57),
(121, '85A', 85),
(122, '113B', 113),
(123, '112B', 112),
(124, '15A', 15),
(125, '77B', 77),
(126, '75B', 75),
(127, '79A', 79),
(128, '53C', 53),
(129, '82A', 82),
(130, '113C', 113),
(131, '41A', 41),
(132, '40B', 40),
(133, '94B', 94),
(134, '96B', 96),
(135, '84B', 84),
(136, '113A', 113),
(137, '47A', 47),
(138, '19B', 19),
(139, '64B', 64),
(140, '4B', 4),
(141, '48A', 48),
(142, '73B', 73),
(143, '78A', 78),
(144, '76A', 76),
(145, '107A', 107),
(146, '29A', 29),
(147, '86B', 86),
(148, '23A', 23),
(149, '69A', 69),
(150, '47B', 47),
(151, '72B', 72),
(152, '103B', 103),
(153, '99B', 99),
(154, '29B', 29),
(155, '67A', 67),
(156, '64A', 64),
(157, '111A', 111),
(158, '15B', 15),
(159, '44B', 44),
(160, '96A', 96),
(161, '76B', 76),
(162, '9B', 9),
(163, '97B', 97),
(164, '22A', 22),
(165, '70B', 70),
(166, '102B', 102),
(167, '78B', 78),
(168, '74B', 74),
(169, '19A', 19),
(170, '101B', 101),
(171, '109A', 109),
(172, '109C', 109),
(173, '69B', 69),
(174, '41B', 41),
(175, '106B', 106),
(176, '18B', 18),
(177, '39B', 39),
(178, '14B', 14),
(179, '48B', 48),
(180, '101A', 101),
(181, '114A', 114),
(182, '97A', 97),
(183, '14A', 14),
(184, '99A', 99),
(185, '61B', 61),
(186, '67B', 67),
(187, '85B', 85),
(188, '75A', 75),
(189, '116A', 116),
(190, '94A', 94),
(191, '84A', 84),
(192, '38B', 38),
(193, '38C', 38),
(194, '62B', 62),
(195, '86C', 86),
(196, '83B', 83),
(197, '112A', 112),
(198, '107B', 107),
(199, '38A', 38),
(200, '88B', 88),
(201, '53B', 53),
(202, '109B', 109),
(203, '40A', 40),
(204, '95C', 95),
(205, '62A', 62),
(206, '4A', 4),
(207, '102A', 102),
(208, '79B', 79),
(209, '104B', 104),
(210, '77A', 77),
(211, '57B', 57),
(212, '72A', 72),
(213, '82B', 82),
(214, '73A', 73),
(215, '76C', 76),
(216, '9A', 9),
(217, '88A', 88),
(218, '90A', 90),
(219, '111B', 111),
(220, '104A', 104),
(221, '91B', 91),
(222, '83A', 83),
(223, '60B', 60),
(224, '15C', 15),
(225, '86E', 86),
(226, '44A', 44),
(227, '60A', 60),
(228, '106A', 106),
(229, '95A', 95),
(230, '86D', 86),
(231, '74A', 74),
(232, '71A', 71),
(233, '18A', 18),
(234, '39A', 39),
(235, '95B', 95),
(236, '71B', 71),
(237, '70A', 70),
(238, '100B', 100),
(239, '53A', 53),
(240, '58B', 58),
(241, '116B', 116),
(242, '86A', 86),
(243, '23B', 23),
(244, 'Arete', null),
(245, 'Girder', null),
(251, 'any', null),
(252, 'floor', null),
(253, 'balcony', null);