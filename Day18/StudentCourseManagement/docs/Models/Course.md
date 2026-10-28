# Course Model

> Keep this page aligned with `Models/Course.cs`. Do not document a
> property until you confirm it exists in the source code.

## Purpose

Represents a course that students can enroll in.

## Database Mapping

-   **Table:** `Courses` (confirm the exact table name in the SQL
    schema)
-   **Primary key:** Confirm from the model and database schema.

## Properties

  ----------------------------------------------------------------------------------
  Property       C# Type        Meaning        Required?      Notes
  -------------- -------------- -------------- -------------- ----------------------
  Add each       Fill in        Fill in        Fill in        Note
  property from                                               nullability/defaults
  `Course.cs`                                                 

  ----------------------------------------------------------------------------------

## Relationships and Rules

-   Document course-to-enrollment relationships as implemented.
-   Enrollment eligibility requires an active course according to the
    enrollment validation workflow.
-   Document any unique course-name rule only if the database/code
    enforces it.

## Change History

  ------------------------------------------------------------------------
  Date              Commit            Change            Verification
  ----------------- ----------------- ----------------- ------------------
  YYYY-MM-DD        `<commit-hash>`   Describe the      Build/test/query
                                      model change      performed

  ------------------------------------------------------------------------

## Update Checklist

-   [ ] Re-check properties against `Course.cs`
-   [ ] Record validation and database constraints
-   [ ] Update the change-history table
-   [ ] Link the commit hash using
    `git log --oneline -- Models/Course.cs`
