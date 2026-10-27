use LearnerManagementAssessment;

--Count students in each department ID, including students with NULL DepartmentId as a group.
select DepartmentId,Count(*) as count from students group by DepartmentId;

--Find the average age for each department ID.
select DepartmentId,Avg(age) as Average_Age from students group by DepartmentId;

--Find the youngest and oldest student age per department.
Select DepartmentId,Min(age) as Min_Age,Max(age) as Max_Age from Students group by DepartmentId;

--Show department IDs that have more than 10 students.
select DepartmentID from students group by DepartmentId having Count(*)>10;

--Count students by active status.
select Count(*) as Students from students group by IsActive; 

--Count enrollments by EnrollmentStatus.
select EnrollmentStatus,Count(*) as Count from Enrollments group by EnrollmentStatus ;

--Find courses with more than 15 enrollments.
select CourseId from enrollments group by CourseId having Count(*)>15;

--Find the average course fee per category.
select Category,Avg(Fee) as Avg_Fee from Courses group by Category; 

--Count courses per category.
select Category,Count(*) as Count_of_Courses from courses group by Category;

--Show payment count and total amount by payment status.
select PaymentStatus,Count(*) as Count_of_Payment,Sum(Amount) as Total_Amount from payments group by PaymentStatus;

--Show payment count and average amount by payment method.
select PaymentMethod,Count(*) as Count_of_Payment,Avg(Amount) as Avg_Amount from payments group by PaymentMethod;

--Find payment statuses with at least 20 payments.
select PaymentStatus from payments group by PaymentStatus having Count(*)>=20;

--Find students with more than 3 enrollments.
select StudentId from enrollments group by StudentId having Count(*) > 3;

--Find students with at least 2 assessment results.
select StudentId from AssessmentResults group by StudentId having Count(*) >= 2;

--Find average marks per assessment.
select AssessmentId, Avg(MarksObtained) as Average_Marks from AssessmentResults group by AssessmentId;

--Find assessments whose average marks are above 60.
select AssessmentId from AssessmentResults group by AssessmentId having Avg(MarksObtained) > 60;

--Count result records by SubmissionStatus.
select SubmissionStatus, Count(*) as Record_Count from Assessmentresults group by SubmissionStatus;

--Find the total fee per course category for active courses.
select Category, Sum(Fee) as Total_Fee from courses where IsActive = 1 group by Category;

--Find departments with average student age above 22.
select DepartmentId from students group by DepartmentId having Avg(age) > 22;

--Count students by DepartmentId and IsActive.
select DepartmentId, IsActive, Count(*) as count from students group by DepartmentId, IsActive;

--Find courses with at least 2 assessments.
select CourseId from assessments group by CourseId having Count(*) >= 2;

--Find each student's total payment amount.
select StudentId, Sum(Amount) as Total_Payment from payments group by StudentId;

--Find students whose total payments exceed 12000.
select StudentId from payments group by StudentId having Sum(Amount) > 12000;

--Count results with non-NULL marks per student.
select StudentId, Count(MarksObtained) as Marks_Count from AssessmentResults group by StudentId;

--Find payment months with more than 10 payments.
select Month(PaymentDate) as Payment_Month from payments group by Month(PaymentDate) having Count(*) > 10;
