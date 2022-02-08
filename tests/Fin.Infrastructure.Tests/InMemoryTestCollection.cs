using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fin.Infrastructure.Tests
{
    [CollectionDefinition("InMemoryTest collection")]
    public class InMemoryTestCollection : ICollectionFixture<InMemoryTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
