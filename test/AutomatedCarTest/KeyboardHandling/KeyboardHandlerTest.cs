using AutomatedCar.KeyboardHandling;
using NUnit.Framework;
using System;
using System.Windows.Input;

namespace AutomatedCarTest.KeyboardHandling
{
    public class KeyboardHandlerTest
    {
        private KeyboardHandler keyboardHandler;
        private int testInt1;
        private int testInt2;
        private double testDouble;

        [SetUp]
        public void SetUp()
        {
            keyboardHandler = new KeyboardHandler(20);

            PressableKey key1 = new PressableKey(Key.A, () => testInt1++);
            PressableKey key2 = new PressableKey(Key.D, () => testInt1--);

            HoldableKey key3 = new HoldableKey(Key.Q, (x) => testInt2 += 2, (x) => testInt2 -= 2);
            HoldableKey key4 = new HoldableKey(Key.E, (x) => testDouble += x, (x) => testDouble -= x);

            keyboardHandler.PressableKeys.Add(key1);
            keyboardHandler.PressableKeys.Add(key2);

            keyboardHandler.HoldableKeys.Add(key3);
            keyboardHandler.HoldableKeys.Add(key4);
        }

        [Test]
        public void PressableKeyOnPressEventIsCalledCorrectlyFromKeyboardHandler()
        {
            testInt1 = 0;
            keyboardHandler.OnKeyDown(Key.A);
            keyboardHandler.OnKeyDown(Key.D);
            keyboardHandler.OnKeyDown(Key.A);

            Assert.That(testInt1, Is.EqualTo(1));
        }

        [Test]
        public void HoldableKeyOnHoldEventIsCalledAfterEveryTickFromKeyboardHandlerIfKeyIsPressedAndNotReleased()
        {
            testInt2 = 0;
            keyboardHandler.OnKeyDown(Key.Q);
            keyboardHandler.Tick();
            keyboardHandler.Tick();
            keyboardHandler.Tick();
            keyboardHandler.Tick();
            keyboardHandler.Tick();

            Assert.That(testInt2, Is.EqualTo(10));
        }

        [Test]
        public void HoldableKeyOnIdleEventIsCalledAfterEveryTickFromKeyboardHandlerIfKeyIsNotPressed()
        {
            testDouble = 0;
            keyboardHandler.Tick();
            keyboardHandler.Tick();
            keyboardHandler.Tick();

            Assert.That(testDouble, Is.LessThan(-0.02));
        }

        [Test]
        public void CanHoldDownMultipleKeys()
        {
            testInt1 = 0;
            testInt2 = 0;
            testDouble = 0;
            keyboardHandler.OnKeyDown(Key.A);
            keyboardHandler.OnKeyDown(Key.D);
            keyboardHandler.OnKeyDown(Key.Q);
            keyboardHandler.OnKeyDown(Key.E);
            keyboardHandler.Tick();
            keyboardHandler.Tick();

            Assert.That(testInt1, Is.EqualTo(0));
            Assert.That(testInt2, Is.GreaterThan(0));
            Assert.That(testDouble, Is.GreaterThan(0));
        }

        [Test]
        public void HoldableKeyTickDurationIsCorrectWhenHolding()
        {
            testDouble = 0;
            keyboardHandler.OnKeyDown(Key.E);
            keyboardHandler.Tick();

            Assert.That(testDouble, Is.EqualTo(0.02));
        }

        [Test]
        public void HoldableKeyIsBeingHeldAfterPress()
        {
            keyboardHandler.OnKeyDown(Key.Q);

            Assert.That(keyboardHandler.HoldableKeys[0].IsBeingHeld, Is.True);
        }

        [Test]
        public void HoldableKeyIsNotBeingHeldAfterRelease()
        {
            keyboardHandler.OnKeyDown(Key.Q);
            keyboardHandler.OnKeyUp(Key.Q);

            Assert.That(keyboardHandler.HoldableKeys[0].IsBeingHeld, Is.False);
        }
    }
}
