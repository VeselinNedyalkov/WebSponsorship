﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SponsorY.ModelBinders
{
    public class DoubleModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(double) || context.Metadata.ModelType == typeof(double?))
            {
                return new DoubleModelBinder();
            }

            return null;
        }
    }
}
