using AutoMapper;
using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Enums;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHub.Tests.Services;

public class PartnerServiceTests
{
    private readonly Mock<IPartnerRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly PartnerService _sut;

    public PartnerServiceTests() => _sut = new PartnerService(_repo.Object, _mapper.Object);

    [Fact]
    public async Task Create_normalizes_gender()
    {
        var dto = new CreatePartnerDto { Name = "Pat", Gender = "female", Email = "p@x.com" };
        _repo.Setup(r => r.GetByEmailAsync("p@x.com")).ReturnsAsync((Partner?)null);
        var entity = new Partner { Name = "Pat" };
        _mapper.Setup(m => m.Map<Partner>(dto)).Returns(entity);
        _repo.Setup(r => r.InsertAsync(entity)).ReturnsAsync(entity);
        _mapper.Setup(m => m.Map<PartnerDto>(entity)).Returns(new PartnerDto { Name = "Pat", Gender = PersonGender.Female.ToApiValue() });

        await _sut.CreateAsync(dto);

        Assert.Equal(PersonGender.Female.ToApiValue(), dto.Gender);
    }

    [Fact]
    public async Task Create_rejects_invalid_gender()
    {
        var dto = new CreatePartnerDto { Name = "Pat", Gender = "Alien" };
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.PartnerGenderInvalid, ex.Message);
    }

    [Fact]
    public async Task Create_rejects_empty_name()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(new CreatePartnerDto { Name = " " }));
    }

    [Fact]
    public async Task Create_rejects_duplicate_email()
    {
        _repo.Setup(r => r.GetByEmailAsync("a@b.com")).ReturnsAsync(new Partner { Email = "a@b.com" });
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _sut.CreateAsync(new CreatePartnerDto { Name = "A", Email = "a@b.com" }));
    }

    [Fact]
    public async Task Update_missing_is_not_found()
    {
        _repo.Setup(r => r.GetByIdAsync(TestIds.Staff)).ReturnsAsync((Partner?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.UpdateAsync(TestIds.Staff, new UpdatePartnerDto { Name = "A" }));
    }

    [Fact]
    public async Task Get_all_and_by_account_map()
    {
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(Array.Empty<Partner>());
        _repo.Setup(r => r.GetByAccountIdAsync(TestIds.Account)).ReturnsAsync(Array.Empty<Partner>());
        _mapper.Setup(m => m.Map<IEnumerable<PartnerDto>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(Array.Empty<PartnerDto>());
        Assert.Empty(await _sut.GetAllAsync());
        Assert.Empty(await _sut.GetByAccountIdAsync(TestIds.Account));
    }

    [Fact]
    public async Task Get_by_id_and_email_null_when_missing()
    {
        _repo.Setup(r => r.GetByIdAsync(TestIds.Staff)).ReturnsAsync((Partner?)null);
        _repo.Setup(r => r.GetByEmailAsync("x@y.com")).ReturnsAsync((Partner?)null);
        Assert.Null(await _sut.GetByIdAsync(TestIds.Staff));
        Assert.Null(await _sut.GetByEmailAsync("x@y.com"));
    }

    [Fact]
    public async Task Delete_returns_false_when_nothing_removed()
    {
        _repo.Setup(r => r.DeleteAsync(TestIds.Staff)).ReturnsAsync(0);
        Assert.False(await _sut.DeleteAsync(TestIds.Staff));
    }
}
