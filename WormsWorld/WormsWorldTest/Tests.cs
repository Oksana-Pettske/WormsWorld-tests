using NUnit.Framework;
using WormsWorld.Mover;
using WormsWorld.Writer;
using WormsWorld.Entity;
using WormsWorld.Generator.Food;
using WormsWorld.Generator.Name;
using WormsWorld.WorldSimulator;

namespace WormsWorldTest
{
    [TestFixture]
    public class Tests
    {
        private WorldService _worldService;
        private FoodGenerator _foodGenerator;
        private FileWriter _fileWriter;
        private WormMover _wormMover;
        
        [SetUp]
        public void Setup()
        {
            _foodGenerator = new FoodGenerator();
            _fileWriter = new FileWriter("AboutWormsTest.txt");
            _wormMover = new WormMover();
            _worldService = new WorldService(_wormMover, _fileWriter, _foodGenerator, new NameGenerator());
        }

        [TearDown]
        public void Teardown()
        {
            _worldService.Worms.Clear();
            _worldService.Foods.Clear();
            _fileWriter.Dispose();
        }

        [Test]
        public void EmptyCellMoveTest()
        {
            var worm = _worldService.CreateWorm();
            var food = new Food(new Cell(0, 100));
            _worldService.Foods.Add(food);
            var before = worm.Position;
            worm.Move();
            var after = worm.Position;
            Assert.True(!before.Equals(after));
        }
        
        [Test]
        public void FoodCellMoveTest()
        {
            var worm = _worldService.CreateWorm();
            var food = new Food(new Cell(0, 1));
            _worldService.Foods.Add(food);
            var before = worm.Vitality;
            worm.Move();
            _worldService.IsFoodEaten();
            var after = worm.Vitality;
            Assert.True(worm.Position.Equals(food.Position));
            Assert.AreEqual(after, before + 10);
        }

        [Test]
        public void SuccessfulMultiplyTest()
        {
            var worm = _worldService.CreateWorm();
            worm.Vitality = 11;
            var before = _worldService.Worms.Count;
            _worldService.Multiply(worm);
            var after = _worldService.Worms.Count;
            Assert.True(before != after);
        }
        
        [Test]
        public void UnsuccessfulMultiplyTest()
        {
            var worm = _worldService.CreateWorm();
            worm.Vitality = 9;
            var before = _worldService.Worms.Count;
            _worldService.Multiply(worm);
            var after = _worldService.Worms.Count;
            Assert.True(before == after);
        }
        
        [Test]
        public void WormNameTest()
        {
            var worm1 = _worldService.CreateWorm();
            var worm2 = _worldService.CreateWorm(0, 1);
            Assert.True(!worm1.Name.Equals(worm2.Name));
        }
        
        [Test]
        public void UniqueFoodCellTest()
        {
            _foodGenerator.GenerateFood(_worldService);
            _foodGenerator.GenerateFood(_worldService);
            Assert.True(!_worldService.Foods[0].Position.Equals(_worldService.Foods[1].Position));
        }

        [Test]
        public void ClosestFoodCellMove()
        {
            var worm = _worldService.CreateWorm();
            var food1 = new Food(new Cell(0, 100));
            var food2 = new Food(new Cell(200, 0));
            var food3 = new Food(new Cell(0, -300));
            var food4 = new Food(new Cell(-4, 0));
            _worldService.Foods.Add(food1);
            _worldService.Foods.Add(food2);
            _worldService.Foods.Add(food3);
            _worldService.Foods.Add(food4);
            
            worm.Move();
            Assert.AreEqual(-1, worm.Position.X);
            Assert.AreEqual(0, worm.Position.Y);
        }
    }
}