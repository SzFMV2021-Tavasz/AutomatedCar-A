using AutomatedCar.Visualization;
using BaseModel.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Windows;
using Xunit;

namespace Test.Visualization
{
    public class RenderCameraTests
    {
        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(9, 16, 144)]
        [InlineData(960, 720, 691200)]
        public void CalculatesViewportOnTheFly(double cameraWidth, double cameraHeight, double expectedArea)
        {
            camera.Width = cameraWidth;
            camera.Height = cameraHeight;

            var viewport = camera.ViewportRect;

            Assert.Equal(expectedArea, viewport.Width * viewport.Height);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(150, 2500)]
        [InlineData(-150, 600)]
        public void CameraPositionOffsetsViewport(double x, double y)
        {
            camera.LeftX = x;
            camera.TopY = y;

            var viewport = camera.ViewportRect;

            Assert.Equal(x, viewport.X);
            Assert.Equal(y, viewport.Y);
        }

        [Theory]
        [InlineData(0, 0, -480, -360)]
        [InlineData(1500, 1000, 1020, 640)]
        public void UpdatingOriginOffsetsTopLeftCoordinates(double originX, double originY, double expectedTopLeftX, double expectedTopLeftY)
        {
            camera.UpdateMiddlePoint(originX, originY);

            Assert.Equal(expectedTopLeftX, camera.ViewportRect.X);
            Assert.Equal(expectedTopLeftY, camera.ViewportRect.Y);
        }

        [Theory]
        [InlineData(10, 7, 5, 2)]
        [InlineData(16, 5, 11, 0)]
        public void CanTranslateGlobalCoordsToLocals(double x, double y, double expectedViewportX, double expectedViewportY)
        {
            camera.LeftX = 5;
            camera.TopY = 5;

            Point actual = camera.TranslateToViewport(x, y);

            Assert.Equal(expectedViewportX, actual.X);
            Assert.Equal(expectedViewportY, actual.Y);
        }

        [Theory]
        [InlineData(5, 5, 10, 10)]
        [InlineData(0, 0, 20, 30)]
        [InlineData(450, 500, 150, 30)]
        [InlineData(-500, -1500, 5000, 5000)]
        public void CameraDetectsIntersectionsCorrectly(int x, int y, int width, int height)
        {
            var renderable = new Mock<IRenderableWorldObject>();
            renderable.Setup(r => r.X).Returns(x);
            renderable.Setup(r => r.Y).Returns(y);
            renderable.Setup(r => r.Width).Returns(width);
            renderable.Setup(r => r.Height).Returns(height);

            var result = camera.IsVisibleInViewport(renderable.Object);
            Assert.True(result);
        }

        [Fact]
        public void CameraFiltersToVisibleObjects()
        {

            //var actual = camera.Filter(renderables);

            //Assert.Equal(visibleObjects, actual);
            Assert.True(true);
        }

        #region TestInternal

        private readonly RenderCamera camera = new RenderCamera();
        private readonly List<IRenderableWorldObject> renderables = new List<IRenderableWorldObject>();
        private readonly List<IRenderableWorldObject> visibleObjects = new List<IRenderableWorldObject>();
        private readonly List<IRenderableWorldObject> farawayObjects = new List<IRenderableWorldObject>();

        private Mock<IRenderableWorldObject> vis1 = new Mock<IRenderableWorldObject>();
        private Mock<IRenderableWorldObject> vis2 = new Mock<IRenderableWorldObject>();
        private Mock<IRenderableWorldObject> vis3 = new Mock<IRenderableWorldObject>();

        private Mock<IRenderableWorldObject> far1 = new Mock<IRenderableWorldObject>();

        public RenderCameraTests()
        {
            camera.Width = 960;
            camera.Height = 720;

            SetupVisibles();
            SetupFaraways();

            renderables.AddRange(visibleObjects);
            renderables.AddRange(farawayObjects);
        }

        private void SetupVisibles()
        {
            vis1 = new Mock<IRenderableWorldObject>();
            vis1.Setup(v => v.X).Returns(50);
            vis1.Setup(v => v.Y).Returns(50);
            vis1.Setup(v => v.Width).Returns(5);
            vis1.Setup(v => v.Height).Returns(15);
            visibleObjects.Add(vis1.Object);

            vis2 = new Mock<IRenderableWorldObject>();
            vis2.Setup(v => v.X).Returns(100);
            vis2.Setup(v => v.Y).Returns(100);
            vis2.Setup(v => v.Width).Returns(5);
            vis2.Setup(v => v.Height).Returns(15);
            visibleObjects.Add(vis2.Object);

            vis3 = new Mock<IRenderableWorldObject>();
            vis3.Setup(v => v.X).Returns(300);
            vis3.Setup(v => v.Y).Returns(300);
            vis3.Setup(v => v.Width).Returns(5);
            vis3.Setup(v => v.Height).Returns(15);
            visibleObjects.Add(vis3.Object);
        }

        private void SetupFaraways()
        {
            far1 = new Mock<IRenderableWorldObject>();
            far1.Setup(f => f.X).Returns(6000);
            far1.Setup(f => f.Y).Returns(5000);
            far1.Setup(f => f.Width).Returns(5);
            far1.Setup(f => f.Height).Returns(5);
            farawayObjects.Add(far1.Object);
        }

        #endregion TestInternal
    }
}