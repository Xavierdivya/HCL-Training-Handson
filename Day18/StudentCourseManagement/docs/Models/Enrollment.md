# Enrollment Model

> Keep this page aligned with `Models/Enrollment.cs`. Do not document a
> property until you confirm it exists in the source code.

## Purpose

Represents a student's enrollment in a course.

## Database Mapping

-   **Table:** `Enrollments` (confirm the exact table name in the SQL
    schema)
-   **Primary key:** Confirm from the model and database schema.

## Properties

  -------------------------------------------------------------------------------------
  Property          C# Type        Meaning        Required?      Notes
  ----------------- -------------- -------------- -------------- ----------------------
  Add each property Fill in        Fill in        Fill in        Note
  from                                                           nullability/defaults
  `Enrollment.cs`                                                

  -------------------------------------------------------------------------------------

## Relationships and Rules

-   Links a student to a course using the corresponding IDs.
-   The database unique constraint `UQ_Enrollments_Student_Course`
    prevents duplicate student-course pairs in the current database.
-   Enrollment creation checks that the student and course are active.
-   Application logic limits a student to five active enrollments.
-   Enrollment creation and the corresponding audit record are written
    within one SQL transaction; the operation commits on success and
    rolls back on failure.

## Change History

  --------------------------------------------------------------------------
  Date              Commit            Change              Verification
  ----------------- ----------------- ------------------- ------------------
  2026-10-28        `e2f04c1`         Create enrollment   Runtime test and
                                      and audit record    SQL verification
                                      transactionally     succeeded

  2026-10-27        `5c81eac`         Add enrollment      Code review;
                                      eligibility and     dedicated
                                      active-enrollment   max-limit runtime
                                      limit validation    test still pending

  YYYY-MM-DD        `<commit-hash>`   Describe another    Build/test/query
                                      model change        performed
  --------------------------------------------------------------------------

## Update Checklist

-   [ ] Re-check properties against `Enrollment.cs`
-   [ ] Record status values and their allowed transitions
-   [ ] Record database constraints and transaction behavior
-   [ ] Update the change-history table
-   [ ] Link model history using
    `git log --oneline -- Models/Enrollment.cs`
