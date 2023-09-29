using System.ComponentModel;

namespace Lensophy.UnitTest.Extension;

public enum Philosophers
{
    [Description("None")]
    None,
    Socrates,
    [Description("Buddha")]
    SiddharthaGautama,
    Confucius,
    Saitama
}