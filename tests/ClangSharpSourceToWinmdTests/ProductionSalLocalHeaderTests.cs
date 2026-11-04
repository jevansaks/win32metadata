using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class ProductionSalLocalHeaderTests
    {
        internal static string PreCutoverCommit()
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/ProductionSalLocalHeaderTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable production /sal_local cutover.");
            var parent = LastErrorHeaderTests.Run("git", "-C", root, "rev-parse", commits[0] + "^");
            Assert.AreEqual(0, parent.ExitCode, parent.Output);
            return parent.Output.Trim();
        }

        internal static byte[] ReadBeforeCutover(string path) =>
            PropertyComOutputHeaderTests.ReadOriginalBytes(path, PreCutoverCommit());

        [TestMethod]
        public void HistoricalSnapshot_IsTheUniqueReachableCutoverParent()
        {
            var ancestry = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(),
                "merge-base", "--is-ancestor", PreCutoverCommit(), "HEAD");
            Assert.AreEqual(0, ancestry.ExitCode, ancestry.Output);
        }
    }
}
