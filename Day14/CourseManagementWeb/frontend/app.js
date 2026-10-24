
const API_URL = "https://localhost:7201/api";

const $ = (selector) => document.querySelector(selector);

const message = $("#message");

function showMessage(text, isError = false) {
    message.textContent = text;
    message.className = text
        ? (isError ? "error" : "success")
        : "";
}

function escapeHtml(value) {
    return String(value ?? "").replace(/[&<>"']/g, (character) => ({
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': "&quot;",
        "'": "&#39;"
    })[character]);
}

function money(value) {
    return Number(value ?? 0).toFixed(2);
}

async function apiRequest(path, options = {}) {
    let response;

    try {
        response = await fetch(`${API_URL}${path}`, {
            ...options,
            headers: {
                ...(options.body
                    ? { "Content-Type": "application/json" }
                    : {}),
                ...options.headers
            }
        });
    } catch {
        throw new Error(
            "Cannot connect to the API. Check the HTTPS port, " +
            "start the backend, and trust its development certificate."
        );
    }

    if (!response.ok) {
        const contentType = response.headers.get("content-type") || "";
        let errorMessage = `Request failed (${response.status}).`;

        if (contentType.includes("application/json")) {
            const data = await response.json();

            if (data.errors) {
                errorMessage = Object.values(data.errors)
                    .flat()
                    .join(" ");
            } else {
                errorMessage =
                    data.message ||
                    data.title ||
                    data.detail ||
                    errorMessage;
            }
        } else {
            const text = await response.text();

            if (text) {
                errorMessage = text;
            }
        }

        throw new Error(errorMessage);
    }

    if (response.status === 204) {
        return null;
    }

    const contentType = response.headers.get("content-type") || "";

    return contentType.includes("application/json")
        ? response.json()
        : null;
}

function renderRows(selector, rows, columnCount, emptyText) {
    const body = $(selector);

    if (!rows.length) {
        body.innerHTML =
            `<tr><td colspan="${columnCount}">` +
            `${escapeHtml(emptyText)}</td></tr>`;
        return;
    }

    body.innerHTML = rows.join("");
}

function setOptions(selector, items, valueField, labelField, placeholder) {
    const select = $(selector);
    const previousValue = select.value;

    select.innerHTML =
        `<option value="">${escapeHtml(placeholder)}</option>`;

    for (const item of items) {
        const option = document.createElement("option");

        option.value = item[valueField];
        option.textContent = item[labelField];

        select.appendChild(option);
    }

    if (items.some(
        item => String(item[valueField]) === previousValue
    )) {
        select.value = previousValue;
    }
}

async function loadCourses() {
    const courses = await apiRequest("/courses");

    $("#courseCount").textContent = courses.length;

    renderRows(
        "#coursesTable",
        courses.map(course => `
            <tr>
                <td>${escapeHtml(course.courseCode)}</td>
                <td>${escapeHtml(course.courseTitle)}</td>
                <td>${money(course.fee)}</td>
                <td>
                    ${escapeHtml(course.enrolledCount)}
                    /
                    ${escapeHtml(course.totalSeats)}
                </td>
                <td>${escapeHtml(course.durationHours)} hours</td>
            </tr>
        `),
        5,
        "No courses available."
    );

    const availableCourses = courses.filter(course =>
        Number(course.enrolledCount) < Number(course.totalSeats)
    );

    setOptions(
        "#enrollCourseCode",
        availableCourses,
        "courseCode",
        "courseTitle",
        "Select course"
    );
}

async function loadLearners() {
    const learners = await apiRequest("/learners");

    $("#learnerCount").textContent = learners.length;

    renderRows(
        "#learnersTable",
        learners.map(learner => `
            <tr>
                <td>${escapeHtml(learner.learnerId)}</td>
                <td>${escapeHtml(learner.learnerName)}</td>
                <td>${escapeHtml(learner.email)}</td>
            </tr>
        `),
        3,
        "No learners available."
    );

    setOptions(
        "#enrollLearnerId",
        learners,
        "learnerId",
        "learnerName",
        "Select learner"
    );
}

async function loadEnrollments() {
    const enrollments = await apiRequest("/enrollments");

    $("#enrollmentCount").textContent = enrollments.length;

    renderRows(
        "#enrollmentsTable",
        enrollments.map(enrollment => {
            const status = String(enrollment.status ?? "");
            const isActive = status.toLowerCase() === "active";

            return `
                <tr>
                    <td>${escapeHtml(enrollment.enrollmentId)}</td>
                    <td>
                        ${escapeHtml(
                enrollment.learnerName ?? enrollment.learnerId
            )}
                    </td>
                    <td>
                        ${escapeHtml(
                enrollment.courseTitle ?? enrollment.courseCode
            )}
                    </td>
                    <td>
                        ${escapeHtml(
                enrollment.enrollmentDate
                    ? new Date(enrollment.enrollmentDate)
                        .toLocaleDateString()
                    : ""
            )}
                    </td>
                    <td>${money(enrollment.finalFee)}</td>
                    <td>${escapeHtml(status)}</td>
                    <td>
                        ${isActive
                    ? `<button type="button"
                                class="cancel-button"
                                data-cancel="${escapeHtml(
                        enrollment.enrollmentId
                    )}">
                                Cancel
                               </button>`
                    : "—"}
                    </td>
                </tr>
            `;
        }),
        7,
        "No enrollments available."
    );
}

async function refreshAll() {
    try {
        await Promise.all([
            loadCourses(),
            loadLearners(),
            loadEnrollments()
        ]);
    } catch (error) {
        showMessage(error.message, true);
        throw error;
    }
}

async function handleFormSubmit(form, submitButton, operation) {
    if (!form.reportValidity()) {
        return;
    }

    const originalText = submitButton.textContent;

    submitButton.disabled = true;
    submitButton.textContent = "Please wait...";

    try {
        await operation();

        form.reset();

        showMessage("Operation completed successfully.");

        await refreshAll();
    } catch (error) {
        showMessage(error.message, true);
    } finally {
        submitButton.disabled = false;
        submitButton.textContent = originalText;
    }
}

// Add course
$("#courseForm").addEventListener("submit", async event => {
    event.preventDefault();

    const form = event.currentTarget;
    const button = form.querySelector('button[type="submit"]');

    const course = {
        courseCode: $("#courseCode").value.trim(),
        courseTitle: $("#courseTitle").value.trim(),
        fee: Number($("#fee").value),
        totalSeats: Number($("#totalSeats").value),
        durationHours: Number($("#durationHours").value)
    };

    await handleFormSubmit(form, button, async () => {
        await apiRequest("/courses", {
            method: "POST",
            body: JSON.stringify(course)
        });
    });
});

// Add learner
$("#learnerForm").addEventListener("submit", async event => {
    event.preventDefault();

    const form = event.currentTarget;
    const button = form.querySelector('button[type="submit"]');

    const learner = {
        learnerId: $("#learnerId").value.trim(),
        learnerName: $("#learnerName").value.trim(),
        email: $("#email").value.trim()
    };

    await handleFormSubmit(form, button, async () => {
        await apiRequest("/learners", {
            method: "POST",
            body: JSON.stringify(learner)
        });
    });
});

// Enroll learner
$("#enrollmentForm").addEventListener("submit", async event => {
    event.preventDefault();

    const form = event.currentTarget;
    const button = form.querySelector('button[type="submit"]');

    const enrollment = {
        learnerId: $("#enrollLearnerId").value,
        courseCode: $("#enrollCourseCode").value
    };

    await handleFormSubmit(form, button, async () => {
        await apiRequest("/enrollments", {
            method: "POST",
            body: JSON.stringify(enrollment)
        });
    });
});

// Refresh all tables
$("#refreshBtn").addEventListener("click", async () => {
    showMessage("");

    try {
        await refreshAll();
        showMessage("Data refreshed successfully.");
    } catch {
        // refreshAll displays the error message.
    }
});

// Cancel enrollment using event delegation
$("#enrollmentsTable").addEventListener("click", async event => {
    const button = event.target.closest("[data-cancel]");

    if (!button || button.disabled) {
        return;
    }

    const enrollmentId = button.dataset.cancel;

    if (!confirm(
        `Are you sure you want to cancel enrollment ${enrollmentId}?`
    )) {
        return;
    }

    button.disabled = true;

    try {
        await apiRequest(
            `/enrollments/${encodeURIComponent(enrollmentId)}/cancel`,
            { method: "PUT" }
        );

        showMessage("Enrollment cancelled successfully.");

        await refreshAll();
    } catch (error) {
        showMessage(error.message, true);
        button.disabled = false;
    }
});

// Initial page load
refreshAll().catch(() => {
    // refreshAll already displays the error.
});
