use  LearnerManagementAssessment
--Display the StudentId, Name, Email, and Age of all students.
select StudentId, Name, Email, Age from students;

--Find students who are exactly 21 years old.
select * from students where Age = 21;

--Find students younger than 20.
select * from students where Age<20;

--Find active students aged 20 or above.
select * from students where Age>=20 and IsActive=1;

--Find students aged between 18 and 25 inclusive.
select * from Students where age between 18 and 25;

--Find students belonging to departments 1, 3, or 6.
select * from students where DepartmentId=1 or DepartmentId=3 or DepartmentId=6;
select * from students where DepartmentId in(1,3,6);

--Find students who have not been assigned a department.
select * from students where DepartmentId  is null;

--Find students who have been assigned a department.
select * from students where DepartmentId is Not null;

--Find names that begin with A.
select Name from students where Name like 'A%'; 

--Find names that end with the digit 5.
select Name from students where Name like '%5';

--Find names containing the text 'ar' (use case-insensitive matching where supported by the database collation).
select Name from students where Name like '%ar%';

--Find names whose second character is 'a'.
select Name from students where Name like '_a%';

--Find names that contain exactly one character before ' ' and then any text.
select Name from Students where Name like '_ _%'; 

--Find emails ending in example.com.
select email from Students where Email like '%example.com';

--Find students whose age is not 20.
select Name,Age from Students where age <> 20;

--Find students aged 18–22 who are active.
select Name,Age,IsActive from students where age between 18 and 22 and IsActive = 1;

--Find students who are inactive or are not assigned to a department.
select name,IsActive,departmentId from students where IsActive = 0 or DepartmentId is null; 

--Sort students from youngest to oldest.
SELECT Name, Age FROM Students ORDER BY Age ASC;

--Display the 10 oldest students.
select Top 10 Name,Age from students ORDER BY Age DESC; 

--Sort students by DepartmentId ascending, then Age descending.
select Name,Age,DepartmentId from Students order by DepartmentId asc ,age desc;

--Display each distinct age value.
select DISTINCT Age from students;

--Display distinct non-NULL department IDs.
select DISTINCT departmentId from students where DepartmentId is not null;

--Find names containing a digit using a LIKE pattern.
select Name from students where name like '%[0-9]%';

--Find names containing any uppercase letter from A through M at the beginning.
select Name from students where name like '[A-M]%';

--Find students admitted during calendar year 2025.
select Name,AdmissionDate from students where AdmissionDate >= '2025-01-01' and AdmissionDate <'2026-01-01';
