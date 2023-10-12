using System.ComponentModel;

namespace Lensophy.Extension;

/// <summary>
/// Extensions for the <see cref="Enum"/> type."
/// </summary>
internal static class EnumExtension
{
    /// <summary>
    /// Get the description of the enumerator.
    /// </summary>
    /// <param name="value">The enumerator.</param>
    /// <returns><para>The description defined through the <see cref="DescriptionAttribute"/>. If it hasn't been decorated
    /// with the attribute, its literal name will be considered. If the <b>value</b> is not part of the items defined
    /// in the enumerator, the same value is returned.</para>
    /// <para>This extension does not throw an exception for non-existent enumerators. See the remark for further
    /// clarification.</para></returns>
    /// <exception cref="ArgumentNullException">If <b>value</b> is null.</exception>
    /// <remarks>
    /// <para>Please note that, during an enumerator cast, by design, C# does not throw an exception if the numeric value
    /// is not part of the enumerator. It preserves the received number.</para>
    /// <para>As this behavior can lead to inappropriate scenarios, always ensure to evaluate whether the transformation
    /// was successful using <see cref="Enum.IsDefined"/> in combination with <see cref="Enum.TryParse{TEnum}(string,out TEnum)"/>
    /// before using this extension.</para>
    /// <para>This extension is not intended to address this particularity.</para>
    /// </remarks>
    internal static string GetDescription(this Enum value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        var attributeExist = value
            .GetType()
            .GetField(value.ToString())?
            .GetCustomAttributes(typeof(DescriptionAttribute), false); 

        if (attributeExist is DescriptionAttribute[] { Length: > 0 } attributes)
        {
            return attributes[0].Description;
        }

        return value.ToString();
    }
}