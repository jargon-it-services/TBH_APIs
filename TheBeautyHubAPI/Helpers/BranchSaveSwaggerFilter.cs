using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TheBeautyHubAPI.Controllers;

namespace TheBeautyHubAPI.Helpers;

/// <summary>
/// Swagger fills optional uuid fields (service_id) with a sample GUID.
/// Hide those aliases so create/update only show the optional services list.
/// </summary>
public sealed class BranchSaveSwaggerFilter : IOperationFilter
{
    private static readonly string[] HiddenFormFields =
    {
        "account_id",
        "service_id",
        "service_ids"
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType != typeof(BranchesController))
            return;

        if (operation.Parameters != null)
        {
            operation.Parameters = operation.Parameters
                .Where(p => !HiddenFormFields.Contains(p.Name))
                .ToList();
        }

        if (operation.RequestBody?.Content == null)
            return;

        foreach (var content in operation.RequestBody.Content.Values)
        {
            var schema = content.Schema;
            if (schema?.Properties == null)
                continue;

            foreach (var name in HiddenFormFields)
                schema.Properties.Remove(name);

            if (content.Encoding != null)
            {
                foreach (var name in HiddenFormFields)
                    content.Encoding.Remove(name);
            }
        }
    }
}
