
let Student_List = [];

function addStudent() {
    const name = document.getElementById("name").value.trim();
    const cutoffValue = document.getElementById("cutoff").value;
    const cutoff = Number(cutoffValue);
    const dept = document.getElementById("dept").value;
    const email = document.getElementById("email").value.trim();
    const ph = document.getElementById("ph").value.trim();
    const address = document.getElementById("address").value.trim();

    const message = document.getElementById("addedmsg");

    // Validate required fields
    if (!name || !cutoffValue || !email || !ph || !address) {
        message.textContent = "Please fill in all the fields.";
        return;
    }

    // Validate cutoff marks
    if (!Number.isFinite(cutoff) || cutoff < 0 || cutoff > 200) {
        message.textContent = "Cutoff must be between 0 and 200.";
        return;
    }

    // Prevent duplicate email addresses
    const existingStudent = Student_List.find(
        student => student.email.toLowerCase() === email.toLowerCase()
    );

    if (existingStudent) {
        message.textContent = "A student with this email already exists.";
        return;
    }

    // Add student to the array
    Student_List.push({
        name,
        cutoff,
        dept,
        email,
        ph,
        address
    });

    message.textContent = "Student added successfully!";

    document.getElementById("studentForm").reset();

    showStudentList();
}

function removeStudent() {
    const email = document.getElementById("email").value.trim();
    const message = document.getElementById("addedmsg");

    if (!email) {
        message.textContent = "Please enter the student's email to remove.";
        return;
    }

    const index = Student_List.findIndex(
        student => student.email.toLowerCase() === email.toLowerCase()
    );

    if (index === -1) {
        message.textContent =
            "Student not found. Please enter a valid student email.";
        return;
    }

    Student_List.splice(index, 1);

    message.textContent = "Student removed successfully!";

    showStudentList();
}

function showStudentList() {
    const tbody = document.getElementById("studentTableBody");

    // Clear the existing rows
    tbody.replaceChildren();

    // Display all students
    for (const student of Student_List) {
        const row = document.createElement("tr");

        for (const field of [
            "name",
            "cutoff",
            "dept",
            "email",
            "ph",
            "address"
        ]) {
            const cell = document.createElement("td");
            cell.textContent = student[field];
            row.appendChild(cell);
        }

        tbody.appendChild(row);
    }

    if (Student_List.length === 0) {
        document.getElementById("addedmsg").textContent =
            "No student records available.";
    }
}
