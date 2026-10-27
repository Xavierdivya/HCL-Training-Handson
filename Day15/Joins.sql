use LearnerManagementAssessment;

--List each student with their department name; only students assigned to a department.
 select s.Name ,d.DepartmentName
 from students s 
 Inner join  Departments d 
 on s.DepartmentId=d.DepartmentId;

--List all students and their department name, including unassigned students.
select s.* , d.DepartmentName
from students s
Left Join Departments d
on s.StudentId = d.DepartmentId;

--List all departments and any students in them, including departments with no students.
select d.DepartmentID,s.Name
from Departments d
Left Join Students s
on d.DepartmentId = s.DepartmentId;

--Find departments with no students.
select d.*,s.*
from students s
Left Join Departments d
on d.DepartmentId = s.DepartmentId;

--List each enrollment with student name and course name.
SELECT 
    e.EnrollmentID,
    s.Name AS StudentName,
    c.CourseName
FROM Enrollments e
INNER JOIN Students s
    ON e.StudentID = s.StudentID
INNER JOIN Courses c
    ON e.CourseID = c.CourseID;

--List all courses and enrollment counts, including courses with zero enrollments.
SELECT 
    c.CourseName,
    Count(EnrollmentId) as EnrllmentCount
FROM Courses c
LEFT JOIN Enrollments e
    ON c.CourseId=e.CourseId 
    group by
        c.CourseId, 
        c.CourseName;

--Find courses with no enrollments.
SELECT 
    c.CourseName,
    Count(e.EnrollmentId) as EnrllmentCount
FROM Courses c
LEFT JOIN Enrollments e
    ON c.CourseId=e.CourseId 
    group by
        c.CourseId, 
        c.CourseName
     HAVING
        Count(e.EnrollmentId)=0;

--List each student with assessment name and marks, where a result exists.
SELECT 
    s.Name as StudentName,
    a.AssessmentName,
    r.MarksObtained
from Students s
INNER JOIN AssessmentResults r
on s.StudentId = r.StudentId
INNER JOIN Assessments a
on a.AssessmentId = r.AssessmentId;

--List all students and any assessment results, including students with no result.
SELECT 
    s.Name as StudentName,
    a.AssessmentName,
    r.MarksObtained
from Students s LEFT JOIN AssessmentResults r
on s.StudentId = r.StudentId LEFT JOIN Assessments a
on a.AssessmentId = r.AssessmentId;

--List assessment names with course names.
SELECT a.AssessmentName,
       c.CourseName
from Assessments a
INNER JOIN Courses c
ON a.CourseId=c.CourseId;

--List all courses and their assessments, including courses without assessments.
SELECT c.CourseName,
       a.AssessmentName
from Courses c 
LEFT JOIN Assessments a
ON a.CourseId=c.CourseId;

--List each payment with student name and department name if available.
select p.*,
       s.Name as StudentName,
       d.DepartmentName
from payments p 
LEFT JOIN Students s
ON p.StudentId = s.StudentId
LEFT JOIN Departments d
ON d.DepartmentId = s.DepartmentId;

    
--Find students who have at least one payment with status Pending.
SELECT DISTINCT 
    s.Name AS StudentName
FROM Students s
INNER JOIN Payments p ON s.StudentId = p.StudentId
WHERE p.PaymentStatus = 'Pending';

--Find students who have never made a payment.
SELECT 
    s.StudentId,
    s.Name AS StudentName
FROM Students s
WHERE NOT EXISTS (
    SELECT 1 
    FROM Payments p 
    WHERE p.StudentId = s.StudentId
);

--Show student name, department, course, and enrollment status.
SELECT 
    s.Name AS StudentName,
    d.DepartmentName,
    c.CourseName,
    e.EnrollmentStatus
FROM Students s
INNER JOIN Departments d ON s.DepartmentId = d.DepartmentId
INNER JOIN Enrollments e ON s.StudentId = e.StudentId
INNER JOIN Courses c ON e.CourseId = c.CourseId;

--Count students per department, including departments with zero students.
SELECT 
    d.DepartmentName,
    COUNT(s.StudentId) AS StudentCount
FROM Departments d
LEFT JOIN Students s ON d.DepartmentId = s.DepartmentId
GROUP BY 
    d.DepartmentId, 
    d.DepartmentName;

--Find assessment results where the student's department is Computer Science.
SELECT 
    s.Name AS StudentName,
    a.AssessmentName,
    r.MarksObtained
FROM AssessmentResults r
INNER JOIN Students s ON r.StudentId = s.StudentId
INNER JOIN Departments d ON s.DepartmentId = d.DepartmentId
INNER JOIN Assessments a ON r.AssessmentId = a.AssessmentId
WHERE d.DepartmentName = 'Computer Science';

--List payments made by students from Data Science.
SELECT 
    p.*,
    s.Name AS StudentName
FROM Payments p
INNER JOIN Students s ON p.StudentId = s.StudentId
INNER JOIN Departments d ON s.DepartmentId = d.DepartmentId
WHERE d.DepartmentName = 'Data Science';

--List each course with its assessment count.
SELECT 
    c.CourseName,
    COUNT(a.AssessmentId) AS AssessmentCount
FROM Courses c
LEFT JOIN Assessments a ON c.CourseId = a.CourseId
GROUP BY 
    c.CourseId, 
    c.CourseName;

--List each student and their number of assessment results.
SELECT 
    s.Name AS StudentName,
    COUNT(r.ResultId) AS ResultCount
FROM Students s
LEFT JOIN AssessmentResults r ON s.StudentId = r.StudentId
GROUP BY 
    s.StudentId, 
    s.Name;

--Find students enrolled in C# Programming.
SELECT 
    s.Name AS StudentName,
    e.EnrollmentStatus
FROM Students s
INNER JOIN Enrollments e ON s.StudentId = e.StudentId
INNER JOIN Courses c ON e.CourseId = c.CourseId
WHERE c.CourseName = 'C# Programming';

--Show students who have results for assessments dated in 2026.
SELECT DISTINCT 
    s.Name AS StudentName
FROM Students s
INNER JOIN AssessmentResults r ON s.StudentId = r.StudentId
INNER JOIN Assessments a ON r.AssessmentId = a.AssessmentId
WHERE a.AssessmentDate >= '2026-01-01' AND a.AssessmentDate <= '2026-12-31';

--List all departments and students using RIGHT JOIN.
SELECT 
    d.DepartmentName,
    s.Name AS StudentName
FROM Students s
RIGHT JOIN Departments d ON s.DepartmentId = d.DepartmentId;

--List all courses and enrollments using FULL OUTER JOIN.
SELECT 
    c.CourseName,
    e.*
FROM Courses c
FULL OUTER JOIN Enrollments e ON c.CourseId=e.CourseId;

--Find each student's latest payment date
SELECT 
    s.Name as StudentName,
    MAX(p.PaymentDate) LatesetPayment
FROM Students s
INNER JOIN Payments p ON s.StudentId=p.StudentId
GROUP BY s.StudentId,
         s.Name;
