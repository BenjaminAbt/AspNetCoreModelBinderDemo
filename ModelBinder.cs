using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace AspNetCoreModelBinderDemo
{
    public class MyPoco
    {
        public MyPoco(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public String Name { get; }
    }


    public class MyPocoModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }


            if (context.Metadata.ModelType == typeof(MyPoco))
            {
                return new BinderTypeModelBinder(typeof(MyPocoModelBinder));
            }
            else
            {
                return null;
            }
        }
    }

    public static class MyPocoRepository
    {
        public static List<MyPoco> Pocos = new List<MyPoco> {
            new MyPoco(1, "Name")
        };
    }


    public class MyPocoModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // Specify a default argument name if none is set by ModelBinderAttribute
            string modelName = bindingContext.BinderModelName;
            if (string.IsNullOrEmpty(modelName))
            {
                modelName = "id";
            }

            // Try to fetch the value of the argument by name
            var valueProviderResult =
                bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult != ValueProviderResult.None)
            {
                bindingContext.ModelState.SetModelValue(modelName,
                    valueProviderResult);

                string value = valueProviderResult.FirstValue;

                // Check if the argument value is null or empty
                if (!string.IsNullOrEmpty(value))
                {
                    if (!int.TryParse(value, out int id))
                    {
                        bindingContext.ModelState.TryAddModelError(modelName, "Id must be an int.");
                    }
                    else
                    {
                        MyPoco poco = MyPocoRepository.Pocos.SingleOrDefault(_ => _.Id == id);
                        bindingContext.Result = ModelBindingResult.Success(poco);
                    }
                }
            }

        }
    }
}
