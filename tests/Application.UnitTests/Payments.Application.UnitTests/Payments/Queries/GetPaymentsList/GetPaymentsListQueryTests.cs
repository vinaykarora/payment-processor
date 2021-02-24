﻿using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Interfaces;
using Payments.Application.Common.Mappings;
using Payments.Application.Payments.Queries.GetPaymentsList;
using Payments.Application.UnitTests.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Payments.Queries.GetPaymentsList
{
    public class GetPaymentsListQueryTests
    {
        private readonly IApplicationDbContext _context;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public GetPaymentsListQueryTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _context = ApplicationDbContextFactory.Create();
            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public async Task Handle_ReturnsCorrectVmAndPaymentsCount()
        {
            var sut = new GetPaymentsListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPaymentsListQuery(), CancellationToken.None);

            result.Should().BeOfType<PaymentsListVm>();
            result.Payments.Count.Should().Be(4);
        }
    }
}