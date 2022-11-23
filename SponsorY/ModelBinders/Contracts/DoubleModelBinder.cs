using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace SponsorY.ModelBinders.Contracts
{
	public class DoubleModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (valueResult != ValueProviderResult.None && !string.IsNullOrEmpty(valueResult.FirstValue))
			{
				double value = 0;
				bool success = false;
				try
				{
					string doubleValue = valueResult.FirstValue;
					doubleValue = doubleValue.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					doubleValue = doubleValue.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					value = Convert.ToDouble(doubleValue, CultureInfo.CurrentCulture);
					success = true;
				}
				catch (FormatException e)
				{
					bindingContext.ModelState.AddModelError(bindingContext.ModelName, e, bindingContext.ModelMetadata);
				}

				if (success)
				{
					bindingContext.Result = ModelBindingResult.Success(value);
				}
			}
			return Task.CompletedTask;
		}
	}
}
