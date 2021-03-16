using AutomatedCar.KeyboardHandling;
using NUnit.Framework;
using System.Windows.Input;

namespace Test.KeyboardHandling
{
    public class HoldableKeyTest
    {
        private HoldableKey holdableKey;

        [SetUp]
        public void SetUp()
        {
            holdableKey = new HoldableKey(Key.A, "Cat", "Con", (dur) => { }, (dur) => { });
        }

        [Test]
        public void Key_IsEqualToA()
        {
            Assert.That(holdableKey.Key, Is.EqualTo(Key.A));
        }

        [Test]
        public void Category_IsEqualToCat()
        {
            Assert.That(holdableKey.Category, Is.EqualTo("Cat"));
        }

        [Test]
        public void Control_IsEqualToCon()
        {
            Assert.That(holdableKey.Control, Is.EqualTo("Con"));
        }

        [Test]
        public void OnHold_IsNotNull()
        {
            Assert.That(holdableKey.OnHold, Is.Not.Null);
        }

        [Test]
        public void OnIdle_IsNotNull()
        {
            Assert.That(holdableKey.OnIdle, Is.Not.Null);
        }
    }
}
