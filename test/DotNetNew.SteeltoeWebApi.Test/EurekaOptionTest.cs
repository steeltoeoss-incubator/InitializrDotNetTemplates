using System.Collections.Generic;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class EurekaOptionTest : OptionTest
    {
        public EurekaOptionTest(ITestOutputHelper logger) : base("eureka", logger)
        {
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet(@"
--eureka  Add access to Eureka, a REST-based service for locating services.
          bool - Optional
          Default: false
");
        }

        protected override void AssertCsproj(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties, string[] packageRefs)
        {
            base.AssertCsproj(steeltoe, framework, properties, packageRefs);
            packageRefs.Should().Contain("Steeltoe.Discovery.ClientCore");
        }

        protected override void AssertStartupCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertStartupCs(steeltoe, framework, source);
            source.Should().ContainSnippet("services.AddDiscoveryClient(Configuration);");
            source.Should().ContainSnippet("app.UseDiscoveryClient();");
        }
    }
}
