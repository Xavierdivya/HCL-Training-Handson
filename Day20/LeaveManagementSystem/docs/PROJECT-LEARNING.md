## Milestone 11: Prevent Overlapping Leave Requests

### Business rule
An employee cannot submit a leave request overlapping an existing Pending or Approved request.

### Implementation
- Use `AnyAsync()` to check existing requests.
- Match the employee ID.
- Include only Pending or Approved statuses.
- Check overlap using `existing.StartDate <= endDate` and `existing.EndDate >= startDate`.

### Test result
A request overlapping an existing Pending or Approved request was rejected with the expected validation message.
