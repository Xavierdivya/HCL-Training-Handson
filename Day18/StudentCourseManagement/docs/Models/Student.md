# Student Model

> Keep this page aligned with `Models/Student.cs`. Do not document a
> property until you confirm it exists in the source code.

## Purpose

Represents a student who may enroll in courses.

## Database Mapping

-   **Table:** `Students` (confirm the exact table name in the SQL
    schema)
-   **Primary key:** Confirm from the model and database schema.

## Properties

  ----------------------------------------------------------------------------------
  Property       C# Type        Meaning        Required?      Notes
  -------------- -------------- -------------- -------------- ----------------------
  Add each       Fill in        Fill in        Fill in        Note
  property from                                               nullability/defaults
  `Student.cs`                                                

  ----------------------------------------------------------------------------------

## Relationships and Rules

-   Document the department relationship if present.
-   Enrollment eligibility requires an active student according to the
    enrollment validation workflow.
-   Add any age, email, or other validation only if it is present in the
    current code.

## Change History

  ------------------------------------------------------------------------
  Date              Commit            Change            Verification
  ----------------- ----------------- ----------------- ------------------
  YYYY-MM-DD        `<commit-hash>`   Describe the      Build/test/query
                                      model change      performed

  ------------------------------------------------------------------------

## Update Checklist

-   [ ] Re-check properties against `Student.cs`
-   [ ] Record validation and database constraints
-   [ ] Update the change-history table
-   [ ] Link the commit hash using
    `git log --oneline -- Models/Student.cs`
