using TheBeautyHubCore.Enums;
using TheBeautyHubData.Enums;
using Xunit;

namespace TheBeautyHub.Tests;

public class EnumTextTests
{
    [Theory]
    [InlineData("Service")]
    [InlineData("in_salon")]
    [InlineData("in salon")]
    public void Service_type_android_and_legacy_values_parse_as_in_salon(string value)
    {
        Assert.True(ServiceOfferingTypes.TryParse(value, out var type));
        Assert.Equal(ServiceOfferingType.InSalon, type);
    }

    [Theory]
    [InlineData("home")]
    [InlineData("home_service")]
    public void Service_type_home_aliases_parse(string value)
    {
        Assert.True(ServiceOfferingTypes.TryParse(value, out var type));
        Assert.Equal(ServiceOfferingType.Home, type);
    }

    [Fact]
    public void Unknown_service_type_does_not_parse()
    {
        Assert.False(ServiceOfferingTypes.TryParse("catalog", out _));
    }

    [Theory]
    [InlineData("Unisex")]
    [InlineData("unisex")]
    [InlineData("UNISEX")]
    public void Service_gender_unisex_parses(string value)
    {
        Assert.True(ServiceGenders.TryParse(value, out var gender));
        Assert.Equal(ServiceGender.Unisex, gender);
    }

    [Theory]
    [InlineData("Fixed Salary", SalaryType.Fixed)]
    [InlineData("fixed", SalaryType.Fixed)]
    [InlineData("Fixed Pay", SalaryType.Fixed)]
    [InlineData("Hybrid", SalaryType.Hybrid)]
    [InlineData("fixed_plus_target", SalaryType.Hybrid)]
    [InlineData("Fixed + Target Bonus", SalaryType.Hybrid)]
    [InlineData("Service Commission", SalaryType.Commission)]
    [InlineData("commission", SalaryType.Commission)]
    [InlineData("Incentive", SalaryType.Commission)]
    public void Salary_type_client_and_stored_values_parse(string value, SalaryType expected)
    {
        Assert.True(SalaryTypes.TryParse(value, out var type));
        Assert.Equal(expected, type);
    }

    [Fact]
    public void Salary_type_api_values_are_android_labels()
    {
        Assert.Equal("Fixed Salary", SalaryType.Fixed.ToApiValue());
        Assert.Equal("Hybrid", SalaryType.Hybrid.ToApiValue());
        Assert.Equal("Service Commission", SalaryType.Commission.ToApiValue());
    }

    [Fact]
    public void Stored_legacy_salary_type_maps_to_android_label()
    {
        Assert.True(SalaryTypes.TryParse("fixed_plus_target", out var type));
        Assert.Equal("Hybrid", type.ToApiValue());
    }

    [Theory]
    [InlineData("Active")]
    [InlineData("active")]
    [InlineData("Inactive")]
    public void Record_status_android_casing_parses(string value)
    {
        Assert.True(RecordStatuses.TryParse(value, out _));
    }

    [Theory]
    [InlineData("Percentage")]
    [InlineData("percent")]
    [InlineData("Fixed Amount")]
    [InlineData("flat")]
    public void Commission_type_aliases_parse(string value)
    {
        Assert.True(CommissionTypes.TryParse(value, out _));
    }

    [Theory]
    [InlineData("cash", PaymentMode.Cash)]
    [InlineData("UPI", PaymentMode.Upi)]
    [InlineData("card", PaymentMode.Card)]
    public void Payment_modes_parse(string value, PaymentMode expected)
    {
        Assert.True(EnumText.TryParse(value, out PaymentMode mode));
        Assert.Equal(expected, mode);
        Assert.Equal(mode.ToApiValue(), EnumText.ParseOrThrow<PaymentMode>(value, "bad").ToApiValue());
    }

    [Theory]
    [InlineData("sale", TransactionKind.Sale)]
    [InlineData("expense", TransactionKind.Expense)]
    public void Transaction_kinds_parse(string value, TransactionKind expected)
    {
        Assert.True(EnumText.TryParse(value, out TransactionKind kind));
        Assert.Equal(expected, kind);
    }

    [Theory]
    [InlineData("Male")]
    [InlineData("female")]
    [InlineData("Other")]
    public void Person_genders_parse(string value)
    {
        Assert.True(EnumText.TryParse(value, out PersonGender _));
    }

    [Fact]
    public void Every_data_enum_round_trips_api_value()
    {
        RoundTrip<RecordStatus>();
        RoundTrip<ServiceGender>();
        RoundTrip<ServiceOfferingType>();
        RoundTrip<CommissionType>();
        RoundTrip<SalaryType>();
        RoundTrip<PersonGender>();
        RoundTrip<PaymentMode>();
        RoundTrip<TransactionKind>();
        RoundTrip<TransactionStatus>();
        RoundTrip<TransactionListPeriod>();
        RoundTrip<CurrencyCode>();
        RoundTrip<SubscriptionStatus>();
    }

    [Fact]
    public void Feature_lock_codes_include_android_typo()
    {
        Assert.True(FeatureLockCodes.TryParse("create_transcation", out var lockCode));
        Assert.Equal(FeatureLock.CreateTransaction, lockCode);
        Assert.Equal("report", FeatureLock.Report.ToApiCode());
    }

    private static void RoundTrip<T>() where T : struct, Enum
    {
        foreach (T value in Enum.GetValues<T>())
        {
            var api = value.ToApiValue();
            Assert.True(EnumText.TryParse(api, out T parsed), $"{typeof(T).Name} {value} -> {api}");
            Assert.Equal(value, parsed);
        }
    }
}
