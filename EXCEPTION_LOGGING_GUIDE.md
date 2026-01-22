# Exception Logging Implementation Guide

## Overview
The ExceptionLog functionality has been implemented as a service-only feature without a dedicated controller. This ensures that exception logging is handled internally by the application and not exposed through public API endpoints.

## Architecture Changes

### 1. Removed ExceptionLogsController
- The `ExceptionLogsController` has been removed from the API layer
- Exception logging is now only accessible through the service layer

### 2. Enhanced ExceptionLogService
The `IExceptionLogService` interface now includes a convenient method for logging exceptions:

```csharp
Task<long> LogExceptionAsync(Exception exception, Guid? userId = null, string? additionalInfo = null);
```

### 3. Updated DTOs
The ExceptionLog DTOs have been enhanced to include:
- `StackTrace` - Full stack trace of the exception
- `InnerException` - Inner exception details
- `AdditionalInfo` - Custom contextual information

## How to Use in Controllers

### Step 1: Inject IExceptionLogService

In your controller constructor, inject the `IExceptionLogService`:

```csharp
public class MyController : ControllerBase
{
    private readonly IMyService _myService;
    private readonly IExceptionLogService _exceptionLogService;
    private readonly IMapper _mapper;

    public MyController(
        IMyService myService, 
        IExceptionLogService exceptionLogService,
        IMapper mapper)
    {
        _myService = myService;
        _exceptionLogService = exceptionLogService;
        _mapper = mapper;
    }
}
```

### Step 2: Log Exceptions in Catch Blocks

In your controller action methods, catch exceptions and log them:

```csharp
[HttpPost]
public async Task<ActionResult<MyResponse>> CreateItem([FromBody] CreateItemRequest request)
{
    try
    {
        var createDto = _mapper.Map<CreateMyDto>(request);
        var itemDto = await _myService.CreateAsync(createDto);
        var response = _mapper.Map<MyResponse>(itemDto);
        
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        // Log the exception to database
        await _exceptionLogService.LogExceptionAsync(
            ex, 
            userId: null, // or pass current user ID if available
            additionalInfo: $"CreateItem - ItemName: {request?.Name}"
        );
        return StatusCode(500, new { error = "An error occurred while creating the item" });
    }
}
```

### Step 3: Include Contextual Information

When logging exceptions, include relevant context that will help with debugging:

```csharp
// For create operations
await _exceptionLogService.LogExceptionAsync(ex, userId, $"CreateAccount - Code: {request?.AccountCode}");

// For update operations
await _exceptionLogService.LogExceptionAsync(ex, userId, $"UpdateAccount - Id: {id}");

// For delete operations
await _exceptionLogService.LogExceptionAsync(ex, userId, $"DeleteAccount - Id: {id}");

// For queries
await _exceptionLogService.LogExceptionAsync(ex, userId, $"GetAccountByCode - Code: {code}");
```

## Example Implementation

See `AccountsController.cs` for a complete example of exception logging implementation across all CRUD operations.

## What Gets Logged

The `LogExceptionAsync` method automatically captures:
- **Type**: Exception class name (e.g., `SqlException`, `NullReferenceException`)
- **ErrorMessage**: The exception message
- **StackTrace**: Full stack trace
- **InnerException**: Inner exception details (if any)
- **AdditionalInfo**: Custom context you provide
- **UserId**: Associated user (if provided)
- **DeviceName**: Server/device name (optional)
- **CreatedAt**: Timestamp (auto-generated)

## Best Practices

1. **Always Log Unhandled Exceptions**: Use the generic `catch (Exception ex)` block to log unexpected errors
2. **Include Context**: Always provide relevant context in the `additionalInfo` parameter
3. **Don't Expose Details**: Return generic error messages to clients, but log detailed information
4. **Use User ID**: Pass the current user ID when available for better tracking
5. **Silent Failures**: The logging service swallows its own exceptions to prevent cascading failures

## Folder Structure

```
TheBeautyHubData/
├── Repositories/
│   ├── Interfaces/          ← Repository interfaces moved here
│   │   └── IExceptionLogRepository.cs
│   └── ExceptionLogRepository.cs
│
TheBeautyHubCore/
├── Services/
│   ├── Interfaces/          ← Service interfaces moved here
│   │   └── IExceptionLogService.cs
│   └── ExceptionLogService.cs
│
TheBeautyHubAPI/
├── Controllers/
│   └── (No ExceptionLogsController)  ← Removed
```

## Database Schema

Exception logs are stored in the `ExceptionLogs` table with the following structure:
- `Id` (bigint, PK, auto-increment)
- `Type` (nvarchar(100), required)
- `ErrorMessage` (nvarchar(max), required)
- `StackTrace` (nvarchar(max), nullable)
- `InnerException` (nvarchar(max), nullable)
- `AdditionalInfo` (nvarchar(1000), nullable)
- `DeviceName` (nvarchar(100), nullable)
- `UserId` (uniqueidentifier, nullable, FK to Users)
- `CreatedAt` (datetime2, auto-generated)
