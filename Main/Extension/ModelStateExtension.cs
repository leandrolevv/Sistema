using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Main.Extension
{
    public static class ModelStateExtension
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            List<string> errors = new List<string>();
            foreach (var values in modelState.Values)
            {
                //values.Errors.Select(x => x.ErrorMessage);
                errors.AddRange(values.Errors.Select(x => x.ErrorMessage));
            }

            return errors;
        }
    }
}
