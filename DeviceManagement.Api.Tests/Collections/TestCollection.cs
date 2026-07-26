using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeviceManagement.Api.Tests.Collections
{
    [CollectionDefinition(
     "Integration Test Collection",
     DisableParallelization = true)]
    public class IntegrationTestCollection
    {

    }
}
