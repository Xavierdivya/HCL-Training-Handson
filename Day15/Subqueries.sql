--Find students older than the average student age.
SELECT * 
FROM Students
WHERE Age > (SELECT AVG(Age) FROM Students);

--Find students younger than the average age.
SELECT * 
FROM Students
WHERE Age < (SELECT AVG(Age) FROM Students);

--Find courses whose fee is greater than the average course fee.
SELECT * 
FROM Courses
WHERE Fee > (SELECT AVG(Fee) FROM Courses);

--Find students whose DepartmentId belongs to Computer Science or Data Science.
SELECT * 
FROM Students
WHERE DepartmentId IN (
    SELECT DepartmentId 
    FROM Departments 
    WHERE DepartmentName IN ('Computer Science', 'Data Science')
);

--Find students who have at least one enrollment using EXISTS.
SELECT s.* 
FROM Students s
WHERE EXISTS (
    SELECT 1 
    FROM Enrollments e 
    WHERE e.StudentId = s.StudentId
);

--Find students who have no enrollment using NOT EXISTS.
SELECT s.* 
FROM Students s
WHERE NOT EXISTS (
    SELECT 1 
    FROM Enrollments e 
    WHERE e.StudentId = s.StudentId
);

--Find courses that have no enrollments using NOT EXISTS.
SELECT c.* 
FROM Courses c
WHERE NOT EXISTS (
    SELECT 1 
    FROM Enrollments e 
    WHERE e.CourseId = c.CourseId
);

--Find students whose age is above the average age in their own department.
SELECT s.* 
FROM Students s
WHERE s.Age > (
    SELECT AVG(s2.Age) 
    FROM Students s2 
    WHERE s2.DepartmentId = s.DepartmentId
);

--Find assessment results with marks greater than the average marks for that assessment.
SELECT r.* 
FROM AssessmentResults r
WHERE r.MarksObtained > (
    SELECT AVG(r2.MarksObtained) 
    FROM AssessmentResults r2 
    WHERE r2.AssessmentId = r.AssessmentId
);

--Find students who have at least one payment above 7000.
SELECT s.* 
FROM Students s
WHERE EXISTS (
    SELECT 1 
    FROM Payments p 
    WHERE p.StudentId = s.StudentId AND p.Amount > 7000
);

--Find students whose total payments are above the average total payment per student among students who have payments.
SELECT StudentId, SUM(Amount) AS TotalPaid
FROM Payments
GROUP BY StudentId
HAVING SUM(Amount) > (
    SELECT AVG(StudentTotal)
    FROM (
        SELECT SUM(Amount) AS StudentTotal 
        FROM Payments 
        GROUP BY StudentId
    ) AS SubTable
);

--Find departments whose student count is greater than the average student count per department among departments with students.
SELECT DepartmentId, COUNT(StudentId) AS StudentCount
FROM Students
WHERE DepartmentId IS NOT NULL
GROUP BY DepartmentId
HAVING COUNT(StudentId) > (
    SELECT AVG(DeptTotal)
    FROM (
        SELECT COUNT(StudentId) AS DeptTotal 
        FROM Students 
        WHERE DepartmentId IS NOT NULL
        GROUP BY DepartmentId
    ) AS SubTable
);

--Find assessments that have at least one pending result.
SELECT * 
FROM Assessments a
WHERE EXISTS (
    SELECT 1 
    FROM AssessmentResults r 
    WHERE r.AssessmentId = a.AssessmentId AND r.SubmissionStatus = 'Pending'
);

--Find students who have submitted at least one result with marks 90 or higher.
SELECT s.* 
FROM Students s
WHERE EXISTS (
    SELECT 1 
    FROM AssessmentResults r 
    WHERE r.StudentId = s.StudentId AND r.MarksObtained >= 90
);

--Find students who have no payment records.
SELECT s.* 
FROM Students s
WHERE NOT EXISTS (
    SELECT 1 
    FROM Payments p 
    WHERE p.StudentId = s.StudentId
);

--Find courses whose fee is above the fee of every inactive course.
SELECT * 
FROM Courses
WHERE Fee > ALL (
    SELECT Fee 
    FROM Courses 
    WHERE IsActive = 0
);

--Find students whose age equals the maximum age.
SELECT * 
FROM Students
WHERE Age = (SELECT MAX(Age) FROM Students);

--Find students who enrolled in the course named SQL Server Fundamentals.
SELECT s.* 
FROM Students s
WHERE s.StudentId IN (
    SELECT e.StudentId 
    FROM Enrollments e
    INNER JOIN Courses c ON e.CourseId = c.CourseId
    WHERE c.CourseName = 'SQL Server Fundamentals'
);

--Find payments whose amount is greater than the average amount for their payment status.
SELECT p.* 
FROM Payments p
WHERE p.Amount > (
    SELECT AVG(p2.Amount) 
    FROM Payments p2 
    WHERE p2.PaymentStatus = p.PaymentStatus
);

--Find students whose most recent payment is later than 2026-01-01.
SELECT s.* 
FROM Students s
WHERE (
    SELECT MAX(p.PaymentDate) 
    FROM Payments p 
    WHERE p.StudentId = s.StudentId
) > '2026-01-01';

--Find students who have results for every assessment? Return students whose result count equals the total assessment count.
SELECT s.StudentId, s.Name
FROM Students s
INNER JOIN AssessmentResults r ON s.StudentId = r.StudentId
GROUP BY s.StudentId, s.Name
HAVING COUNT(DISTINCT r.AssessmentId) = (SELECT COUNT(*) FROM Assessments);

--Find courses with more assessments than the average assessment count per course.
SELECT CourseId, COUNT(AssessmentId) AS TotalAssessments
FROM Assessments
GROUP BY CourseId
HAVING COUNT(AssessmentId) > (
    SELECT AVG(AssessmentCount)
    FROM (
        SELECT COUNT(AssessmentId) AS AssessmentCount 
        FROM Assessments 
        GROUP BY CourseId
    ) AS SubTable
);

--Find students whose average mark is higher than the overall average mark.
SELECT s.StudentId, s.Name, AVG(r.MarksObtained) AS PersonalAverage
FROM Students s
INNER JOIN AssessmentResults r ON s.StudentId = r.StudentId
GROUP BY s.StudentId, s.Name
HAVING AVG(r.MarksObtained) > (SELECT AVG(MarksObtained) FROM AssessmentResults);

--Find departments with at least one student who has a pending payment.
SELECT * 
FROM Departments d
WHERE EXISTS (
    SELECT 1 
    FROM Students s
    INNER JOIN Payments p ON s.StudentId = p.StudentId
    WHERE s.DepartmentId = d.DepartmentId AND p.PaymentStatus = 'Pending'
);

--Find students who have enrolled in all courses in the Programming category.
SELECT s.StudentId, s.Name
FROM Students s
INNER JOIN Enrollments e ON s.StudentId = e.StudentId
INNER JOIN Courses c ON e.CourseId = c.CourseId
WHERE c.Category = 'Programming'
GROUP BY s.StudentId, s.Name
HAVING COUNT(DISTINCT e.CourseId) = (
    SELECT COUNT(*) 
    FROM Courses 
    WHERE Category = 'Programming'
);
