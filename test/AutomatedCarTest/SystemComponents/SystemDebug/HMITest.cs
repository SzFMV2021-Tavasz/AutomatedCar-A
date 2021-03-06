using AutomatedCar.SystemComponents.SystemDebug;
using NUnit.Framework;

namespace Test.SystemComponents.SystemDebug
{
    public class HMITest
    {
        private HMI hmi;
        private bool debugRadar;
        private bool debugSonic;
        private bool debugVideo;
        private bool debugPolys;

        [SetUp]
        public void SetUp()
        {
            hmi = new HMI();
            HMI.DebugActionEventHandler += (s, e) =>
            {
                debugRadar = e.DebugRadar;
                debugSonic = e.DebugSonic;
                debugVideo = e.DebugVideo;
                debugPolys = e.DebugPolys;
            };
        }

        [Test]
        public void DebugRadarChangesToTrue()
        {
            bool val = false;
            HMI.DebugActionEventHandler += (s, e) => val = debugRadar;

            hmi.OnDebugAction(1);

            Assert.That(val, Is.True);
        }

        [Test]
        public void DebugRadarChangesToFalse()
        {
            bool val = true;
            HMI.DebugActionEventHandler += (s, e) => val = debugRadar;

            hmi.OnDebugAction(1);
            hmi.OnDebugAction(1);

            Assert.That(val, Is.False);
        }

        [Test]
        public void DebugSonicChangesToTrue()
        {
            bool val = false;
            HMI.DebugActionEventHandler += (s, e) => val = debugSonic;

            hmi.OnDebugAction(2);

            Assert.That(val, Is.True);
        }

        [Test]
        public void DebugSonicChangesToFalse()
        {
            bool val = true;
            HMI.DebugActionEventHandler += (s, e) => val = debugSonic;

            hmi.OnDebugAction(2);
            hmi.OnDebugAction(2);

            Assert.That(val, Is.False);
        }

        [Test]
        public void DebugVideoChangesToTrue()
        {
            bool val = false;
            HMI.DebugActionEventHandler += (s, e) => val = debugVideo;

            hmi.OnDebugAction(3);

            Assert.That(val, Is.True);
        }

        [Test]
        public void DebugVideoChangesToFalse()
        {
            bool val = true;
            HMI.DebugActionEventHandler += (s, e) => val = debugVideo;

            hmi.OnDebugAction(3);
            hmi.OnDebugAction(3);

            Assert.That(val, Is.False);
        }

        [Test]
        public void DebugPolysChangesToTrue()
        {
            bool val = false;
            HMI.DebugActionEventHandler += (s, e) => val = debugPolys;

            hmi.OnDebugAction(4);

            Assert.That(val, Is.True);
        }

        [Test]
        public void DebugPolysChangesToFalse()
        {
            bool val = true;
            HMI.DebugActionEventHandler += (s, e) => val = debugPolys;

            hmi.OnDebugAction(4);
            hmi.OnDebugAction(4);

            Assert.That(val, Is.False);
        }
    }
}
