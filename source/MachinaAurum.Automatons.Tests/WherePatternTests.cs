using MachinaAurum.Automatons.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace MachinaAurum.Automata.Tests
{
    [TestClass]
    public class WherePatternTests
    {
        [TestMethod]
        [Timeout(1000)]
        public async Task WherePatternA()
        {
            var subjectInput = new Subject<string>();
            var A = subjectInput.WherePattern(x => x.Match(i => i == "A"));

            var first = A.NextAsync();
            subjectInput.OnNext("A");
            Write(await first);

            var second = A.NextAsync(100, null);
            subjectInput.OnNext("A");
            var result = await second;
            Assert.IsNull(result);
            Write(result);
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task WherePatternEveryA()
        {
            var subjectInput = new Subject<string>();
            var As = subjectInput.WherePattern(x => x.Every(x2 => x2.Match(i => i == "A")));

            var first = As.NextAsync();
            subjectInput.OnNext("A");
            Write(await first);

            var second = As.NextAsync();
            subjectInput.OnNext("A");
            Write(await second);

            var third = As.NextAsync(100, null);
            subjectInput.OnNext("B");
            var result = await third;
            Assert.IsNull(result);
            Write(result);
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task WherePatternAB()
        {
            var subjectInput = new Subject<string>();
            var AB = subjectInput.WherePattern(x => x.Match(i => i == "A").Match(i => i == "B"));

            var first = AB.NextAsync();
            subjectInput.OnNext("A");
            subjectInput.OnNext("B");
            Write(await first);

            var second = AB.NextAsync(100, null);
            subjectInput.OnNext("A");
            subjectInput.OnNext("B");
            var result = await second;
            Assert.IsNull(result);
            Write(result);
        }

        [TestMethod]
        [Timeout(500)]
        public async Task WherePatternEveryAB()
        {
            var subjectInput = new Subject<string>();
            var ABs = subjectInput.WherePattern(x => x.Every(x2 => x2.Match(i => i == "A").Match(i => i == "B")));

            var first = ABs.NextAsync();
            subjectInput.OnNext("A");
            subjectInput.OnNext("B");
            Write(await first);

            var second = ABs.NextAsync();
            subjectInput.OnNext("A");
            subjectInput.OnNext("B");
            Write(await second);
        }

        [TestMethod]
        [Timeout(500)]
        public async Task WherePatternAEveryBC()
        {
            var subjectInput = new Subject<string>();
            var ABs = subjectInput.WherePattern(x => x.Match(i => i == "A").Every(x2 => x2.Match(i => i == "B").Match(i => i == "C")));

            var first = ABs.NextAsync();
            subjectInput.OnNext("A");
            subjectInput.OnNext("B");
            subjectInput.OnNext("C");
            Write(await first);

            var second = ABs.NextAsync();
            subjectInput.OnNext("B");
            subjectInput.OnNext("C");
            var result = await second;
            Write(result);
        }

        [TestMethod]
        [Timeout(500)]
        public async Task WherePatternAEveryBCThenD()
        {
            var subjectInput = new Subject<string>();
            var ABs = subjectInput.WherePattern(x => x.Match(i => i == "A").Every(x2 => x2.Match(i => i == "B").Match(i => i == "C")).Match(i => i == "D"));

            var first = ABs.NextAsync();
            subjectInput.OnNext("A");
            subjectInput.OnNext("B");
            subjectInput.OnNext("C");
            subjectInput.OnNext("D");
            await first;

            var second = ABs.NextAsync();
            subjectInput.OnNext("B");
            subjectInput.OnNext("C");
            subjectInput.OnNext("D");
            await second;
        }

        private void Write(IEnumerable<string> items)
        {
            Console.WriteLine("Received");
            Console.WriteLine("----------------------------");
            if (items == null)
            {
                Console.WriteLine("<Nothing>");
            }
            else
            {
                foreach (var item in items)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}
