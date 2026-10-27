--Return page 1 of students with 10 students per page, ordered by StudentId.
DECLARE @PageNumber int = 1;
DECLARE @PageSize int = 10;

select * from students 
order by StudentId
OFFSET (@PageNumber - 1)*@PageSize rows
FETCH NEXT @PageSize rows only;

--Return page 2 of students with 10 students per page.
select * from students
order by StudentId
OFFSET 10 rows
FETCH NEXT 10 rows only;

--Return page 5 of students with 10 students per page.
select * from students 
order by StudentId
OFFSET 40 rows
FETCH NEXT 10 rows only;

--Return page 3 of courses with 4 courses per page, ordered by CourseName.
select * from Courses
order by CourseName
OFFSET 8 rows
FETCH NEXT 4 rows only;

--Return the first 15 payments by PaymentDate, newest first.
select * from Payments
order by PaymentDate DESC
OFFSET 0 rows
FETCH next 15 rows only;

--Return page 2 of payments with 20 rows per page, newest first.
select * from Payments
order by PaymentDate DESC
OFFSET 20 rows
FETCH NEXT 20 rows only;

--Return page 4 of assessment results with 25 rows per page.
select * from AssessmentResults
order by AssessmentId
OFFSET 75 rows
FETCH NEXT 25 rows only;

--Return page 2 of enrollments with 12 rows per page, ordered by date.
select * from Enrollments
order by EnrolledAt
OFFSET 12 rows
FETCH NEXT 12 rows only;

--Return page 1 of students aged 20 or older, 8 rows per page.
SELECT * 
FROM Students
WHERE Age >= 20
ORDER BY StudentId 
OFFSET 0 ROWS
FETCH NEXT 8 ROWS ONLY;

--Return page 2 of active students, 8 rows per page.
SELECT * 
FROM Students
WHERE IsActive = 1
ORDER BY StudentId
OFFSET 8 ROWS
FETCH NEXT 8 ROWS ONLY;

--Return page 1 of courses whose fee is at least 4000, 5 rows per page.
SELECT * 
FROM Courses
WHERE Fee >= 4000
ORDER BY CourseId
OFFSET 0 ROWS
FETCH NEXT 5 ROWS ONLY;

--Return page 2 of assessments ordered by AssessmentDate ascending, 6 per page.
SELECT * 
FROM Assessments
ORDER BY AssessmentDate ASC
OFFSET 6 ROWS
FETCH NEXT 6 ROWS ONLY;

--Return page 3 of students sorted alphabetically by name, 10 per page.
SELECT * 
FROM Students
ORDER BY Name ASC
OFFSET 20 ROWS
FETCH NEXT 10 ROWS ONLY;

--Return page 1 of payments whose status is Pending, 10 per page.
SELECT * 
FROM Payments
WHERE PaymentStatus = 'Pending'
ORDER BY PaymentId
OFFSET 0 ROWS
FETCH NEXT 10 ROWS ONLY;

--Return page 2 of results with submitted status, 15 per page.
SELECT * 
FROM AssessmentResults
WHERE SubmissionStatus = 'Submitted'
ORDER BY ResultId
OFFSET 15 ROWS
FETCH NEXT 15 ROWS ONLY;

--Return page 1 of enrollments for CourseId 3, 10 per page.
SELECT * 
FROM Enrollments
WHERE CourseId = 3
ORDER BY EnrollmentId
OFFSET 0 ROWS
FETCH NEXT 10 ROWS ONLY;

--Return page 2 of students whose names start with A, 7 per page.
SELECT * 
FROM Students
WHERE Name LIKE 'A%'
ORDER BY Name ASC
OFFSET 7 ROWS
FETCH NEXT 7 ROWS ONLY;

--Return page 1 of assessments with maximum marks 50, 5 per page.
SELECT * 
FROM Assessments
WHERE MaxMarks = 50
ORDER BY AssessmentId
OFFSET 0 ROWS
FETCH NEXT 5 ROWS ONLY;

--Return page 3 of students who have a department assigned, 9 per page.
SELECT * 
FROM Students
WHERE DepartmentId IS NOT NULL
ORDER BY StudentId
OFFSET 18 ROWS
FETCH NEXT 9 ROWS ONLY;

--Return page 1 of paid payments with amount over 5000, 10 per page.
SELECT * 
FROM Payments
WHERE PaymentStatus = 'Paid' AND Amount > 5000
ORDER BY PaymentId
OFFSET 0 ROWS
FETCH NEXT 10 ROWS ONLY;

--Return page 2 of course names containing 'Data', 3 per page.
SELECT CourseName 
FROM Courses
WHERE CourseName LIKE '%Data%'
ORDER BY CourseName ASC
OFFSET 3 ROWS
FETCH NEXT 3 ROWS ONLY;

--Return page 1 of results with marks 80 or higher, 20 per page.
SELECT * 
FROM AssessmentResults
WHERE MarksObtained >= 80
ORDER BY MarksObtained DESC
OFFSET 0 ROWS
FETCH NEXT 20 ROWS ONLY;

--Return page 1 of payments from calendar year 2025, 10 per page.
SELECT * 
FROM Payments
WHERE PaymentDate >= '2025-01-01' AND PaymentDate <= '2025-12-31'
ORDER BY PaymentDate DESC
OFFSET 0 ROWS
FETCH NEXT 10 ROWS ONLY;

--Write a parameterized stored procedure that returns a page of students.
GO
CREATE PROCEDURE GetPagedStudents
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    -- Safeguard to handle invalid parameter overrides
    IF @PageNumber < 1 SET @PageNumber = 1;
    IF @PageSize < 1 SET @PageSize = 10;

    SELECT * 
    FROM Students
    ORDER BY StudentId
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

EXEC GetPagedStudents @PageNumber = 5, @PageSize = 20;
--Calculate the number of pages required for all students when page size is 12.
SELECT 
    CEILING(COUNT(*) / 12.0) AS TotalPagesRequired
FROM Students;