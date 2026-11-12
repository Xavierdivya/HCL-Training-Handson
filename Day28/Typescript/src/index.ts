
// 2. Student interface
interface Student {
    id: number | string;  // Union type for id
    name: string;
    department: string;
    age: number;
    email?: string;  // Optional property
}

// 3. Contact Details interface
interface ContactDetails {
    phone: string;
    address: string;
}

// 4. Combine Student and ContactDetails interfaces
interface StudentWithContact extends Student, ContactDetails {
    // Inherits all properties from both interfaces
}

// 5. Array of 5 students
const students: Student[] = [
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
function displayStudents(students: Student[]): void {
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
const studentContact: StudentWithContact = {
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
