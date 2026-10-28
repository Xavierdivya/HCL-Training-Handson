# AuditLog Model

> Keep this page aligned with `Models/AuditLog.cs`. Do not document a
> property until you confirm it exists in the source code.

## Purpose

Represents an audit event that records an action performed by the
application.

## Database Mapping

-   **Table:** `AuditLogs` (confirm the exact table name in the SQL
    schema)
-   **Primary key:** Confirm from the model and database schema.

## Properties

  -----------------------------------------------------------------------------------
  Property        C# Type        Meaning        Required?      Notes
  --------------- -------------- -------------- -------------- ----------------------
  Add each        Fill in        Fill in        Fill in        Note
  property from                                                nullability/defaults
  `AuditLog.cs`                                                

  -----------------------------------------------------------------------------------

## Relationships and Rules

-   Enrollment creation currently records an audit event with entity
    name `Enrollment` and action `Created`.
-   The audit event is written using the same database transaction as
    the enrollment, so both are committed or rolled back together.
-   Document other audited actions only after confirming they are
    implemented.

## Change History

  ------------------------------------------------------------------------
  Date              Commit            Change            Verification
  ----------------- ----------------- ----------------- ------------------
  2026-10-28        `e2f04c1`         Save enrollment   SQL query
                                      and audit record  confirmed matching
                                      in one            audit record
                                      transaction       

  YYYY-MM-DD        `<commit-hash>`   Describe another  Build/test/query
                                      model change      performed
  ------------------------------------------------------------------------

## Update Checklist

-   [ ] Re-check properties against `AuditLog.cs`
-   [ ] Document entity/action naming conventions
-   [ ] Update the change-history table
-   [ ] Link commit history using
    `git log --oneline -- Models/AuditLog.cs`
