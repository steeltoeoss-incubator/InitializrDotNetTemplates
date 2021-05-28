using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class PostgreSqlEfCoreOptionTest : OptionTest
    {
        public PostgreSqlEfCoreOptionTest(ITestOutputHelper logger) : base("postgresql-efcore",
            "Add access to PostgreSQL databases using Entity Framework Core", logger)
        {
        }

        [Fact]
        [Trait("Category", "Functional")]
        public async void TestDefaultNotPolluted()
        {
            var sandbox = await TemplateSandbox("false");
            sandbox.FileExists("Models/ErrorViewModel.cs").Should().BeFalse();
            sandbox.FileExists("Models/SampleContext.cs").Should().BeFalse();
        }

        protected override async Task AssertProject(Steeltoe steeltoe, Framework framework)
        {
            await base.AssertProject(steeltoe, framework);
            Logger.WriteLine("asserting Models/SampleContext.cs");
            var source = await Sandbox.GetFileTextAsync("Models/SampleContext.cs");
            source.Should().ContainSnippet("public class SampleContext : DbContext");
            Sandbox.FileExists("Models/ErrorViewModel.cs").Should().BeTrue();
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            packages.Add("Microsoft.EntityFrameworkCore");
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    packages.Add("Steeltoe.CloudFoundry.Connector.EFCore");
                    break;
                default:
                    packages.Add("Steeltoe.Connector.EFCore");
                    break;
            }
        }

        protected override void AddStartupCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    snippets.Add("using Steeltoe.CloudFoundry.Connector.PostgreSql.EFCore;");
                    break;
                default:
                    snippets.Add("using Steeltoe.Connector.PostgreSql.EFCore;");
                    break;
            }

            snippets.Add($"using {Sandbox.Name}.Models;");
            snippets.Add("services.AddDbContext<SampleContext>(options => options.UseNpgsql(Configuration));");
        }
    }
}
