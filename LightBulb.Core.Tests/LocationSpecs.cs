﻿using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LightBulb.Core.Tests;

public class LocationSpecs
{
    public static TheoryData<string?, GeoLocation?> LocationParseTestCases => new()
    {
        // Valid

        {"41.25 -120.9762", new GeoLocation(41.25, -120.9762)},
        {"41.25, -120.9762", new GeoLocation(41.25, -120.9762)},
        {"41.25,-120.9762", new GeoLocation(41.25, -120.9762)},
        {"-41.25, -120.9762", new GeoLocation(-41.25, -120.9762)},
        {"41.25, 120.9762", new GeoLocation(41.25, 120.9762)},
        {"41.25°N, 120.9762°W", new GeoLocation(41.25, -120.9762)},
        {"41.25N 120.9762W", new GeoLocation(41.25, -120.9762)},
        {"41.25N, 120.9762W", new GeoLocation(41.25, -120.9762)},
        {"41.25 N, 120.9762 W", new GeoLocation(41.25, -120.9762)},
        {"41.25 S, 120.9762 W", new GeoLocation(-41.25, -120.9762)},
        {"41.25 S, 120.9762 E", new GeoLocation(-41.25, 120.9762)},
        {"41, 120", new GeoLocation(41, 120)},
        {"-41, -120", new GeoLocation(-41, -120)},
        {"41 N, 120 E", new GeoLocation(41, 120)},
        {"41 N, 120 W", new GeoLocation(41, -120)},

        // Invalid

        {"41.25; -120.9762", null},
        {"-41.25 S, 120.9762 E", null},
        {"41.25", null},
        {"", null},
        {null, null}
    };

    [Theory]
    [MemberData(nameof(LocationParseTestCases))]
    public void Location_can_be_resolved_from_coordinates(string? str, GeoLocation? expectedResult)
    {
        // Act & assert
        GeoLocation.TryParse(str).Should().Be(expectedResult);
    }

    [Fact]
    public async Task Location_can_be_resolved_from_IP()
    {
        // Act
        var location = await GeoLocation.GetCurrentAsync();

        // Assert
        location.Latitude.Should().NotBe(default);
        location.Longitude.Should().NotBe(default);
    }

    [Fact]
    public async Task Location_can_be_resolved_using_a_search_query()
    {
        // Act
        var location = await GeoLocation.GetAsync("Kyiv, Ukraine");

        // Assert
        location.Latitude.Should().BeApproximately(50.4500, 0.01);
        location.Longitude.Should().BeApproximately(30.5241, 0.01);
    }
}
