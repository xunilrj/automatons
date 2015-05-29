using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MachinaAurum.Automatons;

namespace MachinaAurum.Automaton.Tests
{
    [TestClass]
    public class FiniteAutomatonTests
    {
        [TestMethod]
        public void FiniteAutomatonWorksWithoutManager()
        {
            var openClose = new FiniteAutomaton<string>();
            openClose.AddState(0, "closed", true);
            openClose.AddState(1, "open");
            openClose.AddTransition(0, x => x == "rear", 0);
            openClose.AddTransition(0, x => x == "both", 0);
            openClose.AddTransition(0, x => x == "neither", 0);
            openClose.AddTransition(0, x => x == "front", 1);

            openClose.AddTransition(1, x => x == "front", 1);
            openClose.AddTransition(1, x => x == "rear", 1);
            openClose.AddTransition(1, x => x == "both", 1);
            openClose.AddTransition(1, x => x == "neither", 0);

            var instance = openClose.StartNew();

            Assert.AreEqual("closed", instance.States.First().Name);

            AssertTransition(instance, "rear", "closed");
            AssertTransition(instance, "both", "closed");
            AssertTransition(instance, "neither", "closed");

            AssertTransition(instance, "front", "open");

            AssertTransition(instance, "front", "open");
            AssertTransition(instance, "rear", "open");
            AssertTransition(instance, "both", "open");

            AssertTransition(instance, "neither", "closed");

            openClose.ToConsoleSVG();
        }

        [TestMethod]
        public void FiniteAutomatonWorksWithManager()
        {
            var manager = new AutomatonManager();
            var openClose = manager.CreateNew<string>();
            openClose.AddState(0, "closed", true);
            openClose.AddState(1, "open");
            openClose.AddTransition(0, x => x == "rear", 0);
            openClose.AddTransition(0, x => x == "both", 0);
            openClose.AddTransition(0, x => x == "neither", 0);
            openClose.AddTransition(0, x => x == "front", 1);

            openClose.AddTransition(1, x => x == "front", 1);
            openClose.AddTransition(1, x => x == "rear", 1);
            openClose.AddTransition(1, x => x == "both", 1);
            openClose.AddTransition(1, x => x == "neither", 0);

            var instance = openClose.StartNew();

            Assert.AreEqual("closed", instance.States.First().Name);

            AssertTransition(instance, "rear", "closed");
            AssertTransition(instance, "both", "closed");
            AssertTransition(instance, "neither", "closed");

            instance = manager.Get<string>(openClose, instance.Id);

            AssertTransition(instance, "front", "open");

            AssertTransition(instance, "front", "open");
            AssertTransition(instance, "rear", "open");
            AssertTransition(instance, "both", "open");

            AssertTransition(instance, "neither", "closed");
        }

        private static void AssertTransition(AutomatonInstance<string> instance, string input, string state)
        {
            instance.Send(input);
            Assert.AreEqual(state, instance.States.First().Name);
        }
    }
}
