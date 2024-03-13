
using System.Reflection;
using ООО_Компутир.Classes;

namespace ООО_Компутир___Тестирование
{
    public class Tests
    {
        [Test]
        public void TestSessionLengthCalculation()
        {
            var Session = new ООО_Компутир.Model.Session
            {
                SessionStartDateTime = DateTime.Parse("13.03.2024 10:30"),
                SessionEndDateTime = DateTime.Parse("13.03.2024 12:30")
                
            };

            var SessionExt = new SessionExtended { Session = Session };

            double expectedSessionLength = 2.0;

            double actualSessionLength = SessionExt.SessionLength;

            Assert.AreEqual(expectedSessionLength, actualSessionLength);
        }

        [Test]
        public void TestPCImagePathNotNull()
        {
            var PC = new ООО_Компутир.Model.PC();

            var PCExt = new PCExtended { PC = PC };

            Assert.IsNotEmpty(PCExt.PCImagePath);
        }

        [Test]
        public void TestPCResultCostCalculation()
        {
            var program = new ООО_Компутир.Model.Program { ProgramCost = 10 };
            List<ООО_Компутир.Model.Program> programs = new List<ООО_Компутир.Model.Program>();
            programs.Add(program);

            var PC = new ООО_Компутир.Model.PC { PCCost = 30, PCDiscount = 10, Program = programs };

            var PCExt = new PCExtended { PC = PC, programs = programs };

            double expectedResultCost = 40.0;

            double actualResultCost = PCExt.PCResultCost;

            Assert.AreEqual(expectedResultCost, actualResultCost);
        }

        [Test]
        public void TestPCDiscountCostCalculation()
        {
            var program = new ООО_Компутир.Model.Program { ProgramCost = 10 };
            List<ООО_Компутир.Model.Program> programs = new List<ООО_Компутир.Model.Program>();
            programs.Add(program);

            var PC = new ООО_Компутир.Model.PC { PCCost = 30, PCDiscount = 10, Program = programs };

            var PCExt = new PCExtended { PC = PC, programs = programs };

            double expectedDiscountCost = 36.0;

            double actualDiscountCost = PCExt.PCDiscountCost;

            Assert.AreEqual(expectedDiscountCost, actualDiscountCost);
        }

        [Test]
        public void TestSessionTotalCostCalculation()
        {
            var program = new ООО_Компутир.Model.Program { ProgramCost = 10 };
            List<ООО_Компутир.Model.Program> programs = new List<ООО_Компутир.Model.Program>();
            programs.Add(program);

            var PC = new ООО_Компутир.Model.PC { PCCost = 30, PCDiscount = 10, Program = programs };

            var PCExt = new PCExtended { PC = PC, programs = programs };

            var Session = new ООО_Компутир.Model.Session
            {
                SessionStartDateTime = DateTime.Parse("13.03.2024 10:30"),
                SessionEndDateTime = DateTime.Parse("13.03.2024 12:30")

            };

            var SessionExt = new SessionExtended { Session = Session, PCExt = PCExt };

            double expectedTotalCost = 72.0;

            double actualTotalCost = SessionExt.TotalCost;

            Assert.AreEqual(expectedTotalCost, actualTotalCost);
        }
    }
}