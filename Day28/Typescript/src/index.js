"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// 5. Array of 5 students
const students = [
    {
        id: 101,
        name: "Divya",
        department: "Computer Science",
        age: 23,
        email: "divya@gmail.com"
    },
    {
        id: "S102",
        name: "Anu",
        department: "Information Technology",
        age: 22
    },
    {
        id: 103,
        name: "Rahul",
        department: "Electronics",
        age: 24,
        email: "rahul@gmail.com"
    },
    {
        id: "S104",
        name: "Arun",
        department: "Mechanical",
        age: 23
    },
    {
        id: 105,
        name: "Meera",
        department: "Civil",
        age: 21,
        email: "meera@gmail.com"
    }
];
// 6. Display function
function displayStudents(students) {
    console.log("STUDENT DETAILS");
    students.forEach((student) => {
        console.log("----------------------");
        console.log("ID:", student.id);
        console.log("Name:", student.name);
        console.log("Department:", student.department);
        console.log("Age:", student.age);
        console.log("Email:", student.email ?? "Not provided");
    });
}
// 7. Display all students
displayStudents(students);
// 8. Student with contact details
const studentContact = {
    id: 106,
    name: "John",
    department: "Computer Science",
    age: 22,
    email: "john@gmail.com",
    phone: "9876543210",
    address: "Kerala, India"
};
console.log("\nSTUDENT WITH CONTACT DETAILS");
console.log(studentContact);
