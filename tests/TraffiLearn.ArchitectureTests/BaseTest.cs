﻿using System.Reflection;
using TraffiLearn.Infrastructure.Persistence;
using TraffiLearn.SharedKernel.Primitives;

namespace TraffiLearn.ArchitectureTests
{
    public abstract class BaseTest
    {
        protected static readonly Assembly DomainAssembly = typeof(Entity<>).Assembly;

        protected static readonly Assembly ApplicationAssembly = typeof(TraffiLearn.Application.DependencyInjection).Assembly;

        protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;

        protected static readonly Assembly WebApiAssembly = typeof(WebAPI.Program).Assembly;

        protected static readonly Assembly DomainTestsAssembly = typeof(DomainTests.ReferenceFile).Assembly;
    }
}
