using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TheBeautyHubAPI.Controllers;

namespace TheBeautyHub.Tests;

public class ApiEndpointCatalogTests
{
    public static IReadOnlyList<string> Discover()
    {
        var assembly = typeof(BranchesController).Assembly;
        var routes = new SortedSet<string>(StringComparer.Ordinal);

        foreach (var type in assembly.GetTypes())
        {
            if (!typeof(ControllerBase).IsAssignableFrom(type) || type.IsAbstract)
                continue;

            var prefix = type.GetCustomAttribute<RouteAttribute>()?.Template ?? string.Empty;
            prefix = prefix.Replace("[controller]", type.Name.EndsWith("Controller", StringComparison.Ordinal)
                ? type.Name[..^"Controller".Length]
                : type.Name, StringComparison.OrdinalIgnoreCase);

            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                var httpAttrs = method.GetCustomAttributes(true).OfType<HttpMethodAttribute>().ToList();
                if (httpAttrs.Count == 0)
                    continue;

                var actionRoute = method.GetCustomAttribute<RouteAttribute>()?.Template;
                foreach (var http in httpAttrs)
                {
                    var template = http.Template ?? actionRoute;
                    var verb = http.HttpMethods.First();
                    routes.Add($"{verb} {Combine(prefix, template)}");
                }
            }
        }

        return routes.ToList();
    }

    private static string Combine(string prefix, string? template)
    {
        prefix = prefix.Trim('/');
        if (string.IsNullOrWhiteSpace(template))
            return "/" + prefix;
        return "/" + prefix + "/" + template.Trim('/');
    }

    /// <summary>
    /// Live HTTP surface. Disabled (#if false) controllers are not compiled and must not appear here.
    /// Add a line when you ship a new route so the suite fails until the catalog is updated.
    /// </summary>
    public static readonly string[] Expected =
    [
        "GET /api/branches",
        "GET /api/branches/{branchId:guid}/details",
        "GET /api/expenses/list",
        "GET /api/expenses/{expenseId:guid}/details",
        "GET /api/management/account-summary",
        "GET /api/management/feature-lock",
        "GET /api/salary-rules",
        "GET /api/salary-rules/list",
        "GET /api/salary-rules/{ruleId:guid}/details",
        "GET /api/services",
        "GET /api/services/list",
        "GET /api/services/{serviceId:guid}/details",
        "GET /api/staff/form-config",
        "GET /api/staff/list",
        "GET /api/staff/next-employee-code",
        "GET /api/staff/{userId:guid}/details",
        "GET /api/token/validate",
        "GET /api/transactions",
        "GET /api/transactions/bootstrap",
        "GET /api/transactions/{id}",
        "POST /api/branches",
        "POST /api/branches/{branchId:guid}",
        "POST /api/expenses",
        "POST /api/expenses/{expenseId:guid}",
        "POST /api/expenses/{expenseId:guid}/delete",
        "POST /api/salary-rules",
        "POST /api/salary-rules/{ruleId:guid}",
        "POST /api/salary-rules/{ruleId:guid}/delete",
        "POST /api/services",
        "POST /api/services/{serviceId:guid}",
        "POST /api/services/{serviceId:guid}/delete",
        "POST /api/staff",
        "POST /api/staff/{userId:guid}",
        "POST /api/staff/{userId:guid}/delete",
        "POST /api/transactions",
        "POST /api/transactions/{id}/mark-paid",
        "PUT /api/transactions/{id}"
    ];

    [Fact]
    public void Live_api_routes_match_catalog()
    {
        var actual = Discover();
        Assert.Equal(Expected, actual);
    }

    [Fact]
    public void Every_compiled_controller_is_in_the_catalog()
    {
        var controllers = typeof(BranchesController).Assembly.GetTypes()
            .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract)
            .Select(t => t.Name)
            .OrderBy(n => n)
            .ToArray();

        Assert.Equal(
            new[]
            {
                "BranchesController",
                "ExpensesTypesController",
                "ManagementController",
                "SalaryRulesController",
                "ServicesController",
                "StaffController",
                "TokenController",
                "TransactionsController"
            },
            controllers);
    }
}
