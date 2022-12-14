using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using Xunit;

namespace SpaceBattle.Lib.Test {
    public class CollisionCheckCommandTests
    {
        public CollisionCheckCommandTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            var regStrategy = new Mock<IStrategy>();
            regStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns(new List<int>());

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Collision.GetList", (object[] args) => regStrategy.Object.Execute(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Collision.GetDeltas", (object[] args) => new GetDeltasStrategy().Execute(args)).Execute();
        }

        [Fact]
        public void CollisionCheckTrue()
        {
            var uObject1 = new Mock<IUObject>();
            var uObject2 = new Mock<IUObject>();

            var checkReturns = new Mock<IStrategy>();
            checkReturns.Setup(c => c.Execute(It.IsAny<object[]>())).Returns((object) true);
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Collision.CheckWithTree", (object[] args) => checkReturns.Object.Execute(args)).Execute();
            
            ICommand collisionCheck = new CollisionCheck(uObject1.Object, uObject2.Object);
            Assert.ThrowsAny<Exception>(() => collisionCheck.Execute());
        }

        [Fact]
        public void CollisionCheckFalse()
        {   

            var uObject1 = new Mock<IUObject>();
            var uObject2 = new Mock<IUObject>();

            var checkReturns = new Mock<IStrategy>();
            checkReturns.Setup(c => c.Execute(It.IsAny<object[]>())).Returns((object) false);
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Collision.CheckWithTree", (object[] args) => checkReturns.Object.Execute(args)).Execute();
            
            ICommand collisionCheck = new CollisionCheck(uObject1.Object, uObject2.Object);
            collisionCheck.Execute();
        }

        [Fact]
        public void CollisionCheckNull()
        {
            var uObject1 = new Mock<IUObject>();
            var uObject2 = new Mock<IUObject>();

            var checkReturns = new Mock<IStrategy>();
            checkReturns.Setup(c => c.Execute(It.IsAny<object[]>())).Throws((new NullReferenceException()));
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Collision.CheckWithTree", (object[] args) => checkReturns.Object.Execute(args)).Execute();
            
            ICommand collisionCheck = new CollisionCheck(uObject1.Object, uObject2.Object);
            Assert.ThrowsAny<Exception>(() => collisionCheck.Execute());
        }
    }
}