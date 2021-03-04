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
        private int testInt3;

        [SetUp]
        public void SetUp()
        {
            keyboardHandler = new KeyboardHandler(20);

            PressableKey key1 = new PressableKey(Key.A, () => testInt1++);
            PressableKey key2 = new PressableKey(Key.D, () => testInt1--);

            HoldableKey key3 = new HoldableKey(Key.Q, (x) => testInt2 += 2, (x) => testInt2 -= 2);
            HoldableKey key4 = new HoldableKey(Key.E, (x) => testInt3 += 4, (x) => testInt3 -= 4);

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
            testInt3 = 0;
            keyboardHandler.Tick();
            keyboardHandler.Tick();
            keyboardHandler.Tick();

            Assert.That(testInt3, Is.EqualTo(-12));
        }

        [Test]
        public void CanHoldDownMultipleKeys()
        {
            testInt1 = 0;
            testInt2 = 0;
            testInt3 = 0;
            keyboardHandler.OnKeyDown(Key.A);
            keyboardHandler.OnKeyDown(Key.D);
            keyboardHandler.OnKeyDown(Key.Q);
            keyboardHandler.OnKeyDown(Key.E);
            keyboardHandler.Tick();
            keyboardHandler.Tick();

            Assert.That(testInt1, Is.EqualTo(0));
            Assert.That(testInt2, Is.EqualTo(4));
            Assert.That(testInt3, Is.EqualTo(8));
        }

        [Test]
        public void HoldableKeyDurationIsCorrectAfterSeveralTicksWhenHolding()
        {
            keyboardHandler.OnKeyDown(Key.Q);
            keyboardHandler.Tick();
            keyboardHandler.Tick();
            keyboardHandler.Tick();

            Assert.That(keyboardHandler.HoldableKeys[0].CurrentStateDuration, Is.EqualTo(0.06));
        }

        [Test]
        public void HoldableKeyDurationIsCorrectAfterSeveralTicksWhenIdle()
        {
            keyboardHandler.Tick();
            keyboardHandler.Tick();
            keyboardHandler.Tick();
            keyboardHandler.Tick();

            Assert.That(keyboardHandler.HoldableKeys[1].CurrentStateDuration, Is.EqualTo(0.08));
        }

        [Test]
        public void HoldableKeyDurationResetsAfterStateChange()
        {
            keyboardHandler.OnKeyDown(Key.Q);
            keyboardHandler.Tick();
            keyboardHandler.Tick();
            keyboardHandler.OnKeyUp(Key.Q);

            Assert.That(keyboardHandler.HoldableKeys[0].CurrentStateDuration, Is.EqualTo(0));
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
