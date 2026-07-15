$files = Get-ChildItem "TheBeautyHubData\Entities\*.cs"

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw

    # Remove [Column(TypeName = "datetime2(7)")] lines (with optional whitespace)
    $content = $content -replace '\r?\n\s*\[Column\(TypeName = "datetime2\(7\)"\)\]', ''

    Set-Content $file.FullName $content -NoNewline
}

Write-Host "Fixed all datetime2 TypeName attributes"
