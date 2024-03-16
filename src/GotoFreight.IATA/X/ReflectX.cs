using System.Reflection;

namespace GotoFreight.IATA.X;

public class ReflectX
{
    public static void FixNullValue(object myObject, Type fieldType, object fieldValue)
    {
        var type = myObject.GetType();
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            if (field.FieldType == fieldType && field.GetValue(myObject) == null)
            {
                field.SetValue(myObject, fieldValue);
            }
        }
    }
}