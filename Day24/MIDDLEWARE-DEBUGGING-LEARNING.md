# Day24 -- Visual Studio Debugging: Authentication Diagnostics Middleware

## 1. Learning objective

Learn how to debug the `AuthenticationDiagnosticsMiddleware` in Visual
Studio and inspect the authenticated user's identity and claims while an
HTTP request moves through the ASP.NET Core middleware pipeline.

**Project:** MovieTicketBooking\
**Topic:** ASP.NET Core Middleware and Visual Studio Debugging\
**Date:** 05 November 2026

## 2. Middleware code being debugged

The following middleware logs the authentication state and claims
available on the current `HttpContext.User`.

``` csharp
namespace MovieTicketBooking.Middleware
{
    public class AuthenticationDiagnosticsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationDiagnosticsMiddleware> _logger;

        public AuthenticationDiagnosticsMiddleware(
            RequestDelegate next,
            ILogger<AuthenticationDiagnosticsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation(
                "========== AUTHENTICATION RESULT ==========");

            _logger.LogInformation(
                "Is Authenticated: {IsAuthenticated}",
                context.User.Identity?.IsAuthenticated
            );

            _logger.LogInformation(
                "Authentication Type: {AuthenticationType}",
                context.User.Identity?.AuthenticationType
            );

            foreach (var claim in context.User.Claims)
            {
                _logger.LogInformation(
                    "Claim Type: {ClaimType}, Claim Value: {ClaimValue}",
                    claim.Type,
                    claim.Value
                );
            }

            await _next(context);
        }
    }
}
```

## 3. Important Visual Studio debugging terms

  -----------------------------------------------------------------------
  Term                                Meaning
  ----------------------------------- -----------------------------------
  Breakpoint                          A marker that pauses program
                                      execution at a selected line

  Debugging                           Running the application step by
                                      step to inspect its behavior

  Locals                              Displays local variables and their
                                      current values

  Watch                               Allows selected expressions to be
                                      monitored

  Call Stack                          Shows the sequence of method calls
                                      leading to the current line

  Step Over                           Executes the current line without
                                      entering called methods

  Step Into                           Enters a method call to debug its
                                      implementation

  Step Out                            Finishes the current method and
                                      returns to its caller
  -----------------------------------------------------------------------

## 4. Debugging keyboard shortcuts

  Shortcut      Action
  ------------- ---------------------------------------
  F5            Start debugging or continue execution
  F9            Add or remove a breakpoint
  F10           Step Over
  F11           Step Into
  Shift + F11   Step Out
  Shift + F5    Stop debugging

## 5. Step-by-step debugging procedure

### Step 1 -- Open the middleware file

In Visual Studio:

1.  Open the `MovieTicketBooking` solution.
2.  Expand the `MovieTicketBooking` project.
3.  Open the `Middleware` folder.
4.  Open `AuthenticationDiagnosticsMiddleware.cs`.

### Step 2 -- Set breakpoints

Click the left margin beside these statements, or place the cursor on a
line and press F9:

-   The first `_logger.LogInformation` statement.
-   The `IsAuthenticated` logging statement.
-   The `AuthenticationType` logging statement.
-   The `foreach` statement.
-   The claim logging statement inside the loop.
-   `await _next(context)`.

A red circle indicates that a breakpoint has been set.

### Step 3 -- Start the application in Debug mode

1.  Set `MovieTicketBooking` as the startup project.
2.  Select the required HTTP or HTTPS launch profile.
3.  Press F5 or select **Debug \> Start Debugging**.
4.  Wait for the API to start.

### Step 4 -- Trigger the middleware using Postman

Send a request to an API endpoint that is protected by authentication.
For example, use the actual customer bookings route configured in the
project.

In Postman:

1.  Open the request's **Authorization** tab.
2.  Select **Bearer Token**.
3.  Paste a valid Customer JWT access token.
4.  Send the request.

When the request reaches the middleware, Visual Studio should pause at
the first active breakpoint.

### Step 5 -- Inspect values

While execution is paused, hover over these expressions:

  ---------------------------------------------------------------------------------
  Expression                                    What it tells you
  --------------------------------------------- -----------------------------------
  `context.User`                                Current user principal

  `context.User.Identity`                       Current identity

  `context.User.Identity?.IsAuthenticated`      Whether the current identity is
                                                authenticated

  `context.User.Identity?.AuthenticationType`   Authentication scheme/type, when
                                                supplied

  `context.User.Claims`                         Claims available on the principal

  `claim.Type`                                  Current claim's type/name

  `claim.Value`                                 Current claim's value
  ---------------------------------------------------------------------------------

You can also open these windows from **Debug \> Windows**:

-   **Locals** -- inspect variables in the current method.
-   **Watch \> Watch 1** -- add expressions such as
    `context.User.Identity.IsAuthenticated`.
-   **Call Stack** -- inspect the current method call sequence.

### Step 6 -- Step through the middleware

1.  Press F10 to execute the current statement and move to the next
    statement.
2.  When execution reaches the `foreach`, inspect `context.User.Claims`.
3.  Step through the loop to inspect each `claim.Type` and
    `claim.Value`.
4.  Press F11 only when you intentionally want to enter a called method.
5.  Continue with F5 when you want the request to proceed until the next
    breakpoint.

## 6. Correct middleware registration order

The custom diagnostics middleware must run after authentication if it is
intended to inspect the identity populated by the authentication
handler, and before authorization if it should log the identity before
authorization decisions.

Example `Program.cs` order:

``` csharp
app.UseRouting();

app.UseAuthentication();

app.UseMiddleware<AuthenticationDiagnosticsMiddleware>();

app.UseAuthorization();

app.MapControllers();
```

**Execution concept:**

``` text
HTTP Request
     |
     v
UseRouting
     |
     v
UseAuthentication
     |
     v
AuthenticationDiagnosticsMiddleware
     |
     v
UseAuthorization
     |
     v
Controller endpoint
```

The actual application may have additional middleware. Preserve required
middleware such as exception handling and HTTPS redirection in the
appropriate positions.

## 7. Compilation errors encountered and corrections

### Error CS0103

**Error:**

``` text
The name 'claim' does not exist in the current context
```

**Cause:** The variable `claim` is declared by the `foreach` statement.
It is only available inside that loop's block. Referencing it after the
closing brace causes a scope error.

**Correct code:**

``` csharp
foreach (var claim in context.User.Claims)
{
    _logger.LogInformation(
        "Claim Type: {ClaimType}, Claim Value: {ClaimValue}",
        claim.Type,
        claim.Value
    );
}
```

### Error CS1061

**Error:**

``` text
'HttpContext' does not contain a definition for 'claim'
```

**Cause:** `claim` is not a property of `HttpContext`. Claims are
accessed through the current user's principal.

**Incorrect:**

``` csharp
context.claim
```

**Correct:**

``` csharp
context.User.Claims
```

### Key concept: variable scope

-   `context` is the `HttpContext` parameter of `InvokeAsync`.
-   `context.User` is the current request's `ClaimsPrincipal`.
-   `context.User.Claims` is the collection of claims.
-   `claim` is the loop variable declared in
    `foreach (var claim in ...)`.
-   `claim` can only be used inside the loop body.

## 8. Test scenarios

Run the API under the debugger and test these scenarios using Postman.

  -----------------------------------------------------------------------
  Scenario                            Expected authentication observation
  ----------------------------------- -----------------------------------
  Request without a token             `IsAuthenticated` is normally
                                      `False`

  Request with a valid Customer JWT   `IsAuthenticated` should be `True`

  Request with a valid Admin JWT      `IsAuthenticated` should be `True`

  Request with an expired or invalid  Normally unauthenticated; the
  token                               endpoint may reject the request
  -----------------------------------------------------------------------

The exact authentication type and claim names depend on the JWT bearer
configuration and token contents. A valid token does not automatically
mean the user is authorized to access every endpoint.

## 9. Authentication vs authorization

**Authentication** answers: "Who is making this request?"

Examples:

-   Is the user authenticated?
-   Which identity is associated with the request?
-   What claims are present?

**Authorization** answers: "Is this authenticated user allowed to
perform this action?"

Examples:

-   Can a Customer access their own booking?
-   Can one Customer cancel another Customer's booking?
-   Can an Admin create or manage shows?

An authenticated user can still receive `403 Forbidden` when they do not
have permission to access a resource.

## 10. Important security note

This middleware is for development and learning diagnostics.

-   Do not log JWT access tokens.
-   Avoid logging sensitive claim values or personal information.
-   Restrict detailed claim logging to a safe Development environment.
-   Do not leave verbose authentication diagnostics enabled in
    production.

## 11. Verification checklist

-   [ ] Opened `AuthenticationDiagnosticsMiddleware.cs`.
-   [ ] Set breakpoints using F9.
-   [ ] Started the API using F5.
-   [ ] Sent a request from Postman.
-   [ ] Inspected `context.User`.
-   [ ] Checked `IsAuthenticated`.
-   [ ] Checked `AuthenticationType`.
-   [ ] Inspected claims inside the `foreach` loop.
-   [ ] Verified middleware registration order in `Program.cs`.
-   [ ] Built the project successfully.
-   [ ] Tested requests with and without a JWT.

## 12. Git milestone

After adding this learning document to the Day24 project and verifying
the content:

``` bash
git status
git add Day24/MIDDLEWARE-DEBUGGING-LEARNING.md
git commit -m "docs: add middleware debugging learning notes"
git push origin feature/day24-middleware-learning
```

If the document is already inside the `Day24` folder and the repository
root is `D:\HCL`, the staging command above is relative to that
repository root.

**Milestone:** Visual Studio debugging fundamentals and authentication
diagnostics middleware inspection documented for Day24.
