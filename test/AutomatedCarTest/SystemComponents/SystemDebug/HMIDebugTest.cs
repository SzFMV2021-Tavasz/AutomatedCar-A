using AutomatedCar.SystemComponents.SystemDebug;
using NUnit.Framework;

namespace Test.SystemComponents.SystemDebug
{
    public class HMIDebugTest
    {
        private HMIDebug hmiDebug;

        [SetUp]
        public void SetUp()
        {
            hmiDebug = new HMIDebug();
        }

        [Test]
        public void DebugRadarChangesToTrue()
        {
            bool val = false;
            HMIDebug.DebugActionEventHandler += (s, e) => val = e.DebugRadar;

            hmiDebug.OnDebugAction(1);

            Assert.That(val, Is.True);
        }

        [Test]
        public void DebugRadarChangesToFalse()
        {
            bool val = true;
            HMIDebug.DebugActionEventHandler += (s, e) => val = e.DebugRadar;

            hmiDebug.OnDebugAction(1);
            hmiDebug.OnDebugAction(1);

            Assert.That(val, Is.False);
        }

        [Test]
        public void DebugSonicChangesToTrue()
        {
            bool val = false;
            HMIDebug.DebugActionEventHandler += (s, e) => val = e.DebugSonic;

            hmiDebug.OnDebugAction(2);

            Assert.That(val, Is.True);
        }

        [Test]
        public void DebugSonicChangesToFalse()
        {
            bool val = true;
            HMIDebug.DebugActionEventHandler += (s, e) => val = e.DebugSonic;

            hmiDebug.OnDebugAction(2);
            hmiDebug.OnDebugAction(2);

            Assert.That(val, Is.False);
        }

        [Test]
        public void DebugVideoChangesToTrue()
        {
            bool val = false;
            HMIDebug.DebugActionEventHandler += (s, e) => val = e.DebugVideo;

            hmiDebug.OnDebugAction(3);

            Assert.That(val, Is.True);
        }

        [Test]
        public void DebugVideoChangesToFalse()
        {
            bool val = true;
            HMIDebug.DebugActionEventHandler += (s, e) => val = e.DebugVideo;

            hmiDebug.OnDebugAction(3);
            hmiDebug.OnDebugAction(3);

            Assert.That(val, Is.False);
        }

        [Test]
        public void DebugPolysChangesToTrue()
        {
            bool val = false;
            HMIDebug.DebugActionEventHandler += (s, e) => val = e.DebugPolys;

            hmiDebug.OnDebugAction(4);

            Assert.That(val, Is.True);
        }

        [Test]
        public void DebugPolysChangesToFalse()
        {
            bool val = true;
            HMIDebug.DebugActionEventHandler += (s, e) => val = e.DebugPolys;

            hmiDebug.OnDebugAction(4);
            hmiDebug.OnDebugAction(4);

            Assert.That(val, Is.False);
        }
    }
}
