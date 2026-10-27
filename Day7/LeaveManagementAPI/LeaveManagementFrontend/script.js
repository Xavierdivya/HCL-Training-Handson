
const API_URL = "https://localhost:7171/api/LeaveRequests";

const leaveForm = document.getElementById("leaveForm");
const message = document.getElementById("message");
const tableBody = document.getElementById("leaveTableBody");
const statusFilter = document.getElementById("statusFilter");
const refreshBtn = document.getElementById("refreshBtn");

let leaveRequests = [];

// Display success and error messages
function showMessage(text, isError = false) {
    message.textContent = text;
    message.style.color = isError ? "red" : "green";
}

// Format dates returned by the API
function formatDate(value) {
    return value ? String(value).slice(0, 10) : "";
}

// Load leave requests from the C# API
async function loadLeaveRequests() {
    tableBody.replaceChildren();

    const loadingRow = tableBody.insertRow();
    const loadingCell = loadingRow.insertCell();

    loadingCell.colSpan = 8;
    loadingCell.textContent = "Loading requests...";

    try {
        const selectedStatus = statusFilter.value;

        const url = selectedStatus === "All"
            ? API_URL
            : `${API_URL}?status=${encodeURIComponent(selectedStatus)}`;

        const response = await fetch(url);

        if (!response.ok) {
            throw new Error(
                `Failed to load requests: ${response.status}`
            );
        }

        leaveRequests = await response.json();

        displayLeaveRequests();
    } catch (error) {
        console.error(error);

        tableBody.replaceChildren();

        const row = tableBody.insertRow();
        const cell = row.insertCell();

        cell.colSpan = 8;
        cell.textContent =
            "Could not load requests. Check the API and database connection.";

        showMessage(error.message, true);
    }
}

// Display leave requests in the table
function displayLeaveRequests() {
    tableBody.replaceChildren();

    if (leaveRequests.length === 0) {
        const row = tableBody.insertRow();
        const cell = row.insertCell();

        cell.colSpan = 8;
        cell.textContent = "No leave requests found.";

        return;
    }

    leaveRequests.forEach(leave => {
        const row = tableBody.insertRow();

        // IMPORTANT: Keep the order consistent with index.html
        const values = [
            leave.id,
            leave.employeeName,
            leave.leaveType,
            formatDate(leave.startDate),
            formatDate(leave.endDate),
            leave.reason,
            leave.status
        ];

        // Create the first seven columns
        values.forEach(value => {
            const cell = row.insertCell();
            cell.textContent = value ?? "";
        });

        // Eighth column: Actions
        const actionsCell = row.insertCell();

        if (leave.status === "Pending") {
            const approveBtn = document.createElement("button");

            approveBtn.type = "button";
            approveBtn.textContent = "Approve";

            approveBtn.addEventListener("click", () => {
                updateLeaveStatus(leave, "Approved");
            });

            const rejectBtn = document.createElement("button");

            rejectBtn.type = "button";
            rejectBtn.textContent = "Reject";

            rejectBtn.addEventListener("click", () => {
                updateLeaveStatus(leave, "Rejected");
            });

            actionsCell.append(approveBtn, rejectBtn);
        } else {
            actionsCell.textContent = "No actions";
        }
    });
}

// Submit a new leave request
leaveForm.addEventListener("submit", async event => {
    event.preventDefault();

    const employeeName =
        document.getElementById("employeeName").value.trim();

    const leaveType =
        document.getElementById("leaveType").value;

    const startDate =
        document.getElementById("startDate").value;

    const endDate =
        document.getElementById("endDate").value;

    const reason =
        document.getElementById("reason").value.trim();

    if (
        !employeeName ||
        !leaveType ||
        !startDate ||
        !endDate ||
        !reason
    ) {
        showMessage("Please fill in all fields.", true);
        return;
    }

    if (endDate < startDate) {
        showMessage(
            "End date cannot be earlier than start date.",
            true
        );
        return;
    }

    const leaveRequest = {
        employeeName,
        leaveType,
        startDate,
        endDate,
        reason
    };

    try {
        const response = await fetch(API_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(leaveRequest)
        });

        if (!response.ok) {
            const errorText = await response.text();

            throw new Error(
                errorText || `Request failed: ${response.status}`
            );
        }

        showMessage("Leave request submitted successfully.");

        leaveForm.reset();

        await loadLeaveRequests();
    } catch (error) {
        console.error(error);

        showMessage(
            `Submission failed: ${error.message}`,
            true
        );
    }
});

// Approve or reject an existing leave request
async function updateLeaveStatus(leave, newStatus) {
    try {
        const response = await fetch(
            `${API_URL}/${leave.id}`,
            {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    ...leave,
                    status: newStatus
                })
            }
        );

        if (!response.ok) {
            const errorText = await response.text();

            throw new Error(
                errorText ||
                `Failed to update status: ${response.status}`
            );
        }

        showMessage(
            `Leave request ${newStatus.toLowerCase()} successfully.`
        );

        await loadLeaveRequests();
    } catch (error) {
        console.error(error);

        showMessage(
            `Status update failed: ${error.message}`,
            true
        );
    }
}

// Filter requests by status
statusFilter.addEventListener("change", loadLeaveRequests);

// Refresh the table
refreshBtn.addEventListener("click", loadLeaveRequests);

// Load requests when the page opens
loadLeaveRequests();
