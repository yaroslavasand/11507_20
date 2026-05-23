using System.Reflection;

public class MethodExecutor
{
    public object Execute(object obj, string methodName)
    {
        MethodInfo method = obj.GetType().GetMethod(methodName);
        ParameterInfo[] parameters = method.GetParameters();
        
        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(int))
        {
            int countValue = (int)obj.GetType().GetProperty("Count").GetValue(obj);
            return method.Invoke(obj, new object[] { countValue });
        }
        
        return method.Invoke(obj, null);
    }
}