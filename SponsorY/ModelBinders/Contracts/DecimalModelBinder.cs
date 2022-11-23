using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace SponsorY.ModelBinders.Contracts
{
	public class DecimalModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (valueResult != ValueProviderResult.None && !string.IsNullOrEmpty(valueResult.FirstValue))
			{
				decimal value = 0m;
				bool success = false;
				try
				{
					string decimalValue = valueResult.FirstValue;
					decimalValue = decimalValue.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					decimalValue = decimalValue.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					value = Convert.ToDecimal(decimalValue, CultureInfo.CurrentCulture);
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
