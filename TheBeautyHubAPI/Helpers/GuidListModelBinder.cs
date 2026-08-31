using Microsoft.AspNetCore.Mvc.ModelBinding;
using TheBeautyHubCore.Parsing;

namespace TheBeautyHubAPI.Helpers;

public sealed class GuidListModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var result = context.ValueProvider.GetValue(context.FieldName);
        if (result == ValueProviderResult.None)
        {
            context.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        var parsed = GuidListParser.ParseMany(result.Values.Select(v => v));
        context.Result = ModelBindingResult.Success(parsed);
        return Task.CompletedTask;
    }
}
