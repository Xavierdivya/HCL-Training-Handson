# 🎓 Student Management System

## 📌 Project Description

The Student Management System is a simple web application developed using HTML, CSS, and JavaScript. It helps manage student records by allowing users to add student details, remove students using their email address, and display the list of registered students.

The application is designed for PSNA College of Engineering & Technology and demonstrates basic front-end web development concepts.

## ✨ Features

- **Add Student:** Enter and store student details.
- **Remove Student:** Delete a student record using the student's email address.
- **Show Student List:** Display all registered students in a table.
- **Input Validation:** Validate required fields and HSC cutoff marks.
- **Duplicate Prevention:** Prevent multiple student records with the same email address.
- **Status Messages:** Display success and error messages for user actions.
- **Responsive Design:** Adapt the layout to different screen sizes.
- **Dynamic Table:** Update the student list automatically when records are added or removed.

## 🛠️ Technologies Used

- **HTML5** – Structures the admission form and student table.
- **CSS3** – Provides styling, colors, layouts, and responsive design.
- **JavaScript** – Implements student record management and validation.

## 📂 Project Structure

```text
Student-Management-System/
│
├── index.html
├── Student-Management-System.css
├── Student-Management-System.js
└── README.md
```

## 📝 Student Details

The application collects the following information:

| Field | Description |
|---|---|
| Name | Student's full name |
| HSC Cutoff | Higher Secondary School cutoff marks |
| Department | CSE, IT, or AIDS |
| Email | Student's email address |
| Phone Number | Student's contact number |
| Address | Student's residential address |

## 🚀 How to Run the Project

1. Download or clone the project repository.
2. Keep the HTML, CSS, and JavaScript files in the same folder.
3. Open `index.html` in a modern web browser.
4. Fill in the student admission form.
5. Click **ADD** to register a student.
6. Enter a registered student's email and click **REMOVE** to delete that record.
7. Click **SHOW LIST** to view the student records.

## ⚙️ How It Works

### 1. Add Student

The `addStudent()` function retrieves the values entered in the form, validates the information, checks for duplicate email addresses, and stores the student record in the `Student_List` array.

### 2. Remove Student

The `removeStudent()` function searches for a student by email address. If the student exists, the record is removed using the JavaScript `splice()` method. Otherwise, an error message is displayed.

### 3. Show Student List

The `showStudentList()` function clears the existing table rows and dynamically creates new rows for each student stored in the array.

### 4. Validation

The application checks that required fields are filled in, the HSC cutoff is between 0 and 200, and the email address is not already registered.

## 🎯 Project Objectives

- Understand HTML forms and input elements.
- Learn CSS styling and responsive web design.
- Practice JavaScript functions, arrays, and objects.
- Implement CRUD-related operations for student records.
- Learn DOM manipulation and dynamic table creation.
- Apply basic input validation and error handling.

## 🔮 Future Enhancements

- Add student record editing functionality.
- Implement search and filtering by name, department, or email.
- Store student records permanently using local storage or a database.
- Add student ID generation.
- Improve validation for phone numbers and email addresses.
- Add authentication for authorized administrators.
- Export student records to CSV or PDF.

## ⚠️ Limitations

- Student records are stored in an in-memory JavaScript array.
- Data is lost when the page is refreshed or closed.
- The application does not use a backend server or database.
- It is intended for educational and demonstration purposes rather than production use.

## 📄 License

This project is intended for educational and personal learning purposes.
