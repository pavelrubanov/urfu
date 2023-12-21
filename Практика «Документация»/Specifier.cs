using System;
using System.Linq;
using System.Reflection;

namespace Documentation
{
    public class Specifier<T> : ISpecifier
    {
        private Type _type = typeof(T);

        public string GetApiDescription()
        {
            return _type.GetCustomAttributes<ApiDescriptionAttribute>().FirstOrDefault()?.Description;
        }

        public string[] GetApiMethodNames()
        {
            return _type.GetMethods().Where(m =>
            m.GetCustomAttributes(true).OfType<ApiMethodAttribute>().Any())
            .Select(m => m.Name)
            .ToArray();
        }

        public string GetApiMethodDescription(string methodName)
        {
            return GetMethod(methodName)?.GetCustomAttributes<ApiDescriptionAttribute>().FirstOrDefault()?.Description;
        }

        public string[] GetApiMethodParamNames(string methodName)
        {
            return GetMethod(methodName)?.GetParameters().Select(p => p.Name).ToArray();
        }

        public string GetApiMethodParamDescription(string methodName, string paramName)
        {
            var param = GetMethod(methodName)?.GetParameters().FirstOrDefault(p => p.Name == paramName);
            return param?.GetCustomAttributes<ApiDescriptionAttribute>().FirstOrDefault()?.Description;
        }

        public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
        {
            var paramDesc = new ApiParamDescription();
            paramDesc.ParamDescription = new CommonDescription(paramName);
            var parameter = GetMethod(methodName)?.GetParameters().Where(p => p.Name == paramName);
            if (parameter == null || !parameter.Any())
                return paramDesc;
            return GetParamDescription(parameter.First(), paramDesc);
        }

        public ApiMethodDescription GetApiMethodFullDescription(string methodName)
        {
            var fullDesc = new ApiMethodDescription();
            var method = GetMethod(methodName);
            if (method?.GetCustomAttribute<ApiMethodAttribute>() == null)
                return null;
            fullDesc.MethodDescription = new CommonDescription(methodName);
            fullDesc.ParamDescriptions = method.GetParameters()
            .Select(p => GetApiMethodParamFullDescription(methodName, p.Name))
            .ToArray();
            fullDesc.MethodDescription.Description =
            method.GetCustomAttributes<ApiDescriptionAttribute>().FirstOrDefault()?.Description;
            var returnParam = method.ReturnParameter;
            if (returnParam.GetCustomAttributes<ApiIntValidationAttribute>().FirstOrDefault() ==
            null && returnParam.GetCustomAttributes<ApiRequiredAttribute>().FirstOrDefault() == null)
                return fullDesc;
            var paramDescription = new ApiParamDescription();
            paramDescription.ParamDescription = new CommonDescription();
            fullDesc.ReturnDescription = GetParamDescription(returnParam, paramDescription);
            return fullDesc;
        }

        public ApiParamDescription GetParamDescription(ParameterInfo parameter, ApiParamDescription paramDesc)
        {
            var desc = parameter.GetCustomAttributes<ApiDescriptionAttribute>().FirstOrDefault();
            if (desc != null)
                paramDesc.ParamDescription.Description = desc.Description;
            var intValidationAttribute = parameter.GetCustomAttributes<ApiIntValidationAttribute>().FirstOrDefault();
            if (intValidationAttribute != null)
            {
                paramDesc.MinValue = intValidationAttribute.MinValue;
                paramDesc.MaxValue = intValidationAttribute.MaxValue;
            }
            var requiredAttribute = parameter.GetCustomAttributes<ApiRequiredAttribute>().FirstOrDefault();
            if (requiredAttribute != null)
                paramDesc.Required = requiredAttribute.Required;
            return paramDesc;
        }

        public MethodInfo GetMethod(string methodName)
        {
            return _type.GetMethod(methodName);
        }
    }
}
