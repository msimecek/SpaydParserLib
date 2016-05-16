# SpaydParserLib
Knihovna pro parsování řetězce SPAYD (Short Payment Descriptor) nebo SIND (Short Invoice Descriptor) na objekty C# podle specifikace [QR platba](http://qr-platba.cz/pro-vyvojare/specifikace-formatu/) a [QR faktura](http://qr-faktura.cz/popis-formatu). 

## Použití

```csharp
Spayd result = Spayd.FromString(spaydString);
```

```csharp
Sind result = Sind.FromString(sindString);
```
