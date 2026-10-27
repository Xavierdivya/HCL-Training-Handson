use LearnerManagementAssessment;

--Count all students.
select Count(*) from students;

--Count students who have an email value.
select Count(email) from Students;

--Count students who have a department assigned.
select Count(*) from students where DepartmentId is not null;

--Find the youngest student age.
select Min(age) from students;

--Find the oldest student age.
select Max(age) from students;

--Calculate the average student age with two decimal places.
select Round(Avg(age),2) from students;

--Find the total number of credits across all courses.
select sum(Credits) from courses;

--Find the average course fee.
select Avg(Fee) from courses;

--Find the lowest and highest course fees.
select Min(Fee) as Min_Fee,Max(Fee) as Max_Fee from courses;

--Count distinct student ages.
select count(Distinct Age) from students;

--Count distinct non-NULL department IDs represented in Students.
select count(Distinct DepartmentId) from students;

--Count all enrollment records.
select count(*) from Enrollments;

--Count completed enrollments.
select Count(*) from Enrollments where EnrollmentStatus='completed';

--Calculate the total amount of all payments.
select Sum(Amount) from payments;

--Calculate the total amount of payments with status Paid.
select Sum(Amount) from payments where PaymentStatus='paid';

--Find the average marks obtained across non-NULL assessment results.
select Avg(marksObtained) from AssessmentResults;

--Find the highest mark obtained.
select Max(MarksObtained) from AssessmentResults;

--Find the lowest non-NULL mark obtained.
select Min(MarksObtained) from AssessmentResults;

--Count assessment results with no marks.
select Count(*) from AssessmentResults where MarksObtained is null;

--Count submitted results.
select count(*) from AssessmentResults;

--Calculate the average marks for submitted results only.
select Avg(MarksObtained) from AssessmentResults ;

--Find the total number of assessments.
select count(*) from Assessments;

--Find the total maximum marks across all assessments.
select Sum(MaxMarks) from Assessments;

--Find the most recent payment date.
select Max(PaymentDate) from payments;

--Find the total number of active courses.
select count(*) from courses where IsActive=1;
