using System.Text.Json;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.Parsing;

namespace TheBeautyHub.Tests;

public class AndroidRequestBindingTests
{
    private static readonly JsonSerializerOptions Json = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public void Service_create_payload_binds_type_gender_and_branch_ids()
    {
        var json = """
            {
              "name": "haircut",
              "description": "hair cut des",
              "category": "Hair",
              "duration_minutes": 20,
              "applicable_gender": "Unisex",
              "type": "Service",
              "status": "Active",
              "customer_price": 120.0,
              "material_cost": 20.0,
              "commission_type": "Percentage",
              "commission_value": 50.0,
              "other_cost": 6.0,
              "home_service_available": false,
              "all_branches": false,
              "branch_ids": ["244ffded-2edc-4584-9716-955894c58da9"],
              "app_name": "The Beauty Hub",
              "platform": "android"
            }
            """;

        var request = JsonSerializer.Deserialize<SaveServiceRequest>(json, Json);
        Assert.NotNull(request);
        Assert.Equal("Service", request!.Type);
        Assert.Equal("Unisex", request.ApplicableGender);
        Assert.Equal("Percentage", request.CommissionType);
        Assert.Null(request.Branches);
        Assert.Equal(new[] { TestIds.Branch }, request.BranchIds);
        Assert.Equal(new[] { TestIds.Branch }, GuidListParser.Merge(request.Branches, request.BranchIds));
    }

    [Fact]
    public void Expense_update_payload_binds_branch_ids()
    {
        var json = """
            {
              "name": "Water Bottle",
              "description": "Water Bottle",
              "all_branches": false,
              "branch_ids": ["dec70dc9-e396-49e5-a359-5ef69864ef43"],
              "status": "active",
              "app_name": "The Beauty Hub",
              "platform": "android"
            }
            """;

        var request = JsonSerializer.Deserialize<SaveExpenseRequest>(json, Json);
        Assert.NotNull(request);
        Assert.False(request!.AllBranches);
        Assert.Equal(new[] { TestIds.OtherBranch }, GuidListParser.Merge(request.Branches, request.BranchIds));
    }

    [Fact]
    public void Salary_rule_payload_binds_android_salary_type()
    {
        var json = """
            {
              "name": "Incentive",
              "description": "Incentive",
              "salary_type": "Fixed Salary",
              "fixed_salary": 10000.0,
              "allow_advance_recovery": false,
              "status": "active"
            }
            """;

        var request = JsonSerializer.Deserialize<SaveSalaryRuleRequest>(json, Json);
        Assert.Equal("Fixed Salary", request!.SalaryType);
    }

    [Fact]
    public void Status_only_service_payload_has_no_name()
    {
        var request = JsonSerializer.Deserialize<SaveServiceRequest>("""{"status":"Inactive","app_name":"The Beauty Hub"}""", Json);
        Assert.Equal("Inactive", request!.Status);
        Assert.True(string.IsNullOrWhiteSpace(request.Name));
    }

    [Fact]
    public void Empty_json_array_token_is_valid_guid_list()
    {
        Assert.Empty(GuidListParser.Parse("[]"));
        Assert.Null(GuidListParser.Merge(GuidListParser.Parse("[]")));
    }

    [Fact]
    public void Staff_create_omits_username_and_binds_login_flags()
    {
        var json = """
            {
              "full_name": "Test Staff",
              "mobile": "9999999999",
              "email": "staff@example.com",
              "gender": "Female",
              "aadhaar_number": "123412341234",
              "designation": "Stylist",
              "specialist": "Hair",
              "branch_id": "244ffded-2edc-4584-9716-955894c58da9",
              "salary_rule_id": "31528a90-2678-4ba6-9f57-bfce2c4ae417",
              "status": "Active",
              "allow_app_login": true,
              "app_role": "staff"
            }
            """;
        var request = JsonSerializer.Deserialize<SaveStaffRequest>(json, Json);
        Assert.Null(request!.Username);
        Assert.True(request.AllowAppLogin);
        Assert.Equal("Female", request.Gender);
        Assert.Equal(TestIds.Branch, request.BranchId);
    }

    [Fact]
    public void Transaction_sale_payload_binds()
    {
        var json = $$"""
            {
              "idempotency_key": "idem-1",
              "type": "sale",
              "branch_id": "{{TestIds.Branch}}",
              "payment_mode": "upi",
              "services": [{ "service_id": "{{TestIds.Service}}", "quantity": 1, "staff_id": "{{TestIds.Staff}}" }],
              "customer_name": "Walk-in"
            }
            """;
        var request = JsonSerializer.Deserialize<SaveTransactionRequest>(json, Json);
        Assert.Equal("sale", request!.Type);
        Assert.Equal("upi", request.PaymentMode);
        Assert.Equal(TestIds.Branch, request.BranchId);
        Assert.Single(request.Services!);
        Assert.Equal(TestIds.Service, request.Services![0].ServiceId);
    }

    [Fact]
    public void Branch_status_only_payload_has_no_name()
    {
        var request = JsonSerializer.Deserialize<SaveBranchRequest>("""{"status":"Inactive","app_name":"The Beauty Hub"}""", Json);
        Assert.Equal("Inactive", request!.Status);
        Assert.True(string.IsNullOrWhiteSpace(request.Name));
    }
}
