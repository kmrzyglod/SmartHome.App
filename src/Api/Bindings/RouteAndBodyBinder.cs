using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace SmartHome.Api.Bindings
{
    ///Important - add [FromBody] attribute before [ModelBinder(typeof(RouteAndBodyBinder))] attribute in controller source code because custom model binder is not working properly with swagger !!!
    public class RouteAndBodyBinder : IModelBinder
    {
        private readonly BodyModelBinder defaultBinder;

        public RouteAndBodyBinder(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory)
        {
            defaultBinder = new BodyModelBinder(formatters, readerFactory);
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            await defaultBinder.BindModelAsync(bindingContext);

            if (bindingContext.Result.IsModelSet)
            {
                var fromRouteProperties = bindingContext.Result.Model.GetType()
                    .GetProperties()
                    .Where(x => x.CustomAttributes.Any(z => z.AttributeType == typeof(FromRouteAttribute)));
                var data = bindingContext.Result.Model;

                foreach (var property in fromRouteProperties)
                {
                    var value = bindingContext.ValueProvider.GetValue(property.Name)
                        .FirstValue;
                    var parsedValue = Convert.ChangeType(value, property.PropertyType);
                    data.GetType()
                        .GetProperty(property.Name)
                        ?.SetValue(data, parsedValue);
                }

                bindingContext.Result = ModelBindingResult.Success(data);
            }
        }
    }
}
