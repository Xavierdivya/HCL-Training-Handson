
using Microsoft.Data.SqlClient;
using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;
using StudentCourseManagement.Services;

// Initialize repositories
DepartmentRepository departmentRepository = new();
StudentRepository studentRepository = new();
CourseRepository courseRepository = new();
ReportRepository reportRepository = new();
AuditLogRepository auditLogRepository = new();

// Initialize services
DepartmentService departmentService = new(departmentRepository);
StudentService studentService = new(studentRepository);
CourseService courseService = new(courseRepository);
EnrollmentRepository enrollmentRepository = new();
EnrollmentService enrollmentService = new(enrollmentRepository);
AuditLogService auditLogService = new(auditLogRepository);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("====================================");
    Console.WriteLine("   STUDENT COURSE MANAGEMENT SYSTEM");
    Console.WriteLine("====================================");
    Console.WriteLine("1.  Add Department");
    Console.WriteLine("2.  View All Departments");
    Console.WriteLine("3.  Update Department");
    Console.WriteLine("4.  Delete Department");
    Console.WriteLine("------------------------------------");
    Console.WriteLine("5.  Add Student");
    Console.WriteLine("6.  View All Students");
    Console.WriteLine("7.  Update Student");
    Console.WriteLine("8.  Delete Student");
    Console.WriteLine("------------------------------------");
    Console.WriteLine("9.  Add Course");
    Console.WriteLine("10. View All Courses");
    Console.WriteLine("11. Update Course");
    Console.WriteLine("12. Delete Course");
    Console.WriteLine("------------------------------------");
    Console.WriteLine("13. Add Enrollment");
    Console.WriteLine("14. View All Enrollments");
    Console.WriteLine("15. Update Enrollment Status");
    Console.WriteLine("16. Delete Enrollment");
    Console.WriteLine("------------------------------------");
    Console.WriteLine("17. View Enrollment Report");
    Console.WriteLine("18. View Audit Logs");
    Console.WriteLine("19. Search Students by Name");
    Console.WriteLine("20. Search Courses by Name");
    Console.WriteLine("21. Search Departments by Name");
    Console.WriteLine("22. View Courses by Student ID");
    Console.WriteLine("23. View Students by Course ID");
    Console.WriteLine("24. View Enrollment Count by Course");
    Console.WriteLine("25. View Student Enrollment Report");
    Console.WriteLine("0.  Exit");
    Console.WriteLine("====================================");
    Console.Write("Choose an option: ");

    string choice = Console.ReadLine() ?? "";

    if (choice == "0")
    {
        Console.WriteLine("Exiting application. Goodbye!");
        break;
    }

    try
    {
        switch (choice)
        {
            // =========================================
            // DEPARTMENT OPERATIONS
            // =========================================


            case "1":
                {
                    Department department = new();

                    Console.Write("Enter department name: ");
                    department.DepartmentName = Console.ReadLine() ?? "";

                    Console.Write("Enter department location: ");
                    department.Location = Console.ReadLine();

                    int newDepartmentId =
                        await departmentService.AddAsync(department);

                    await auditLogService.AddAsync(new AuditLog
                    {
                        EntityName = "Department",
                        EntityId = newDepartmentId,
                        Action = "Created",
                        Details = "Department created successfully"
                    });

                    Console.WriteLine(
                        $"Department added successfully. ID: {newDepartmentId}");

                    break;
                }


            case "2":
                {
                    List<Department> departments =
                        await departmentService.GetAllAsync();

                    if (departments.Count == 0)
                    {
                        Console.WriteLine("No departments found.");
                        break;
                    }

                    foreach (Department department in departments)
                    {
                        Console.WriteLine(
                            $"ID: {department.DepartmentId} | " +
                            $"Name: {department.DepartmentName} | " +
                            $"Location: {department.Location ?? "N/A"} | " +
                            $"Active: {department.IsActive}");
                    }

                    break;
                }

            case "3":
                {
                    Department department = new();

                    Console.Write("Enter department ID to update: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int updateDepartmentId))
                    {
                        Console.WriteLine("Invalid department ID.");
                        break;
                    }

                    department.DepartmentId = updateDepartmentId;

                    Console.Write("Enter new department name: ");
                    department.DepartmentName = Console.ReadLine() ?? "";

                    Console.Write("Enter new department location: ");
                    department.Location = Console.ReadLine();

                    bool departmentUpdated =
                        await departmentService.UpdateAsync(department);

                    Console.WriteLine(departmentUpdated
                        ? "Department updated successfully."
                        : "Department not found.");

                    break;
                }

            case "4":
                {
                    Console.Write("Enter department ID to delete: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int deleteDepartmentId))
                    {
                        Console.WriteLine("Invalid department ID.");
                        break;
                    }

                    bool departmentDeleted =
                        await departmentService.DeleteAsync(deleteDepartmentId);

                    Console.WriteLine(departmentDeleted
                        ? "Department deleted successfully."
                        : "Department not found.");

                    break;
                }

            // =========================================
            // STUDENT OPERATIONS
            // =========================================

            case "5":
                {
                    Student student = new();

                    Console.Write("Enter student name: ");
                    student.Name = Console.ReadLine() ?? "";

                    Console.Write("Enter student email: ");
                    student.Email = Console.ReadLine() ?? "";

                    Console.Write("Enter student age: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int studentAge))
                    {
                        Console.WriteLine("Invalid age.");
                        break;
                    }

                    student.Age = studentAge;

                    Console.Write("Enter department ID: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int studentDepartmentId))
                    {
                        Console.WriteLine("Invalid department ID.");
                        break;
                    }

                    student.DepartmentId = studentDepartmentId;

                    int newStudentId =
                        await studentService.AddAsync(student);
                    
                    await auditLogService.AddAsync(new AuditLog
                    {
                        EntityName = "Student",
                        EntityId = newStudentId,
                        Action = "Created",
                        Details = $"Student '{student.Name}' was added."
                    });

                    Console.WriteLine(
                        $"Student added successfully. ID: {newStudentId}");

                    break;
                }

            case "6":
                {
                    List<Student> students =
                        await studentService.GetAllAsync();

                    if (students.Count == 0)
                    {
                        Console.WriteLine("No students found.");
                        break;
                    }

                    foreach (Student student in students)
                    {
                        Console.WriteLine(
                            $"ID: {student.StudentId} | " +
                            $"Name: {student.Name} | " +
                            $"Email: {student.Email} | " +
                            $"Age: {student.Age} | " +
                            $"Department ID: {student.DepartmentId} | " +
                            $"Admission Date: {student.AdmissionDate:g} | " +
                            $"Active: {student.IsActive}");
                    }

                    break;
                }

            case "7":
                {
                    Student student = new();

                    Console.Write("Enter student ID to update: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int updateStudentId))
                    {
                        Console.WriteLine("Invalid student ID.");
                        break;
                    }

                    student.StudentId = updateStudentId;

                    Console.Write("Enter new student name: ");
                    student.Name = Console.ReadLine() ?? "";

                    Console.Write("Enter new student email: ");
                    student.Email = Console.ReadLine() ?? "";

                    Console.Write("Enter new student age: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int updatedStudentAge))
                    {
                        Console.WriteLine("Invalid age.");
                        break;
                    }

                    student.Age = updatedStudentAge;

                    Console.Write("Enter new department ID: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int updatedStudentDepartmentId))
                    {
                        Console.WriteLine("Invalid department ID.");
                        break;
                    }

                    student.DepartmentId = updatedStudentDepartmentId;

                    bool studentUpdated =
                        await studentService.UpdateAsync(student);

                    if (studentUpdated)
                    {
                        await auditLogService.AddAsync(new AuditLog
                        {
                            EntityName = "Student",
                            EntityId = student.StudentId,
                            Action = "Updated",
                            Details = $"Student '{student.Name}' was updated."
                        });
                    }

                    Console.WriteLine(studentUpdated
                        ? "Student updated successfully."
                        : "Student not found.");

                    break;
                }

            case "8":
                {
                    Console.Write("Enter student ID to delete: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int deleteStudentId))
                    {
                        Console.WriteLine("Invalid student ID.");
                        break;
                    }

                    bool studentDeleted =
                        await studentService.DeleteAsync(deleteStudentId);

                    if (studentDeleted)
                    {
                        await auditLogService.AddAsync(new AuditLog
                        {
                            EntityName = "Student",
                            EntityId = deleteStudentId,
                            Action = "Deleted",
                            Details = $"Student ID {deleteStudentId} was deleted."
                        });
                    }

                    Console.WriteLine(studentDeleted
                        ? "Student deleted successfully."
                        : "Student not found.");

                    break;
                }

            // =========================================
            // COURSE OPERATIONS
            // =========================================

            case "9":
                {
                    Course course = new();

                    Console.Write("Enter course name: ");
                    course.CourseName = Console.ReadLine() ?? "";

                    Console.Write("Enter duration in months: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int courseDuration))
                    {
                        Console.WriteLine("Invalid duration.");
                        break;
                    }

                    course.DurationMonths = courseDuration;

                    Console.Write("Enter course fee: ");

                    if (!decimal.TryParse(
                        Console.ReadLine(), out decimal courseFee))
                    {
                        Console.WriteLine("Invalid fee.");
                        break;
                    }

                    course.Fee = courseFee;

                    Console.Write("Enter department ID: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int courseDepartmentId))
                    {
                        Console.WriteLine("Invalid department ID.");
                        break;
                    }

                    course.DepartmentId = courseDepartmentId;

                    int newCourseId =
                        await courseService.AddAsync(course);

                    await auditLogService.AddAsync(new AuditLog
                    {
                        EntityName = "Course",
                        EntityId = newCourseId,
                        Action = "Created",
                        Details = $"Course '{course.CourseName}' was created."
                    });

                    Console.WriteLine(
                        $"Course added successfully. ID: {newCourseId}");

                    break;
                }

            case "10":
                {
                    List<Course> courses =
                        await courseService.GetAllAsync();

                    if (courses.Count == 0)
                    {
                        Console.WriteLine("No courses found.");
                        break;
                    }

                    foreach (Course course in courses)
                    {
                        Console.WriteLine(
                            $"ID: {course.CourseId} | " +
                            $"Name: {course.CourseName} | " +
                            $"Duration: {course.DurationMonths} months | " +
                            $"Fee: {course.Fee:F2} | " +
                            $"Department ID: {course.DepartmentId} | " +
                            $"Active: {course.IsActive}");
                    }

                    break;
                }

            case "11":
                {
                    Course course = new();

                    Console.Write("Enter course ID to update: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int updateCourseId))
                    {
                        Console.WriteLine("Invalid course ID.");
                        break;
                    }

                    course.CourseId = updateCourseId;

                    Console.Write("Enter new course name: ");
                    course.CourseName = Console.ReadLine() ?? "";

                    Console.Write("Enter new duration in months: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int updatedCourseDuration))
                    {
                        Console.WriteLine("Invalid duration.");
                        break;
                    }

                    course.DurationMonths = updatedCourseDuration;

                    Console.Write("Enter new course fee: ");

                    if (!decimal.TryParse(
                        Console.ReadLine(), out decimal updatedCourseFee))
                    {
                        Console.WriteLine("Invalid fee.");
                        break;
                    }

                    course.Fee = updatedCourseFee;

                    Console.Write("Enter new department ID: ");

                    if (!int.TryParse(
                        Console.ReadLine(),
                        out int updatedCourseDepartmentId))
                    {
                        Console.WriteLine("Invalid department ID.");
                        break;
                    }

                    course.DepartmentId = updatedCourseDepartmentId;

                    bool courseUpdated =
                        await courseService.UpdateAsync(course);

                    if (courseUpdated)
                    {
                        await auditLogService.AddAsync(new AuditLog
                        {
                            EntityName = "Course",
                            EntityId = course.CourseId,
                            Action = "Updated",
                            Details = $"Course '{course.CourseName}' was updated."
                        });
                    }

                    Console.WriteLine(courseUpdated
                        ? "Course updated successfully."
                        : "Course not found.");

                    break;
                }

            case "12":
                {
                    Console.Write("Enter course ID to delete: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int deleteCourseId))
                    {
                        Console.WriteLine("Invalid course ID.");
                        break;
                    }

                    bool courseDeleted =
                        await courseService.DeleteAsync(deleteCourseId);

                    if (courseDeleted)
                    {
                        await auditLogService.AddAsync(new AuditLog
                        {
                            EntityName = "Course",
                            EntityId = deleteCourseId,
                            Action = "Deleted",
                            Details = $"Course ID {deleteCourseId} was deleted."
                        });
                    }

                    Console.WriteLine(courseDeleted
                        ? "Course deleted successfully."
                        : "Course not found.");

                    break;
                }



            case "13":
                {
                    Enrollment enrollment = new();

                    Console.Write("Enter student ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int enrollmentStudentId))
                    {
                        Console.WriteLine("Invalid student ID.");
                        break;
                    }

                    enrollment.StudentId = enrollmentStudentId;

                    Console.Write("Enter course ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int enrollmentCourseId))
                    {
                        Console.WriteLine("Invalid course ID.");
                        break;
                    }

                    enrollment.CourseId = enrollmentCourseId;

                    try
                    {
                        int newEnrollmentId =
                            await enrollmentService.AddAsync(enrollment);

                        await auditLogService.AddAsync(new AuditLog
                        {
                            EntityName = "Enrollment",
                            EntityId = newEnrollmentId,
                            Action = "Created",
                            Details =
                                $"Student ID {enrollment.StudentId} enrolled " +
                                $"in Course ID {enrollment.CourseId}."
                        });

                        Console.WriteLine(
                            $"Enrollment added successfully. ID: {newEnrollmentId}");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"Enrollment failed: {ex.Message}");
                    }
                    catch (SqlException)
                    {
                        Console.WriteLine(
                            "Enrollment failed because of a database error.");
                    }

                    break;
                }

            case "14":
                {
                    List<Enrollment> enrollments =
                        await enrollmentService.GetAllAsync();

                    if (enrollments.Count == 0)
                    {
                        Console.WriteLine("No enrollments found.");
                        break;
                    }

                    foreach (Enrollment enrollment in enrollments)
                    {
                        Console.WriteLine(
                            $"Enrollment ID: {enrollment.EnrollmentId} | " +
                            $"Student ID: {enrollment.StudentId} | " +
                            $"Course ID: {enrollment.CourseId} | " +
                            $"Enrolled At: {enrollment.EnrolledAt:g} | " +
                            $"Status: {enrollment.Status}");
                    }
                    break;
                }

            case "15":
                {
                    Console.Write("Enter enrollment ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int statusEnrollmentId))
                    {
                        Console.WriteLine("Invalid enrollment ID.");
                        break;
                    }

                    Console.Write("Enter status (Enrolled/Completed/Cancelled): ");
                    string enrollmentStatus = Console.ReadLine() ?? "";

                    bool statusUpdated = await enrollmentService.UpdateStatusAsync(
                        statusEnrollmentId, enrollmentStatus);

                    if (statusUpdated)
                    {
                        await auditLogService.AddAsync(new AuditLog
                        {
                            EntityName = "Enrollment",
                            EntityId = statusEnrollmentId,
                            Action = "Updated",
                            Details = $"Enrollment status changed to {enrollmentStatus}."
                        });
                    }

                    Console.WriteLine(statusUpdated
                        ? "Enrollment status updated successfully."
                        : "Enrollment not found.");
                    break;
                }

            case "16":
                {
                    Console.Write("Enter enrollment ID to delete: ");
                    if (!int.TryParse(Console.ReadLine(), out int deleteEnrollmentId))
                    {
                        Console.WriteLine("Invalid enrollment ID.");
                        break;
                    }

                    bool enrollmentDeleted =
                        await enrollmentService.DeleteAsync(deleteEnrollmentId);

                    if (enrollmentDeleted)
                    {
                        await auditLogService.AddAsync(new AuditLog
                        {
                            EntityName = "Enrollment",
                            EntityId = deleteEnrollmentId,
                            Action = "Deleted",
                            Details = $"Enrollment ID {deleteEnrollmentId} was deleted."
                        });
                    }

                    Console.WriteLine(enrollmentDeleted
                        ? "Enrollment deleted successfully."
                        : "Enrollment not found.");
                    break;
                }

            case "17":
                {
                    List<string> enrollmentReport =
                        await reportRepository.GetEnrollmentReportAsync();

                    if (enrollmentReport.Count == 0)
                    {
                        Console.WriteLine("No enrollment records found.");
                        break;
                    }

                    Console.WriteLine();
                    Console.WriteLine("========== ENROLLMENT REPORT ==========");

                    foreach (string reportLine in enrollmentReport)
                    {
                        Console.WriteLine(reportLine);
                    }

                    Console.WriteLine("=======================================");
                    break;
                }


            case "18":
                {
                    List<AuditLog> auditLogs =
                        await auditLogService.GetAllAsync();

                    if (auditLogs.Count == 0)
                    {
                        Console.WriteLine("No audit logs found.");
                        break;
                    }

                    Console.WriteLine();
                    Console.WriteLine("========== AUDIT LOGS ==========");

                    foreach (AuditLog auditLog in auditLogs)
                    {
                        Console.WriteLine(
                            $"Audit ID: {auditLog.AuditId} | " +
                            $"Entity: {auditLog.EntityName} | " +
                            $"Entity ID: {auditLog.EntityId} | " +
                            $"Action: {auditLog.Action} | " +
                            $"Details: {auditLog.Details ?? "N/A"} | " +
                            $"Date: {auditLog.CreatedAt:g}");
                    }

                    Console.WriteLine("================================");
                    break;
                }


            case "19":
                {
                    Console.Write("Enter student name to search: ");
                    string name = Console.ReadLine() ?? string.Empty;

                    List<Student> students =
                        await studentRepository.SearchByNameAsync(name);

                    if (students.Count == 0)
                    {
                        Console.WriteLine("No students found.");
                    }
                    else
                    {
                        foreach (Student student in students)
                        {
                            Console.WriteLine(
                                $"ID: {student.StudentId} | " +
                                $"Name: {student.Name} | " +
                                $"Email: {student.Email} | " +
                                $"Age: {student.Age} | " +
                                $"Department ID: {student.DepartmentId} | " +
                                $"Active: {student.IsActive}");
                        }
                    }

                    break;
                }


            case "20":
                {
                    Console.Write("Enter course name to search: ");
                    string name = Console.ReadLine() ?? string.Empty;

                    List<Course> courses =
                        await courseRepository.SearchByNameAsync(name);

                    if (courses.Count == 0)
                    {
                        Console.WriteLine("No courses found.");
                    }
                    else
                    {
                        foreach (Course course in courses)
                        {
                            Console.WriteLine(
                                $"ID: {course.CourseId} | " +
                                $"Name: {course.CourseName} | " +
                                $"Duration: {course.DurationMonths} months | " +
                                $"Fee: {course.Fee} | " +
                                $"Department ID: {course.DepartmentId} | " +
                                $"Active: {course.IsActive}");
                        }
                    }

                    break;
                }



            case "21":
                {
                    Console.Write("Enter department name to search: ");
                    string name = Console.ReadLine() ?? string.Empty;

                    List<Department> departments =
                        await departmentRepository.SearchByNameAsync(name);

                    if (departments.Count == 0)
                    {
                        Console.WriteLine("No departments found.");
                    }
                    else
                    {
                        foreach (Department department in departments)
                        {
                            Console.WriteLine(
                                $"ID: {department.DepartmentId} | " +
                                $"Name: {department.DepartmentName} | " +
                                $"Location: {department.Location ?? "N/A"} | " +
                                $"Active: {department.IsActive}");
                        }
                    }

                    break;
                }



            case "22":
                {
                    Console.Write("Enter Student ID: ");

                    if (!int.TryParse(Console.ReadLine(), out int studentId))
                    {
                        Console.WriteLine("Invalid Student ID.");
                        break;
                    }

                    List<Enrollment> enrollments =
                        await enrollmentRepository.GetByStudentIdAsync(studentId);

                    if (enrollments.Count == 0)
                    {
                        Console.WriteLine("No enrollments found for this student.");
                    }
                    else
                    {
                        foreach (Enrollment enrollment in enrollments)
                        {
                            Console.WriteLine(
                                $"Enrollment ID: {enrollment.EnrollmentId} | " +
                                $"Student ID: {enrollment.StudentId} | " +
                                $"Course ID: {enrollment.CourseId} | " +
                                $"Date: {enrollment.EnrolledAt} | " +
                                $"Status: {enrollment.Status}");
                        }
                    }

                    break;
                }



            case "23":
                {
                    Console.Write("Enter Course ID: ");

                    if (!int.TryParse(Console.ReadLine(), out int courseId))
                    {
                        Console.WriteLine("Invalid Course ID.");
                        break;
                    }

                    List<Enrollment> enrollments =
                        await enrollmentRepository.GetByCourseIdAsync(courseId);

                    if (enrollments.Count == 0)
                    {
                        Console.WriteLine("No enrollments found for this course.");
                    }
                    else
                    {
                        foreach (Enrollment enrollment in enrollments)
                        {
                            Console.WriteLine(
                                $"Enrollment ID: {enrollment.EnrollmentId} | " +
                                $"Student ID: {enrollment.StudentId} | " +
                                $"Course ID: {enrollment.CourseId} | " +
                                $"Date: {enrollment.EnrolledAt} | " +
                                $"Status: {enrollment.Status}");
                        }
                    }

                    break;
                }

            case "24":
                {

                    Dictionary<string, int> report =
                        await courseRepository.GetEnrollmentCountByCourseAsync();

                    Console.WriteLine("\nEnrollment Count by Course");
                    Console.WriteLine("--------------------------");

                    foreach (var item in report)
                    {
                        Console.WriteLine(
                            $"Course: {item.Key} | Enrollments: {item.Value}");
                    }

                    break;
                }

            case "25":
                {

                    List<string> report =
                        await enrollmentRepository.GetStudentEnrollmentReportAsync();

                    Console.WriteLine("\nStudent Enrollment Report");
                    Console.WriteLine("-------------------------");

                    if (report.Count == 0)
                    {
                        Console.WriteLine("No enrollments found.");
                    }
                    else
                    {
                        foreach (string item in report)
                        {
                            Console.WriteLine(item);
                        }
                    }

                    break;
                }

            default:
                {
                    Console.WriteLine(
                        "Invalid option. Please choose a number from 0 to 17.");
                    break;
                }
        }
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Validation error: {ex.Message}");
    }
    catch (SqlException ex)
    {
        Console.WriteLine($"Database error: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}
