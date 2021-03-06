﻿using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Payments.Application.Common.Interfaces;
using Payments.Domain.Common;
using Payments.Domain.Entities;
using Payments.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Common
{
    public static class ApplicationDbContextFactory
    {
        public static IApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var operationalStoreOptions = Options.Create(
                new OperationalStoreOptions
                {
                    DeviceFlowCodes = new TableConfiguration("DeviceCodes"),
                    PersistedGrants = new TableConfiguration("PersistedGrants")
                });

            var dateTimeMock = new Mock<IDateTime>();
            dateTimeMock.Setup(m => m.Now)
                .Returns(new DateTime(3001, 1, 1));

            var domainServiceMock = new Mock<IDomainEventService>();
            domainServiceMock.Setup(m => m.Publish(It.IsAny<DomainEvent>()))
                .Returns(Task.CompletedTask);

            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns("00000000-0000-0000-0000-000000000000");

            var context = new ApplicationDbContext(
                options, operationalStoreOptions,
                currentUserServiceMock.Object, dateTimeMock.Object, domainServiceMock.Object);

            context.Database.EnsureCreated();

            SeedSampleData(context);

            return context;
        }

        public static void SeedSampleData(ApplicationDbContext context)
        {
            context.Payments.AddRange(
                new Payment { Id = 1, Name = "Do this thing." },
                new Payment { Id = 2, Name = "Do this thing too." },
                new Payment { Id = 3, Name = "Do many, many things." },
                new Payment { Id = 4, Name = "This thing is done!", IsComplete = true }
            );

            context.SaveChanges();
        }

        public static void Destroy(IApplicationDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}