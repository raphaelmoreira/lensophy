using FluentAssertions;
using Xunit;
using Lensophy.Domain.Extension;

namespace Lensophy.UnitTest.Extension;

public class EnumExtensionTest
{
    [Fact]
    [Trait("Extension", "GetDescription")]
    public void Extension_Enum_GetDescription_Should_ThrowArgumentNullException()
    {
        //arranges
        Philosophers? nullPhilosophers = null;

        //act
        var exception = () => nullPhilosophers!.GetDescription();

        //assert
        exception.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    [Trait("Extension", "GetDescription")]
    public void Extension_Enum_GetDescription_ShouldBe_DefaultDescription()
    {
        //arranges
        const Philosophers philosophersWithoutDescription = Philosophers.Socrates;
        const string philosophersDefaultDescriptionExpected = nameof(Philosophers.Socrates);

        //act
        var philosophersDescription = philosophersWithoutDescription.GetDescription();

        //assert
        philosophersDescription.Should().Be(philosophersDefaultDescriptionExpected);
    }
}