using AutomatedCar.Controls;
using NUnit.Framework;
using System.Windows.Input;

namespace AutomatedCarTest.Controls
{
    public class PressableKeyTest
    {
        private PressableKey pressableKey;

        [SetUp]
        public void SetUp()
        {
            pressableKey = new PressableKey(Key.B, "Ca", "Co", () => { });
        }

        [Test]
        public void Key_IsEqualToA()
        {
            Assert.That(pressableKey.Key, Is.EqualTo(Key.B));
        }

        [Test]
        public void Category_IsEqualToCa()
        {
            Assert.That(pressableKey.Category, Is.EqualTo("Ca"));
        }

        [Test]
        public void Control_IsEqualToCo()
        {
            Assert.That(pressableKey.Control, Is.EqualTo("Co"));
        }

        [Test]
        public void OnPress_IsNotNull()
        {
            Assert.That(pressableKey.OnPress, Is.Not.Null);
        }
    }
}
