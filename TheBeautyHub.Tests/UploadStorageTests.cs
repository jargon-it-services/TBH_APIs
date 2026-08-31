using Microsoft.AspNetCore.Http;
using TheBeautyHubAPI.Helpers;
using TheBeautyHubCore.Constants;
using TheBeautyHub.Tests.TestData;

namespace TheBeautyHub.Tests;

public class UploadStorageTests
{
    private static IFormFile File(string name, long length)
    {
        var mock = new Mock<IFormFile>();
        mock.SetupGet(f => f.FileName).Returns(name);
        mock.SetupGet(f => f.Length).Returns(length);
        mock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        return mock.Object;
    }

    [Fact]
    public async Task Branch_logo_rejects_wrong_type_and_empty_file()
    {
        var storage = ApiTest.BranchLogos();
        Assert.Equal(ApiMessages.InvalidImageType("logo"),
            (await Assert.ThrowsAsync<ArgumentException>(() => storage.SaveAsync(File("logo.exe", 10)))).Message);
        Assert.Equal(ApiMessages.FileTooLargeFor("logo"),
            (await Assert.ThrowsAsync<ArgumentException>(() => storage.SaveAsync(File("logo.png", 0)))).Message);
    }

    [Fact]
    public async Task Service_photo_rejects_wrong_type()
    {
        var storage = ApiTest.ServicePhotos();
        Assert.Equal(ApiMessages.InvalidImageType("photo"),
            (await Assert.ThrowsAsync<ArgumentException>(() => storage.SaveAsync(File("cut.bmp", 10)))).Message);
    }

    [Fact]
    public async Task Staff_files_accept_pdf_for_aadhaar_and_reject_exe()
    {
        var storage = ApiTest.StaffFiles();
        Assert.Equal(ApiMessages.InvalidImageOrPdfType("aadhaar card"),
            (await Assert.ThrowsAsync<ArgumentException>(() => storage.SaveAadhaarAsync(File("id.exe", 10)))).Message);
        Assert.Equal(ApiMessages.InvalidImageOrPdfType("photo"),
            (await Assert.ThrowsAsync<ArgumentException>(() => storage.SavePhotoAsync(File("face.exe", 10)))).Message);
    }
}
