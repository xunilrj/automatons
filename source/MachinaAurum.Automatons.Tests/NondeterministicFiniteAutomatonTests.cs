using MachinaAurum.Automatons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MachinaAurum.Automaton.Tests
{
    [TestClass]
    public class NondeterministicAutomatonTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var automaton = new NondeterministicAutomaton<string>();
            automaton.AddState(1, "q1", start: true);
            automaton.AddState(2, "q2");
            automaton.AddState(3, "q3");
            automaton.AddState(4, "q4", accept: true);
            automaton.AddTransition(1, x => x == "0", 1);
            automaton.AddTransition(1, x => x == "1", 1);
            automaton.AddTransition(1, x => x == "1", 2);
            automaton.AddTransition(2, x => x == "0", 3);
            automaton.AddTransition(3, x => x == "1", 4);

            var instance = automaton.StartNew();

            AssertTransition(instance, "0", "q1");
            Assert.IsFalse(instance.IsAccepted);
            AssertTransition(instance, "1", "q1", "q2");
            Assert.IsFalse(instance.IsAccepted);
            AssertTransition(instance, "0", "q1", "q3");
            Assert.IsFalse(instance.IsAccepted);
            AssertTransition(instance, "1", "q1", "q2", "q4");
            Assert.IsTrue(instance.IsAccepted);
        }

        [TestMethod]
        public void Simplify()
        {
            var automaton = new NondeterministicAutomaton<string>();
            automaton.AddState(1, "q1", start: true);
            automaton.AddState(2, "q2");
            automaton.AddState(3, "q3");
            automaton.AddState(4, "q4", accept: true);
            automaton.AddEmptyTransition(1, 2);
            automaton.AddEmptyTransition(4, 1);
            automaton.AddTransition(2, x => x == "A", 3);
            automaton.AddTransition(3, x => x == "B", 4);
            automaton.ToConsoleWriteLine();
            automaton.Simplify();
            automaton.ToConsoleWriteLine();

            var instance = automaton.StartNew();
            instance.Send("A");
            instance.Send("B");

            Assert.IsTrue(instance.IsAccepted);
        }

        private static void AssertTransition(AutomatonInstance instance, string input, params string[] states)
        {
            instance.Send(input);

            var zip = instance.States.Zip(states, (a, b) => new { a, b });

            foreach (var item in zip)
            {
                Assert.AreEqual(item.b, item.a.Name);
            }
        }
    }
}
