--
--ER/Studio 6.0 SQL Code Generation
-- Company :      Kgb
-- Project :      mike.dm1
-- Author :       Mike
--
-- Date Created : Tuesday, May 30, 2006 22:22:40
-- Target DBMS : MySQL 4.x
--


-- 
-- TABLE: tblClassRooms 
--

CREATE TABLE tblClassRooms(
    ID       DECIMAL(10, 0)    NOT NULL,
    sName    CHAR(128),
    PRIMARY KEY (ID)
)TYPE=MYISAM
;



-- 
-- TABLE: tblClassRoomToProffesor 
--

CREATE TABLE tblClassRoomToProffesor(
    ID               DECIMAL(10, 0)    NOT NULL,
    iID_Proffesor    DECIMAL(10, 0)    NOT NULL,
    iID_ClassRoom    DECIMAL(10, 0)    NOT NULL,
    PRIMARY KEY (ID)
)TYPE=MYISAM
;



-- 
-- TABLE: tblExams 
--

CREATE TABLE tblExams(
    ID            DECIMAL(10, 0)    NOT NULL,
    iMinGrade     DECIMAL(2, 0)     NOT NULL,
    iID_Object    DECIMAL(10, 0),
    sName         CHAR(128)         NOT NULL,
    dDate         DATE,
    PRIMARY KEY (ID)
)
;



-- 
-- TABLE: tblGrades 
--

CREATE TABLE tblGrades(
    ID            DECIMAL(10, 0)    NOT NULL,
    iID_Object    DECIMAL(10, 0),
    dDate         DATE,
    iGradeType    DECIMAL(1, 0),
    iGrade        DECIMAL(2, 0),
    PRIMARY KEY (ID)
)
;



-- 
-- TABLE: tblGradeToStudent 
--

CREATE TABLE tblGradeToStudent(
    ID             DECIMAL(10, 0)    NOT NULL,
    iID_Student    DECIMAL(10, 0)    NOT NULL,
    iID_Grade      DECIMAL(10, 0)    NOT NULL,
    PRIMARY KEY (ID)
)
;



-- 
-- TABLE: tblObjects 
--

CREATE TABLE tblObjects(
    ID        DECIMAL(10, 0)    NOT NULL,
    sName     CHAR(128)         NOT NULL,
    iHours    DECIMAL(3, 0)     NOT NULL,
    PRIMARY KEY (ID)
)
;



-- 
-- TABLE: tblProffesors 
--

CREATE TABLE tblProffesors(
    ID             DECIMAL(10, 0)    NOT NULL,
    iID_Object     DECIMAL(10, 0)    NOT NULL,
    sFirstName     CHAR(128)         NOT NULL,
    sLastName      CHAR(128)         NOT NULL,
    iProffGrade    DECIMAL(1, 0)     NOT NULL,
    PRIMARY KEY (ID)
)TYPE=MYISAM
;



-- 
-- TABLE: tblStudents 
--

CREATE TABLE tblStudents(
    ID               DECIMAL(10, 0)    NOT NULL,
    iID_ClassRoom    CHAR(10)          NOT NULL,
    sFirstName       CHAR(12),
    sLastName        CHAR(10),
    dBirthDate       DATE              NOT NULL,
    PRIMARY KEY (ID)
)TYPE=MYISAM
;



-- 
-- TABLE: tblTests 
--

CREATE TABLE tblTests(
    ID            DECIMAL(10, 0)    NOT NULL,
    iID_Object    DECIMAL(10, 0),
    dDate         DATE,
    sName         CHAR(128)         NOT NULL,
    PRIMARY KEY (ID)
)TYPE=MYISAM
;




