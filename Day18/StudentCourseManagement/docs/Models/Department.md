# Department Model

> Keep this page aligned with `Models/Department.cs`. Do not document a
> property until you confirm it exists in the source code.

## Purpose

Represents a department in the student/course management system.

## Database Mapping

-   **Table:** `Departments` (confirm the exact table name in the SQL
    schema)
-   **Primary key:** Confirm from the model and database schema.

## Properties

  -------------------------------------------------------------------------------------
  Property          C# Type        Meaning        Required?      Notes
  ----------------- -------------- -------------- -------------- ----------------------
  Add each property Fill in        Fill in        Fill in        Note
  from                                                           nullability/defaults
  `Department.cs`                                                

  -------------------------------------------------------------------------------------

## Relationships and Rules

-   Record relationships with students or other entities as implemented.
-   Document duplicate-name rules or other validation only after
    checking the service/database code.

## Change History

  ------------------------------------------------------------------------
  Date              Commit            Change            Verification
  ----------------- ----------------- ----------------- ------------------
  YYYY-MM-DD        `<commit-hash>`   Describe the      Build/test/query
                                      model change      performed

  ------------------------------------------------------------------------

## Update Checklist

-   [ ] Re-check properties against `Department.cs`
-   [ ] Record validation and database constraints
-   [ ] Update the change-history table
-   [ ] Link the commit hash using
    `git log --oneline -- Models/Department.cs`
