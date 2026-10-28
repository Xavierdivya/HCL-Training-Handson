# Project Changelog

Record notable project changes here, newest entries first. Keep entries
factual and link them to the commit that introduced the change.

## 2026-10-28

-   `e2f04c1` --- `feat: make enrollment creation transactional`
    -   Enrollment creation and audit-log insertion use one SQL
        transaction.
    -   Runtime test succeeded and SQL queries confirmed both records
        were saved.

## 2026-10-27

-   `5c81eac` --- `feat: validate enrollment eligibility and limits`
    -   Added checks for active student/course eligibility and a maximum
        of five active enrollments per student.
    -   A dedicated runtime test for the five-enrollment limit remains
        to be completed.
-   `273cc98` --- `feat: add student enrollment report`
-   `6926fc2` --- `feat: add enrollment count by course report`
-   `4353a24` --- `feat: add student count by department report`

## How to Add an Entry

1.  Make and verify the code change.
2.  Commit it with a Conventional Commit message.
3.  Run `git log --oneline -5` to find the commit hash.
4.  Add the date, hash, summary, and verification result above.
5.  Update the relevant `docs/Models/*.md` file if a model changed.
6.  Commit the documentation update, for example:

``` bash
git add README.md docs
git commit -m "docs: record enrollment transaction changes"
```
